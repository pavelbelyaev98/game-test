"""Sunny r5: sparse weathered mineral fragments, original Blender MCP authoring.

Keeps the approved r4 soil matrix and r3 turf. Stone masks share the existing
roughness texture: R roughness, G contact, B embedded-stone coverage.
"""
import bpy
import math
import random
import types
from pathlib import Path
from mathutils import Vector, Euler

SOURCE = Path(r'C:/Users/pavel/Desktop/Dev/CompanyProjects/game-test/art/ground-textures')
VARIANT = 'A-Sunny-r5'


def bind():
    global v,d,c,g,scene
    v=types.ModuleType('varied_soil')
    exec(compile((SOURCE/'varied_soil.py').read_text(),'varied_soil.py','exec'),v.__dict__)
    v.bind();d,c,g,scene=v.d,v.c,v.g,v.scene
    d.VARIANT=VARIANT


def mineral(index):
    m=v.material(index)
    m.name=VARIANT+' weathered mineral '+str(index)
    p=m.node_tree.nodes['Surface'];coords=g.periodic_coords(m)
    fine=g.noise(m,coords,55,2,'Fine mineral pits')
    strata=g.noise(m,coords,12,2,'Broken mineral layering')
    bump=g.node(m,'ShaderNodeBump','Worn granular stone face')
    bump.inputs['Distance'].default_value=.0011
    bump.inputs['Strength'].default_value=.35
    g.wire(m,g.calc(m,'ADD',g.calc(m,'MULTIPLY',fine,.6),g.calc(m,'MULTIPLY',strata,.4)),bump.inputs['Height'])
    d.assign(m,bump.outputs['Normal'],p.inputs['Normal'])
    rough=g.ramp(m,fine,[(.25,(.69,.69,.69)),(.75,(.84,.84,.84))],'Dry mineral roughness')
    d.assign(m,rough,p.inputs['Roughness'])
    # Fine mineral variation is visible in color without baked directional light.
    original=p.inputs['Base Color'].links[0].from_socket
    variation=g.ramp(m,strata,[(.28,(.72,.76,.79)),(.7,(1.16,1.13,1.08))],'Subtle mineral bands')
    mix=g.node(m,'ShaderNodeMixRGB','Mineral color breakup');mix.blend_type='MULTIPLY'
    mix.inputs[0].default_value=.48
    g.wire(m,original,mix.inputs[1]);g.wire(m,variation,mix.inputs[2])
    d.assign(m,mix.outputs[0],p.inputs['Base Color'])
    return m


def initialize():
    bind()
    if scene.objects.get(VARIANT+' Soil source'):raise RuntimeError('Retain existing r5; bind and bake it.')
    # Reuse the exact accepted dirt nodes, independently so bake packing cannot
    # modify the retained r4 trial. Only the embedded fragments are revised.
    matrix=scene.objects['A-Sunny-r4 Soil source'].data.materials[0].copy()
    matrix.name=VARIANT+' retained soil matrix'
    c.plane(VARIANT+' Soil source',0,matrix)
    receiver=bpy.data.materials.new(VARIANT+' Soil receiver');receiver.use_nodes=True
    c.plane(VARIANT+' Soil target',-.025,receiver)
    bpy.context.window.scene=scene
    bpy.ops.object.select_all(action='DESELECT')
    bpy.ops.mesh.primitive_ico_sphere_add(subdivisions=1,radius=1)
    template=bpy.context.object;mesh=template.data
    base=[v.co.copy() for v in mesh.vertices];triangles=[tuple(f.vertices) for f in mesh.polygons]
    bpy.data.objects.remove(template,do_unlink=True);bpy.data.meshes.remove(mesh)
    rng=random.Random(105905);vertices,faces,slots,placed=[],[],[],[]
    # 30 readable stones instead of 93; chips reduced from 350 to 100.
    for count,lo,hi in ((6,.045,.085),(24,.012,.038),(100,.0018,.008)):
        for _ in range(count):
            radius=rng.uniform(lo,hi);x,y=rng.random()*2,rng.random()*2
            if lo>.01:
                for attempt in range(80):
                    if all(min(abs(x-a),2-abs(x-a))**2+min(abs(y-b),2-abs(y-b))**2>(radius+s+.07)**2 for a,b,s in placed):break
                    x,y=rng.random()*2,rng.random()*2
                placed.append((x,y,radius))
            scale=Vector((radius*rng.uniform(.85,1.4),radius*rng.uniform(.65,1.05),radius*rng.uniform(.27,.4)))
            rot=Euler((rng.uniform(-.1,.1),rng.uniform(-.1,.1),rng.random()*math.tau)).to_matrix()
            burial=scale.z*rng.uniform(.28,.56)
            shear=rng.uniform(-.3,.3);points=[]
            for a in base:
                # Coherent shear and broad fractured planes avoid noisy crowns.
                b=rot@Vector(((a.x+a.z*shear)*scale.x,a.y*scale.y,a.z*scale.z))
                points.append((x+b.x,y+b.y,b.z-burial))
            oxs=[0]+([2] if min(p[0] for p in points)<.022 else [])+([-2] if max(p[0] for p in points)>1.978 else [])
            oys=[0]+([2] if min(p[1] for p in points)<.022 else [])+([-2] if max(p[1] for p in points)>1.978 else [])
            slot=rng.randrange(8)
            for ox in oxs:
                for oy in oys:
                    start=len(vertices);vertices.extend((a+ox,b+oy,z) for a,b,z in points)
                    faces.extend(tuple(start+i for i in f) for f in triangles);slots.extend([slot]*len(triangles))
    mesh=bpy.data.meshes.new(VARIANT+' sparse fractured stone source')
    mesh.from_pydata(vertices,[],faces);mesh.update()
    uv=mesh.uv_layers.new(name='Periodic mineral detail')
    for face in mesh.polygons:
        for li in face.loop_indices:
            p=mesh.vertices[mesh.loops[li].vertex_index].co;uv.data[li].uv=(p.x/2,p.y/2)
    for i in range(8):mesh.materials.append(mineral(i))
    for face,slot in zip(mesh.polygons,slots):face.material_index=slot
    soften_faces(mesh)
    obj=bpy.data.objects.new(VARIANT+' Soil relief',mesh);scene.collection.objects.link(obj)
    # A retained Blender modifier rounds only fractured edges. Broad faces keep
    # their planes; the game receives their baked normals, not additional meshes.
    bevel=obj.modifiers.new('Weathered fracture edges','BEVEL')
    bevel.width=.0015;bevel.segments=3;bevel.limit_method='ANGLE';bevel.angle_limit=.3
    scene.cycles.samples=24
    return {'variant':VARIANT,'readableStones':30,'chips':100,'previousReadableStones':93,'previousChips':350}


def soften_faces(mesh):
    helper=types.ModuleType('rocky_soil')
    exec(compile((SOURCE/'rocky_soil.py').read_text(),'rocky_soil.py','exec'),helper.__dict__)
    helper.weather_stone_normals(mesh)


def bake(channel):
    bind();d.bind=lambda:None;d.SIZE=1024
    if channel!='Roughness':return d.bake('Soil',channel)
    # Preserve the existing bake implementation and add the stone coverage to B.
    # Its packer sets B=1 by default; a linked value is deliberately retained.
    for suffix,value in ((' source',0),(' relief',1)):
        for m in scene.objects[VARIANT+' Soil'+suffix].data.materials:
            mask=m.node_tree.nodes.get('Packed surface mask') or g.node(m,'ShaderNodeCombineColor','Packed surface mask')
            identity=g.node(m,'ShaderNodeValue','Embedded mineral coverage');identity.outputs[0].default_value=value
            d.assign(m,identity.outputs[0],mask.inputs[2])
    return d.bake('Soil',channel)


def save_source():
    bind();return c.save_source()
