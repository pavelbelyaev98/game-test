import bpy, bmesh, math, random, json, sys
from pathlib import Path
from mathutils import Vector

OUT = Path(__file__).resolve().parent
OUT.mkdir(parents=True, exist_ok=True)
scene = bpy.data.scenes.new('SDT_Minerals')
bpy.context.window.scene = scene
scene.unit_settings.system = 'METRIC'
scene.render.engine = 'CYCLES'
scene.cycles.samples = 24
scene.cycles.bake_type = 'EMIT'
scene.render.bake.margin = 8
scene.view_settings.view_transform = 'AgX'
rng = random.Random(145)
items = []

def material(name, color, metallic, rough):
    m=bpy.data.materials.new('SDT_Mineral_'+name); m.use_nodes=True
    n=m.node_tree.nodes; l=m.node_tree.links; p=n.get('Principled BSDF')
    p.inputs['Metallic'].default_value=metallic; p.inputs['Roughness'].default_value=rough
    noise=n.new('ShaderNodeTexNoise'); noise.inputs['Scale'].default_value=38; noise.inputs['Detail'].default_value=3
    ramp=n.new('ShaderNodeValToRGB')
    ramp.color_ramp.elements[0].position=.18; ramp.color_ramp.elements[0].color=tuple(c*.45 for c in color)+(1,)
    ramp.color_ramp.elements[1].position=.82; ramp.color_ramp.elements[1].color=tuple(min(1,c*1.25+.015) for c in color)+(1,)
    l.new(noise.outputs['Fac'],ramp.inputs[0]); l.new(ramp.outputs[0],p.inputs['Base Color'])
    bump=n.new('ShaderNodeBump'); bump.inputs['Strength'].default_value=.22; bump.inputs['Distance'].default_value=.008
    l.new(noise.outputs['Fac'],bump.inputs['Height']); l.new(bump.outputs[0],p.inputs['Normal'])
    return m

matrix=material('Matrix',(.13,.11,.085),.05,.88)
colors=[(.035,.045,.054),(.65,.20,.055),(.26,.095,.05),(.62,.69,.76),(.95,.50,.035),(.015,.62,.16),(.75,.016,.055),(.48,.82,.98)]
names=['Coal','Copper','Iron','Silver','Gold','Emerald','Ruby','Diamond']
dims=[(.60,.44,.40),(.59,.46,.41),(.60,.45,.40),(.57,.45,.40),(.54,.43,.39),(.45,.40,.60),(.48,.43,.55),(.46,.44,.55)]

def ico(name,location,scale,mat,sub=1):
    bpy.ops.mesh.primitive_ico_sphere_add(subdivisions=sub,radius=1,location=location)
    o=bpy.context.object; o.name=name
    for v in o.data.vertices: v.co *= rng.uniform(.84,1.16)
    o.scale=scale; o.data.materials.append(mat)
    bevel=o.modifiers.new('Worn edges','BEVEL'); bevel.width=.012; bevel.segments=2
    bpy.ops.object.modifier_apply(modifier=bevel.name)
    return o

def crystal(name,location,radius,length,sides,mat,diamond=False):
    verts=[]
    rings=[(0,.0),(.25,1),(.60,1),(1,.12)] if diamond else [(0,.65),(.14,1),(.78,1),(1,.57)]
    for z,r in rings:
        for i in range(sides):
            a=i*2*math.pi/sides; verts.append((radius*r*math.cos(a),radius*r*math.sin(a),length*z))
    faces=[tuple(reversed(range(sides)))]
    for k in range(3):
        for j in range(sides): faces.append((k*sides+j,k*sides+(j+1)%sides,(k+1)*sides+(j+1)%sides,(k+1)*sides+j))
    faces.append(tuple(range(3*sides,4*sides)))
    mesh=bpy.data.meshes.new(name); mesh.from_pydata(verts,[],faces); mesh.update()
    o=bpy.data.objects.new(name,mesh); scene.collection.objects.link(o); o.location=location
    o.rotation_euler=(rng.uniform(-.28,.28),rng.uniform(-.32,.32),rng.random()*6.28); o.data.materials.append(mat)
    return o

for idx,name in enumerate(names):
    mineral=material(name,colors[idx],.75 if idx in (1,3,4) else .25 if idx==0 else .12,.28 if idx<5 else .19)
    parts=[]
    if idx==0:
        for j in range(5): parts.append(ico(name,(rng.uniform(-.11,.11),rng.uniform(-.08,.08),rng.uniform(.05,.17)),(.21,.14,.12),mineral,1))
    elif idx<5:
        parts.append(ico(name,(0,0,.13),(.27,.20,.16),matrix,2))
        for j in range(15):
            a=j*2.4; r=rng.uniform(.08,.20)
            parts.append(ico(name,(math.cos(a)*r,math.sin(a)*r*.7,rng.uniform(.15,.28)),(.065,.043,.035),mineral,1))
    else:
        parts.append(ico(name,(0,0,.045),(.20,.17,.085),matrix,1))
        for j in range(4):
            parts.append(crystal(name,(rng.uniform(-.09,.09),rng.uniform(-.08,.08),.02),.09 if j else .125,.28 if j else .44,8 if idx==7 else 6,mineral,idx==7))
    bpy.ops.object.select_all(action='DESELECT')
    for p in parts:p.select_set(True)
    bpy.context.view_layer.objects.active=parts[0]; bpy.ops.object.join(); o=bpy.context.object; o.name='SDT_Mineral_'+name
    bpy.ops.object.transform_apply(location=False,rotation=True,scale=True)
    lo=Vector((min(v.co.x for v in o.data.vertices),min(v.co.y for v in o.data.vertices),min(v.co.z for v in o.data.vertices)))
    hi=Vector((max(v.co.x for v in o.data.vertices),max(v.co.y for v in o.data.vertices),max(v.co.z for v in o.data.vertices)))
    for v in o.data.vertices:
        for a in range(3): v.co[a]=(v.co[a]-(lo[a]+hi[a])*.5)*dims[idx][a]/(hi[a]-lo[a])
    o.location=(0,0,0); bpy.ops.object.mode_set(mode='EDIT'); bpy.ops.mesh.select_all(action='SELECT'); bpy.ops.uv.smart_project(island_margin=.02); bpy.ops.object.mode_set(mode='OBJECT')
    folder=OUT/name.lower(); folder.mkdir(exist_ok=True)
    mats=list(o.data.materials)
    for kind in ('BaseColor','Normal','Masks'):
        img=bpy.data.images.new(name+'_'+kind,512,512,alpha=True)
        if kind!='BaseColor':img.colorspace_settings.name='Non-Color'
        links=[]
        for m in mats:
            nt=m.node_tree; p=nt.nodes.get('Principled BSDF'); out=nt.nodes.get('Material Output')
            tex=nt.nodes.new('ShaderNodeTexImage'); tex.image=img; nt.nodes.active=tex
            if kind!='Normal':
                emit=nt.nodes.new('ShaderNodeEmission')
                if kind=='BaseColor':nt.links.new(p.inputs['Base Color'].links[0].from_socket,emit.inputs['Color'])
                else:emit.inputs['Color'].default_value=(p.inputs['Metallic'].default_value,1,0,1)
                nt.links.new(emit.outputs[0],out.inputs['Surface']);links.append((nt,p,out,emit))
        bpy.ops.object.bake(type='NORMAL' if kind=='Normal' else 'EMIT')
        if kind=='Masks':
            # Alpha is authored smoothness. Derive by material region's metallic mask;
            # ore/crystals retain highlights while the surrounding host remains rough.
            pixels=list(img.pixels[:])
            for k in range(0,len(pixels),4): pixels[k+3]=.68 if pixels[k]>.10 else .12
            img.pixels[:]=pixels
        img.filepath_raw=str(folder/(kind+'.png')); img.file_format='PNG'; img.save(); img.pack()
        for nt,p,out,emit in links: nt.links.new(p.outputs[0],out.inputs['Surface']);nt.nodes.remove(emit)
    atlas=bpy.data.materials.new('SDT_Mineral_'+name+'_Atlas');atlas.use_nodes=True
    nt=atlas.node_tree; bs=nt.nodes.get('Principled BSDF')
    for kind,socket in [('BaseColor','Base Color'),('Masks','Metallic')]:
        tex=nt.nodes.new('ShaderNodeTexImage');tex.image=bpy.data.images[name+'_'+kind];nt.links.new(tex.outputs['Color'],bs.inputs[socket])
        if kind=='Masks':
            inv=nt.nodes.new('ShaderNodeMath');inv.operation='SUBTRACT';inv.inputs[0].default_value=1;nt.links.new(tex.outputs['Alpha'],inv.inputs[1]);nt.links.new(inv.outputs[0],bs.inputs['Roughness'])
    tex=nt.nodes.new('ShaderNodeTexImage');tex.image=bpy.data.images[name+'_Normal'];normal=nt.nodes.new('ShaderNodeNormalMap');nt.links.new(tex.outputs[0],normal.inputs['Color']);nt.links.new(normal.outputs[0],bs.inputs['Normal'])
    o.data.materials.clear();o.data.materials.append(atlas)
    for p in o.data.polygons:p.material_index=0
    bpy.ops.export_scene.fbx(filepath=str(folder/(name+'.fbx')),use_selection=True,object_types={'MESH'},add_leaf_bones=False,bake_anim=False,axis_forward='-Z',axis_up='Y')
    mesh=bpy.data.meshes.new(name+'_Hull');bm=bmesh.new()
    directions=[Vector(v) for v in ((1,0,0),(-1,0,0),(0,1,0),(0,-1,0),(0,0,1),(0,0,-1))]
    for i in range(72):
        z=1-2*(i+.5)/72; a=i*2.399963; r=math.sqrt(1-z*z); directions.append(Vector((r*math.cos(a),r*math.sin(a),z)))
    support={max(range(len(o.data.vertices)),key=lambda i:o.data.vertices[i].co.dot(d)) for d in directions}
    for i in support:bm.verts.new(o.data.vertices[i].co)
    bmesh.ops.convex_hull(bm,input=list(bm.verts));bm.to_mesh(mesh);bm.free()
    hull=bpy.data.objects.new(name+'_Hull',mesh);scene.collection.objects.link(hull)
    bpy.ops.object.select_all(action='DESELECT');hull.select_set(True);bpy.context.view_layer.objects.active=hull
    bpy.ops.export_scene.fbx(filepath=str(folder/(name+'_collision.fbx')),use_selection=True,object_types={'MESH'},add_leaf_bones=False,bake_anim=False,axis_forward='-Z',axis_up='Y')
    hull.hide_render=True;hull.hide_set(True)
    o.location=((idx%4)*1.4-2.1,(idx//4)*-1.5,.34)
    items.append({'name':name,'dimensions_m':dims[idx],'vertices':len(o.data.vertices),'triangles':sum(len(p.vertices)-2 for p in o.data.polygons)})
    bpy.ops.object.text_add(location=(o.location.x-.38,o.location.y-.49,.025))
    label=bpy.context.object;label.name='SDT_Label_'+name;label.data.body=name;label.data.size=.16

# Preview scene stays separate from the user's default scene and outside Unity Assets.
scene.world=bpy.data.worlds.new('SDT_MineralWorld');scene.world.use_nodes=True;scene.world.node_tree.nodes['Background'].inputs[0].default_value=(.13,.17,.21,1)
scene.world.node_tree.nodes['Background'].inputs[1].default_value=.6
bpy.ops.mesh.primitive_plane_add(size=200,location=(0,0,-.04));plane=bpy.context.object;plane.name='SDT_PreviewBackdrop';plane.data.materials.append(material('Backdrop',(.035,.045,.06),0,.9))
for loc,power,size in [((-3,-4,6),1000,5),((4,0,4),800,4)]:
    bpy.ops.object.light_add(type='AREA',location=loc);light=bpy.context.object;light.data.energy=power;light.data.shape='DISK';light.data.size=size;light.rotation_euler=(Vector((0,-.7,0))-light.location).to_track_quat('-Z','Y').to_euler()
bpy.ops.object.camera_add(location=(0,-6.7,7.5));camera=bpy.context.object;camera.rotation_euler=(Vector((0,-.7,.1))-camera.location).to_track_quat('-Z','Y').to_euler();camera.data.type='ORTHO';camera.data.ortho_scale=6.2;scene.camera=camera
scene.render.resolution_x=1600;scene.render.resolution_y=1000;scene.render.resolution_percentage=100
scene.render.filepath=str(OUT/'preview.png')
bpy.data.libraries.write(str(OUT/'Minerals.blend'),{scene}|{m for m in bpy.data.materials if m.name.startswith('SDT_Mineral_')},fake_user=True)
(OUT/'manifest.json').write_text(json.dumps(items,indent=2))
if "--preview" in sys.argv: bpy.ops.render.render(write_still=True)
result={'preview':str(OUT/'preview.png'),'source':str(OUT/'Minerals.blend'),'items':items}
