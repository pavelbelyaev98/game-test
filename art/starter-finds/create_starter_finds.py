"""Original starter props. Execute through Blender MCP; owns SDT_Starter only."""
import bpy
import bmesh
import math
import json
import random
from pathlib import Path
from mathutils import Vector

ROOT = Path(__file__).resolve().parents[2]
OUT = ROOT / 'art/starter-finds'
PREFIX = 'SDT_Starter'
ATLAS_SIZE = 2048
# Editable catalog is the single owner of item identity, sizing and tuning.
SPECS = [(v['content_id'], v['display_name'], v['atlas_group'], tuple(v['dimensions_m']), v['instances'], v['sale_value'])
         for v in json.loads((OUT/'catalog.json').read_text(encoding='utf-8'))['variants']]


def rgba(h):
    h = h.lstrip('#')
    rgb = [int(h[i:i+2], 16)/255 for i in (0, 2, 4)]
    return tuple(v/12.92 if v <= .04045 else ((v+.055)/1.055)**2.4 for v in rgb)+(1,)


def node(mat, kind, name):
    n = mat.node_tree.nodes.new(kind)
    n.name = n.label = name
    return n


def wire(mat, a, b):
    if hasattr(a, 'node'):
        mat.node_tree.links.new(a, b)
    else:
        b.default_value = a


def calc(mat, op, a, b=0):
    n = node(mat, 'ShaderNodeMath', op)
    n.operation = op
    wire(mat, a, n.inputs[0]); wire(mat, b, n.inputs[1])
    return n.outputs[0]


def mix(mat, fac, a, b, name):
    n = node(mat, 'ShaderNodeMixRGB', name)
    wire(mat, fac, n.inputs[0]); wire(mat, a, n.inputs[1]); wire(mat, b, n.inputs[2])
    return n.outputs[0]


def ramp(mat, socket, entries, name):
    n = node(mat, 'ShaderNodeValToRGB', name)
    n.color_ramp.interpolation = 'EASE'
    for e in list(n.color_ramp.elements)[2:]:
        n.color_ramp.elements.remove(e)
    for i, (at, color) in enumerate(entries):
        e = n.color_ramp.elements[i] if i < 2 else n.color_ramp.elements.new(at)
        e.position = at; e.color = rgba(color)
    wire(mat, socket, n.inputs[0])
    return n.outputs['Color']


def noise(mat, coords, scale, name, detail=1):
    n = node(mat, 'ShaderNodeTexNoise', name)
    n.inputs['Scale'].default_value = scale
    n.inputs['Detail'].default_value = detail
    n.inputs['Roughness'].default_value = .65
    wire(mat, coords, n.inputs['Vector'])
    return n.outputs['Fac']


def material(name, colors, rough=.6, metal=0, kind='clay', label=False):
    m = bpy.data.materials.new(PREFIX+'_'+name)
    m.use_nodes = True; m.use_fake_user = True
    m.node_tree.nodes.clear()
    tex = node(m, 'ShaderNodeTexCoord', 'Stable object coordinates')
    coords = tex.outputs['Generated']
    if kind == 'clay':
        aspect=node(m,'ShaderNodeVectorMath','Equal scale clay patches');aspect.operation='MULTIPLY'
        wire(m,coords,aspect.inputs[0]);aspect.inputs[1].default_value=(2.3,1.1,.7);coords=aspect.outputs[0]
    xyz = node(m, 'ShaderNodeSeparateXYZ', 'Height and front')
    wire(m, coords, xyz.inputs[0])
    broad = noise(m, coords, 4.2, 'Broad painted patches', .8)
    fine = noise(m, coords, 19 if kind == 'clay' else 12, 'Restrained surface wear', 1.2)
    color = ramp(m, broad, list(zip((.23, .48, .73), colors)), 'Painterly palette')
    color = mix(m, calc(m, 'MULTIPLY', fine, .16), color, rgba(colors[-1]), 'Soft scuffs')
    if kind == 'glass':
        # Very broad clean-to-dusty change; no transparent sorting dependency.
        dust = calc(m, 'MULTIPLY', calc(m, 'LESS_THAN', xyz.outputs['Z'], .12), .24)
        color = mix(m, dust, color, rgba('a99978'), 'Worn dusty foot')
    if label:
        z = calc(m, 'ADD', xyz.outputs['Z'], calc(m, 'MULTIPLY', broad, .045))
        band = calc(m, 'MULTIPLY', calc(m, 'GREATER_THAN', z, .31), calc(m, 'LESS_THAN', z, .66))
        if kind == 'glass':
            band = calc(m, 'MULTIPLY', band, calc(m, 'LESS_THAN', xyz.outputs['Y'], .13))
        # Broken edges preserve a plain faded patch, not a readable product label.
        band = calc(m, 'MULTIPLY', band, calc(m, 'GREATER_THAN', fine, .30))
        paper = ramp(m, broad, [(.25, 'b3a48a'), (.72, 'e4d8b1')], 'Unbranded worn paper')
        color = mix(m, band, color, paper, 'Faded label fragment')
    p = node(m, 'ShaderNodeBsdfPrincipled', 'Original Surface')
    wire(m, color, p.inputs['Base Color'])
    roughval = calc(m, 'ADD', rough-.075, calc(m, 'MULTIPLY', fine, .15))
    wire(m, roughval, p.inputs['Roughness'])
    metalval = metal
    if kind == 'metal':
        rusty = calc(m, 'LESS_THAN', broad, .36)
        rustcolor = ramp(m, fine, [(.2, '714a3b'), (.72, 'b1794d')], 'Broad oxidized patches')
        color = mix(m, rusty, color, rustcolor, 'Dull steel and rust')
        wire(m, color, p.inputs['Base Color'])
        metalval = calc(m, 'MULTIPLY', calc(m, 'SUBTRACT', 1, rusty), metal)
    wire(m, metalval, p.inputs['Metallic'])
    if kind == 'glass':
        p.inputs['Coat Weight'].default_value = .36
        p.inputs['Coat Roughness'].default_value = .21
        p.inputs['IOR'].default_value = 1.46
    bump = node(m, 'ShaderNodeBump', 'Small sculpted material variation')
    bump.inputs['Strength'].default_value = .17 if kind == 'glass' else .28
    bump.inputs['Distance'].default_value = .00035 if kind == 'glass' else .0013
    wire(m, fine, bump.inputs['Height']); wire(m, bump.outputs[0], p.inputs['Normal'])
    color_out = node(m, 'ShaderNodeEmission', 'Color Bake')
    wire(m, color, color_out.inputs['Color'])
    masks = node(m, 'ShaderNodeCombineColor', 'Metal AO Roughness')
    wire(m, metalval, masks.inputs[0]); masks.inputs[1].default_value = 1
    wire(m, roughval, masks.inputs[2])
    mask_out = node(m, 'ShaderNodeEmission', 'Masks Bake')
    wire(m, masks.outputs[0], mask_out.inputs['Color'])
    output = node(m, 'ShaderNodeOutputMaterial', 'Output')
    wire(m, p.outputs[0], output.inputs['Surface'])
    m.diffuse_color = rgba(colors[1])
    # Give editable graphs a readable basic arrangement.
    for i, n in enumerate(m.node_tree.nodes):
        n.location = ((i % 7)*210, -(i//7)*230)
    return m


def scene():
    return bpy.data.scenes[PREFIX+'_Studio']


def models():
    return [bpy.data.objects[PREFIX+'_'+s[0]] for s in SPECS]


def activate(objects):
    if bpy.context.mode != 'OBJECT': bpy.ops.object.mode_set(mode='OBJECT')
    bpy.ops.object.select_all(action='DESELECT')
    for obj in objects: obj.select_set(True)
    if objects: bpy.context.view_layer.objects.active = objects[-1]


def own_object(obj, collection):
    for c in list(obj.users_collection): c.objects.unlink(obj)
    collection.objects.link(obj)
    if obj.data and not obj.data.name.startswith(PREFIX):obj.data.name=PREFIX+'_'+obj.data.name


def finish(obj, ident, dims, group):
    obj.name = PREFIX+'_'+ident
    obj.data.name=PREFIX+'_'+ident+'_Mesh'
    activate([obj])
    bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
    bpy.context.view_layer.update()
    obj.dimensions = dims
    bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
    minimum = min(v.co.z for v in obj.data.vertices)
    for v in obj.data.vertices: v.co.z -= minimum
    obj['content_id'] = ident; obj['atlas_group'] = group
    obj['source_materials'] = [m.name for m in obj.data.materials]
    obj['source_material_indices'] = [f.material_index for f in obj.data.polygons]
    obj['detector_eligible'] = False
    obj['category'] = next(s[1] for s in SPECS if s[0] == ident)
    return obj


def bottle(ident, dims, mat, collection, squat=False, square=False):
    # Cross-section walks the outside, thick rolled rim, inner wall and inner floor.
    if squat:
        profile = [(0, .10), (.018,.92), (.042,1), (.60,1), (.67,.96), (.73,.72),
                   (.78,.36), (.88,.34), (.90,.40), (.93,.40), (.94,.35), (1,.35),
                   (1,.23), (.93,.23), (.84,.25), (.77,.29), (.70,.66), (.64,.86), (.08,.86), (.065,.10)]
    else:
        profile = [(0,.12), (.012,.83), (.028,.98), (.054,1), (.60,1), (.66,.97),
                   (.72,.78), (.78,.43), (.83,.32), (.93,.32), (.945,.40), (.98,.40),
                   (1,.34), (1,.23), (.96,.22), (.86,.22), (.79,.30), (.73,.65),
                   (.65,.85), (.08,.85), (.055,.12)]
    seg = 32
    verts=[]; faces=[]
    for j,(z,r) in enumerate(profile):
        for k in range(seg):
            a=2*math.pi*k/seg
            c,s=math.cos(a),math.sin(a)
            squareness=max(0,min(1,(.78-z)*7)) if square else 0
            exponent=1-.48*squareness
            x=math.copysign(abs(c)**exponent,c)*r*dims[0]/2
            y=math.copysign(abs(s)**exponent,s)*r*dims[1]/2
            # A restrained hand-formed shoulder; lip still reads cleanly.
            irregular=1+.008*math.sin(a*3+.7)*math.sin(z*math.pi)
            verts.append((x*irregular,y*irregular,z*dims[2]))
    for j in range(len(profile)-1):
        for k in range(seg):
            n=(k+1)%seg
            faces.append((j*seg+k,j*seg+n,(j+1)*seg+n,(j+1)*seg+k))
    faces.append(tuple(reversed(range(seg))))
    faces.append(tuple((len(profile)-1)*seg+k for k in range(seg)))
    mesh=bpy.data.meshes.new(PREFIX+'_'+ident+'_Mesh'); mesh.from_pydata(verts,[],faces); mesh.update()
    obj=bpy.data.objects.new(PREFIX+'_'+ident,mesh); collection.objects.link(obj)
    obj.data.materials.append(mat)
    for f in mesh.polygons: f.use_smooth=True
    return finish(obj,ident,dims,'Bottles')


def atlas_uvs(group):
    obs=[o for o in models() if o['atlas_group']==group]
    activate(obs)
    bpy.ops.object.mode_set(mode='EDIT');bpy.ops.mesh.select_all(action='SELECT')
    bpy.ops.uv.smart_project(angle_limit=math.radians(66),island_margin=.015,correct_aspect=True)
    bpy.ops.uv.select_all(action='SELECT')
    bpy.ops.uv.pack_islands(rotate=True,margin=.014)
    bpy.ops.object.mode_set(mode='OBJECT')
    for o in obs:o.data.uv_layers.active.name='UVMap'


def look_at(obj,point):
    obj.rotation_euler=(Vector(point)-obj.location).to_track_quat('-Z','Y').to_euler()


def stage():
    sc=scene();coll=bpy.data.collections[PREFIX+'_PreviewOnly']
    locations=[(-.28,.08,0),(0,.08,0),(.28,.08,0)]
    for o,pos in zip(models(),locations):o.location=pos
    # Turn the square bottle to show its broad face and depth.
    models()[2].rotation_euler.z=math.radians(-12)
    bpy.ops.mesh.primitive_plane_add(size=200,location=(0,0,-.002))
    floor=bpy.context.object;floor.name=PREFIX+'_PreviewFloor';own_object(floor,coll)
    floor.data.materials.append(material('StudioBackdrop',['252c30','30383a','3b4445'],.9))
    world=bpy.data.worlds.new(PREFIX+'_StudioWorld');world.use_nodes=True
    world.node_tree.nodes['Background'].inputs[0].default_value=(.22,.26,.30,1)
    world.node_tree.nodes['Background'].inputs[1].default_value=.5;sc.world=world
    for name,pos,power,size,color in [
        ('Key',(-.7,-1.6,2.4),180,2.0,(1,.88,.73)),
        ('Fill',(1,-.6,1.1),90,1.5,(.73,.86,1)),
        ('Rim',(.2,1.5,2),260,1.7,(1,.90,.72))]:
        light=bpy.data.lights.new(PREFIX+'_'+name,'AREA');light.energy=power;light.shape='DISK';light.size=size;light.color=color
        ob=bpy.data.objects.new(PREFIX+'_'+name,light);coll.objects.link(ob);ob.location=pos;look_at(ob,(0,0,.1))
    cam_data=bpy.data.cameras.new(PREFIX+'_Camera');cam=bpy.data.objects.new(PREFIX+'_Camera',cam_data);coll.objects.link(cam)
    cam.location=(.25,-2.8,1.95);look_at(cam,(.04,.08,.11));cam_data.type='ORTHO';cam_data.ortho_scale=2.52;cam_data.lens=50
    sc.camera=cam
    sc.render.engine='BLENDER_EEVEE';sc.render.resolution_x=2100;sc.render.resolution_y=900;sc.render.resolution_percentage=100
    sc.render.image_settings.file_format='PNG';sc.render.film_transparent=False
    sc.view_settings.view_transform='AgX'
    sc.view_settings.exposure=-.35
    try:sc.view_settings.look='AgX - Medium High Contrast'
    except TypeError:pass
    sc.render.threads_mode='FIXED';sc.render.threads=8


def create():
    if PREFIX+'_Studio' in bpy.data.scenes:
        raise RuntimeError('Owned scene already exists; use existing source or explicitly revise it, not a duplicate.')
    for name in ('exports','textures','previews'):(OUT/name).mkdir(parents=True,exist_ok=True)
    old=bpy.context.scene
    sc=bpy.data.scenes.new(PREFIX+'_Studio');bpy.context.window.scene=sc
    sc.unit_settings.system='METRIC';sc.unit_settings.scale_length=1
    for suffix in ('Models','PreviewOnly'):
        c=bpy.data.collections.new(PREFIX+'_'+suffix);sc.collection.children.link(c)
    coll=bpy.data.collections[PREFIX+'_Models']
    green=material('GreenGlass',['315846','60836b','9cab86'],.28,kind='glass',label=True)
    amber=material('AmberGlass',['684526','a57845','cfa16a'],.29,kind='glass')
    blue=material('BlueGlass',['365c60','648b87','a2b7a1'],.27,kind='glass',label=True)
    bottle(SPECS[0][0],SPECS[0][3],green,coll)
    bottle(SPECS[1][0],SPECS[1][3],amber,coll,squat=True)
    bottle(SPECS[2][0],SPECS[2][3],blue,coll,square=True)
    for group in ('Bottles',):atlas_uvs(group)
    stage();activate(models())
    for area in bpy.context.screen.areas:
        if area.type=='VIEW_3D':
            area.spaces.active.region_3d.view_perspective='CAMERA'
            area.spaces.active.shading.type='MATERIAL'
    save_source()
    return {'scene':sc.name,'preserved_scene':old.name,'models':[o.name for o in models()]}


def save_source():
    # Write only the new scene and its dependencies; preserve the user's live file path.
    blocks={s for s in bpy.data.scenes if s.name.startswith(PREFIX)}|{m for m in bpy.data.materials if m.name.startswith(PREFIX)}
    bpy.data.libraries.write(str(OUT/'StarterFinds.blend'),blocks,path_remap='RELATIVE_ALL',fake_user=True,compress=True)


def preview(name='overview',group='Bottles'):
    sc=scene();cam=sc.camera
    for o in models():o.hide_render=group is not None and o['atlas_group']!=group
    if group:
        obs=[o for o in models() if not o.hide_render]
        x=sum(o.location.x for o in obs)/len(obs)
        span=max(o.location.x+o.dimensions.x*.5 for o in obs)-min(o.location.x-o.dimensions.x*.5 for o in obs)
        cam.location=(x+.05,-1.8,1.05);look_at(cam,(x,.08,.12 if group=='Bottles' else .06))
        cam.data.ortho_scale=max(.72,span+.22)
        sc.render.resolution_x=1600;sc.render.resolution_y=1100
    else:
        cam.location=(.25,-2.8,1.95);look_at(cam,(.04,.08,.11));cam.data.ortho_scale=2.52
        sc.render.resolution_x=2100;sc.render.resolution_y=900
    sc.render.engine='BLENDER_EEVEE';sc.render.filepath=str(OUT/'previews'/f'{name}.png')
    bpy.ops.render.render(write_still=True)
    return {'render':sc.render.filepath}


def source_mats(group):
    names={name for o in models() if o['atlas_group']==group for name in o['source_materials']}
    return [bpy.data.materials[n] for n in sorted(names)]


def bake(group,channel):
    sc=scene();sc.render.engine='CYCLES';sc.cycles.samples=4;sc.cycles.device='CPU'
    obs=[o for o in models() if o['atlas_group']==group]
    # Return source slots after previous previews/exports.
    for o in obs:
        o.hide_render=False;o.data.materials.clear()
        for name in o['source_materials']:o.data.materials.append(bpy.data.materials[name])
        for f,index in zip(o.data.polygons,o['source_material_indices']):f.material_index=index
    image_name=PREFIX+'_'+group+'_'+channel
    img=bpy.data.images.get(image_name) or bpy.data.images.new(image_name,ATLAS_SIZE,ATLAS_SIZE,alpha=False)
    img.colorspace_settings.name='sRGB' if channel=='BaseColor' else 'Non-Color'
    img.generated_color=(.5,.5,1,1) if channel=='Normal' else (0,0,0,1)
    img.generated_width=img.generated_height=ATLAS_SIZE
    if tuple(img.size)!=(ATLAS_SIZE,ATLAS_SIZE):img.scale(ATLAS_SIZE,ATLAS_SIZE)
    img.filepath_raw=str(OUT/'textures'/f'{group}_{channel}.png');img.file_format='PNG'
    # Cycles may reload FILE-backed images; persist the target size before baking.
    img.save()
    for m in source_mats(group):
        n=m.node_tree.nodes.get('Atlas Bake Target') or node(m,'ShaderNodeTexImage','Atlas Bake Target')
        n.image=img;m.node_tree.nodes.active=n
        for other in m.node_tree.nodes:other.select=False
        n.select=True
        src=m.node_tree.nodes['Original Surface' if channel=='Normal' else ('Color Bake' if channel=='BaseColor' else 'Masks Bake')]
        wire(m,src.outputs[0],m.node_tree.nodes['Output'].inputs['Surface'])
    activate(obs)
    bpy.ops.object.bake(type='NORMAL' if channel=='Normal' else 'EMIT',normal_space='TANGENT',margin=10,use_clear=True)
    img.filepath_raw=str(OUT/'textures'/f'{group}_{channel}.png');img.file_format='PNG';img.save()
    for m in source_mats(group):wire(m,m.node_tree.nodes['Original Surface'].outputs[0],m.node_tree.nodes['Output'].inputs['Surface'])
    sc.render.engine='BLENDER_EEVEE'
    return {'image':img.filepath_raw,'size':list(img.size)}


def delivery_material(group):
    name=PREFIX+'_'+group+'_Baked'
    m=bpy.data.materials.get(name)
    if m:return m
    m=bpy.data.materials.new(name);m.use_nodes=True
    p=m.node_tree.nodes.get('Principled BSDF')
    for channel in ('BaseColor','Normal','Masks'):
        n=node(m,'ShaderNodeTexImage',channel)
        n.image=bpy.data.images[PREFIX+'_'+group+'_'+channel]
        n.extension='EXTEND';n.interpolation='Linear'
        if channel=='BaseColor':wire(m,n.outputs['Color'],p.inputs['Base Color'])
        elif channel=='Normal':
            normal=node(m,'ShaderNodeNormalMap','Tangent normal');wire(m,n.outputs['Color'],normal.inputs['Color']);wire(m,normal.outputs[0],p.inputs['Normal'])
        else:
            sep=node(m,'ShaderNodeSeparateColor','Metal AO Roughness');wire(m,n.outputs['Color'],sep.inputs[0])
            wire(m,sep.outputs[0],p.inputs['Metallic']);wire(m,sep.outputs[2],p.inputs['Roughness'])
    if group=='Bottles':p.inputs['Coat Weight'].default_value=.36;p.inputs['Coat Roughness'].default_value=.21
    return m


def use_baked():
    for o in models():
        o.data.materials.clear();o.data.materials.append(delivery_material(o['atlas_group']))
        for f in o.data.polygons:f.material_index=0


def export_all():
    use_baked();records=[]
    for spec,o in zip(SPECS,models()):
        loc=o.location.copy();rot=o.rotation_euler.copy()
        o.location=(0,0,0);o.rotation_euler=(0,0,0);activate([o])
        bpy.context.view_layer.update()
        path=OUT/'exports'/f'{spec[0]}.fbx'
        bpy.ops.export_scene.fbx(filepath=str(path),use_selection=True,object_types={'MESH'},global_scale=1,apply_unit_scale=True,apply_scale_options='FBX_SCALE_UNITS',axis_forward='-Z',axis_up='Y',bake_space_transform=False,use_mesh_modifiers=True,use_triangles=True,mesh_smooth_type='FACE',add_leaf_bones=False,bake_anim=False,path_mode='RELATIVE',embed_textures=False)
        o.data.calc_loop_triangles()
        records.append({'content_id':spec[0],'display_name':spec[1],'atlas_group':spec[2],
            'dimensions_m':[round(v,6) for v in o.dimensions],'instances':spec[4],'sale_value':spec[5],
            'slots':1,'tier':'common','detector_eligible':False,'required_exposure':.4,
            'vertices':len(o.data.vertices),'triangles':len(o.data.loop_triangles),'materials':len(o.data.materials),
            'fbx':'exports/'+path.name,'textures':{c:f'textures/{spec[2]}_{c}.png' for c in ('BaseColor','Normal','Masks')}})
        o.location=loc;o.rotation_euler=rot
    manifest={'format_version':1,'units':'metres','fbx_up_axis':'+Y','source_up_axis':'+Z','origin':'base centre',
        'categories':len({s[2] for s in SPECS}),'visual_variants':len(SPECS),'total_instances':sum(s[4] for s in SPECS),'total_sale_value':sum(s[4]*s[5] for s in SPECS),'atlas_size':ATLAS_SIZE,
        'masks':'R metallic; G AO (1); B roughness; smoothness = 1-B',
        'glass':'Opaque frosted stylized response, no transparency dependency','variants':records}
    (OUT/'manifest.json').write_text(json.dumps(manifest,indent=2)+'\n',encoding='utf-8')
    for o in models():o.hide_render=False
    import importlib.util
    spec=importlib.util.spec_from_file_location("starter_colliders",str(OUT/"create_bottle_colliders.py"))
    colliders=importlib.util.module_from_spec(spec);spec.loader.exec_module(colliders);colliders.build()
    save_source()
    return {'exports':len(records),'total_triangles':sum(r['triangles'] for r in records),'manifest':str(OUT/'manifest.json')}
