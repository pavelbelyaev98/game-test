"""Original rocky soil, authored and baked in Blender via the existing source.

The two-metre tile contains buried, distorted stone geometry in a fine soil
matrix. Only colour, tangent normal and packed roughness/occlusion are exported.
Use bind_source(), create_revision(), then bake(channel) and save_source().
"""
import bpy
import math
import random
import types
import sys
from pathlib import Path
from mathutils import Vector, Euler

ROOT = Path(r'C:/Users/pavel/Desktop/Dev/CompanyProjects/game-test')
SOURCE = ROOT/'art/ground-textures'
DEST = ROOT/'unity/Assets/Content/GroundTextures'
TILE = 2.0
SIZE = 2048


def bind_source():
    global g, scene, target, bed, rocks
    g = types.ModuleType('ground_authoring')
    sys.modules[g.__name__] = g
    exec(compile((SOURCE/'create_ground.py').read_text(encoding='utf-8'), 'create_ground.py', 'exec'), g.__dict__)
    g.bind_existing()
    scene = g.scene
    target = scene.objects.get('Rocky soil bake tile - 2 metres')
    bed = scene.objects.get('Rocky soil matrix source')
    rocks = scene.objects.get('Embedded stone source - wrapped')


def matrix_material():
    m = bpy.data.materials.new('Rocky soil - warm granular matrix')
    m.use_nodes = True
    m.node_tree.nodes.clear()
    c = g.periodic_coords(m)
    broad = g.noise(m, c, 0.9, 3, 'Slow soil tone variation')
    crumb = g.noise(m, c, 11, 4, 'Fine compacted earth crumbs')
    sand = g.noise(m, c, 140, 2, 'Sand grains')
    pore = g.noise(m, c, 60, 2, 'Sparse soil pores')
    v = g.calc(m, 'ADD', g.calc(m, 'MULTIPLY', broad, 0.36), g.calc(m, 'MULTIPLY', crumb, 0.64))
    colour = g.ramp(m, v, [(0.22,(0.09,0.041,0.017)), (0.50,(0.225,0.117,0.05)), (0.78,(0.34,0.202,0.097))], 'Dry warm brown earth')
    grain_colour = g.ramp(m, sand, [(0.29,(0.45,0.35,0.25)),(0.55,(0.98,0.90,0.79)),(0.72,(1.5,1.32,1.08))], 'Fine pale mineral grit')
    multiply = g.node(m, 'ShaderNodeMixRGB', 'Grain integrated into soil colour')
    multiply.blend_type = 'MULTIPLY'
    multiply.inputs[0].default_value = 0.8
    g.wire(m, colour, multiply.inputs[1])
    g.wire(m, grain_colour, multiply.inputs[2])
    dark = g.calc(m,'LESS_THAN',pore,0.31)
    pores = g.node(m,'ShaderNodeMixRGB','Small irregular soil pores')
    g.wire(m,g.calc(m,'MULTIPLY',dark,0.28),pores.inputs[0])
    g.wire(m,multiply.outputs[0],pores.inputs[1])
    pores.inputs[2].default_value=(0.04,0.025,0.012,1)
    height = g.calc(m,'ADD',g.calc(m,'MULTIPLY',crumb,0.6),g.calc(m,'MULTIPLY',sand,0.32))
    height = g.calc(m,'SUBTRACT',height,g.calc(m,'MULTIPLY',dark,0.1))
    bump = g.node(m,'ShaderNodeBump','Fine granular relief')
    bump.inputs['Distance'].default_value=0.0035
    bump.inputs['Strength'].default_value=0.45
    g.wire(m,height,bump.inputs['Height'])
    p = g.node(m,'ShaderNodeBsdfPrincipled','Surface')
    g.wire(m,pores.outputs[0],p.inputs['Base Color'])
    g.wire(m,bump.outputs['Normal'],p.inputs['Normal'])
    g.wire(m,g.calc(m,'ADD',0.8,g.calc(m,'MULTIPLY',sand,0.16)),p.inputs['Roughness'])
    output = g.node(m,'ShaderNodeOutputMaterial','Output')
    g.wire(m,p.outputs[0],output.inputs[0])
    g.node(m,'ShaderNodeEmission','Bake emission')
    return m


def stone_material(index, base):
    m = bpy.data.materials.new('Embedded stone %02d' % index)
    m.use_nodes=True
    m.node_tree.nodes.clear()
    # Four-dimensional periodic coordinates use a wrapping UV layer on the
    # sculpted stones, so copies at opposite tile edges have identical detail.
    coords = g.periodic_coords(m)
    fleck = g.noise(m,coords,82,3,'Mineral mottling')
    dust = g.noise(m,coords,8,2,'Uneven soil coating')
    scale = g.calc(m,'ADD',0.55,g.calc(m,'MULTIPLY',fleck,0.9))
    colour = g.node(m,'ShaderNodeMixRGB','Stone mineral tone')
    colour.blend_type='MULTIPLY'
    colour.inputs[0].default_value=1
    colour.inputs[1].default_value=(*base,1)
    g.wire(m,scale,colour.inputs[2])
    geometry = g.node(m,'ShaderNodeNewGeometry','Surface geometry')
    separate = g.node(m,'ShaderNodeSeparateXYZ','Height above matrix')
    g.wire(m,geometry.outputs['Position'],separate.inputs[0])
    exposed = g.calc(m,'MINIMUM',g.calc(m,'MAXIMUM',g.calc(m,'MULTIPLY',separate.outputs['Z'],330),0),1)
    coating = g.calc(m,'MINIMUM',0.94,g.calc(m,'ADD',g.calc(m,'MULTIPLY',g.calc(m,'SUBTRACT',1,exposed),0.38),g.calc(m,'ADD',0.38,g.calc(m,'MULTIPLY',dust,0.65))))
    coated = g.node(m,'ShaderNodeMixRGB','Embedded edge dust')
    g.wire(m,coating,coated.inputs[0])
    g.wire(m,colour.outputs[0],coated.inputs[1])
    dirt = g.ramp(m, fleck, [(0.2,(0.12,0.058,0.024)), (0.5,(0.205,0.105,0.044)), (0.8,(0.29,0.163,0.074))], 'Adhering granular dirt')
    g.wire(m,dirt,coated.inputs[2])
    p = g.node(m,'ShaderNodeBsdfPrincipled','Surface')
    g.wire(m,coated.outputs[0],p.inputs['Base Color'])
    bump=g.node(m,'ShaderNodeBump','Small rock pits')
    bump.inputs['Distance'].default_value=0.0018
    bump.inputs['Strength'].default_value=0.5
    g.wire(m,fleck,bump.inputs['Height'])
    g.wire(m,bump.outputs['Normal'],p.inputs['Normal'])
    g.wire(m,g.calc(m,'ADD',0.82,g.calc(m,'MULTIPLY',fleck,0.15)),p.inputs['Roughness'])
    output=g.node(m,'ShaderNodeOutputMaterial','Output')
    g.wire(m,p.outputs[0],output.inputs[0])
    g.node(m,'ShaderNodeEmission','Bake emission')
    return m


def make_plane(name, extent, z, mat):
    bpy.ops.mesh.primitive_plane_add(size=extent, location=(TILE/2,TILE/2,z))
    obj=bpy.context.object
    obj.name=name
    obj.data.materials.append(mat)
    for loop in obj.data.uv_layers.active.data:
        loop.uv=((loop.uv.x-.5)*extent/TILE+.5,(loop.uv.y-.5)*extent/TILE+.5)
    return obj


def create_revision():
    global target, bed, rocks
    if target is not None:
        raise RuntimeError('Revision source already exists; bind and rebake it.')
    bpy.context.window.scene=scene
    matrix=matrix_material()
    receiver=bpy.data.materials.new('Rocky soil bake receiver')
    receiver.use_nodes=True
    target=make_plane('Rocky soil bake tile - 2 metres',TILE,-0.025,receiver)
    bed=make_plane('Rocky soil matrix source',TILE+0.3,0,matrix)
    palettes=[(0.11,0.08,0.052),(0.16,0.124,0.085),(0.125,0.112,0.085),
              (0.20,0.139,0.082),(0.085,0.066,0.045),(0.25,0.216,0.158)]
    mats=[stone_material(i,tuple(v*(0.8+(i//6)*0.19) for v in palettes[i%6])) for i in range(18)]
    bpy.ops.mesh.primitive_ico_sphere_add(subdivisions=2,radius=1)
    template=bpy.context.object
    template_mesh=template.data
    base_vertices=[v.co.copy() for v in template_mesh.vertices]
    base_faces=[tuple(p.vertices) for p in template_mesh.polygons]
    bpy.data.objects.remove(template,do_unlink=True)
    bpy.data.meshes.remove(template_mesh)
    rng=random.Random(771903)
    vertices, faces, slots=[],[],[]
    placed=[]
    # Embedded stones, broken gravel, then fine chips. A broad size distribution
    # and sparse large stones avoid the uniform polka-dot appearance.
    for count,minimum,maximum in ((28,.023,.061),(65,.004,.012),(1600,.0009,.0035)):
        for i in range(count):
            radius=rng.uniform(minimum,maximum)
            x,y=rng.uniform(0,TILE),rng.uniform(0,TILE)
            if minimum>.01:
                for attempt in range(60):
                    if all(min(abs(x-px),TILE-abs(x-px))**2+min(abs(y-py),TILE-abs(y-py))**2>(radius+pr+.025)**2 for px,py,pr in placed):
                        break
                    x,y=rng.uniform(0,TILE),rng.uniform(0,TILE)
                placed.append((x,y,radius))
            shape=Vector((radius*rng.uniform(.72,1.35),radius*rng.uniform(.65,1.05),radius*rng.uniform(.30,.55)))
            rotation=Euler((rng.uniform(-.20,.20),rng.uniform(-.20,.20),rng.random()*math.tau)).to_matrix()
            bury=shape.z*rng.uniform(.65,.86)
            sculpt=[]
            for v in base_vertices:
                wobble=rng.uniform(.82,1.18)
                a=rotation@Vector((v.x*shape.x*wobble,v.y*shape.y*wobble,v.z*shape.z*rng.uniform(.88,1.14)))
                # A sheared, asymmetrical crown gives chipped silhouettes and
                # different facets without a circular pebble mask.
                a.x+=a.z*rng.uniform(-.18,.18)
                sculpt.append((x+a.x,y+a.y,a.z-bury))
            # Include the AO radius outside the tile, not just crossing faces.
            oxs=[0]+([TILE] if min(v[0] for v in sculpt)<.022 else [])+([-TILE] if max(v[0] for v in sculpt)>TILE-.022 else [])
            oys=[0]+([TILE] if min(v[1] for v in sculpt)<.022 else [])+([-TILE] if max(v[1] for v in sculpt)>TILE-.022 else [])
            material=rng.randrange(len(mats))
            for ox in oxs:
                for oy in oys:
                    offset=len(vertices)
                    vertices.extend((vx+ox,vy+oy,vz) for vx,vy,vz in sculpt)
                    faces.extend(tuple(offset+v for v in f) for f in base_faces)
                    slots.extend([material]*len(base_faces))
    mesh=bpy.data.meshes.new('Distorted buried stone geometry')
    mesh.from_pydata(vertices,[],faces)
    mesh.update()
    uv=mesh.uv_layers.new(name='Periodic planar detail')
    for p in mesh.polygons:
        p.use_smooth=False
        for li in p.loop_indices:
            v=mesh.vertices[mesh.loops[li].vertex_index].co
            uv.data[li].uv=(v.x/TILE,v.y/TILE)
    for m in mats: mesh.materials.append(m)
    for p,slot in zip(mesh.polygons,slots): p.material_index=slot
    rocks=bpy.data.objects.new('Embedded stone source - wrapped',mesh)
    scene.collection.objects.link(rocks)
    weather_stone_normals(mesh)
    scene.cycles.samples=16
    return {'stones':1693,'vertices':len(vertices),'triangles':len(faces),'tileMetres':TILE}


def weather_stone_normals(mesh):
    # Blend broad worn surfaces with the fractured faces. Silhouettes stay
    # chipped, while the normal bake avoids shiny triangular rock-candy facets.
    vertex_normals=[Vector((0,0,0)) for _ in mesh.vertices]
    for face in mesh.polygons:
        weighted=face.normal*face.area
        for index in face.vertices: vertex_normals[index]+=weighted
    vertex_normals=[n.normalized() for n in vertex_normals]
    normals=[(0,0,1)]*len(mesh.loops)
    for face in mesh.polygons:
        face.use_smooth=True
        for li in face.loop_indices:
            n=vertex_normals[mesh.loops[li].vertex_index]*.65+face.normal*.35
            normals[li]=tuple(n.normalized())
    mesh.normals_split_custom_set(normals)
    mesh.update()


def rebuild():
    global target,bed,rocks
    previous=(target,bed,rocks)
    if not all(previous): raise RuntimeError('Bind the existing revision first.')
    for obj in previous: obj.name='Prior '+obj.name
    target=bed=rocks=None
    result=create_revision()
    for old in previous[0].data.materials[0].node_tree.nodes:
        if old.type=='TEX_IMAGE' and old.image:
            n=g.node(target.data.materials[0],'ShaderNodeTexImage',old.name)
            n.image=old.image
    for obj in previous:
        mesh=obj.data
        materials=list(mesh.materials)
        bpy.data.objects.remove(obj,do_unlink=True)
        if mesh.users==0: bpy.data.meshes.remove(mesh)
        for material in materials:
            if material.users==0: bpy.data.materials.remove(material)
    return result


def bake(channel):
    bpy.context.window.scene=scene
    mats=[bed.data.materials[0],*rocks.data.materials]
    for m in mats:
        nodes=m.node_tree.nodes
        p,out,em=nodes['Surface'],nodes['Output'],nodes['Bake emission']
        if channel=='Normal':
            g.wire(m,p.outputs[0],out.inputs[0])
        elif channel=='Albedo':
            g.wire(m,p.inputs['Base Color'].links[0].from_socket,em.inputs['Color'])
            g.wire(m,em.outputs[0],out.inputs[0])
        else:
            ao=nodes.get('Stone contact occlusion') or g.node(m,'ShaderNodeAmbientOcclusion','Stone contact occlusion')
            ao.inputs['Distance'].default_value=.018
            ao.samples=16
            packed=nodes.get('R roughness G occlusion') or g.node(m,'ShaderNodeCombineColor','R roughness G occlusion')
            g.wire(m,p.inputs['Roughness'].links[0].from_socket,packed.inputs[0])
            g.wire(m,ao.outputs['AO'],packed.inputs[1])
            packed.inputs[2].default_value=1
            g.wire(m,packed.outputs[0],em.inputs['Color'])
            g.wire(m,em.outputs[0],out.inputs[0])
    bpy.ops.object.select_all(action='DESELECT')
    for obj in scene.objects: obj.hide_render=True
    for obj in (target,bed,rocks):
        obj.hide_render=False
        obj.select_set(True)
    bpy.context.view_layer.objects.active=target
    mat=target.data.materials[0]
    texture=mat.node_tree.nodes.get('Soil_'+channel) or g.node(mat,'ShaderNodeTexImage','Soil_'+channel)
    im=texture.image or bpy.data.images.new('RockySoil_'+channel,SIZE,SIZE,alpha=False)
    im.colorspace_settings.name='sRGB' if channel=='Albedo' else 'Non-Color'
    texture.image=im
    mat.node_tree.nodes.active=texture
    bpy.ops.object.bake(type='NORMAL' if channel=='Normal' else 'EMIT',use_selected_to_active=True,cage_extrusion=.13,max_ray_distance=.2,margin=0)
    im.filepath_raw=str(DEST/('Soil_'+channel+'.png'))
    im.file_format='PNG'
    im.save()
    for m in mats: g.wire(m,m.node_tree.nodes['Surface'].outputs[0],m.node_tree.nodes['Output'].inputs[0])
    return {'file':im.filepath_raw,'size':SIZE}


def save_source():
    for n in target.data.materials[0].node_tree.nodes:
        if n.type=='TEX_IMAGE' and n.image: n.image.pack()
    recipe=bpy.data.texts.get('rocky_soil.py') or bpy.data.texts.new('rocky_soil.py')
    recipe.clear()
    recipe.write((SOURCE/'rocky_soil.py').read_text(encoding='utf-8'))
    # The shared saver lays out all node graphs and embeds both recipes.
    return g.save_source()
