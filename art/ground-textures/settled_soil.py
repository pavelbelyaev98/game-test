"""Sunny r7: a modestly sparser, soil-coated revision of the approved stones.

Execute through Blender MCP. Keep the accepted matrix and retained r6 trial;
remove 20 of 130 periodic stone identities without reshuffling the remainder.
"""
import bpy
import bmesh
import random
import types
from pathlib import Path
from mathutils import Vector

SOURCE = Path(r'C:/Users/pavel/Desktop/Dev/CompanyProjects/game-test/art/ground-textures')
VARIANT = 'A-Sunny-r7'


def bind():
    global r6,d,c,g,scene
    r6=types.ModuleType('crisp_ground')
    exec(compile((SOURCE/'crisp_ground.py').read_text(),'crisp_ground.py','exec'),r6.__dict__)
    r6.bind();d,c,g,scene=r6.d,r6.c,r6.g,r6.scene
    d.VARIANT=VARIANT;d.SIZE=2048;d.bind=lambda:None


def initialize():
    bind()
    if scene.objects.get(VARIANT+' Soil source'):
        raise RuntimeError('Retain existing r7; bind and bake it.')
    for suffix in (' source',' relief'):
        src=scene.objects['A-Sunny-r6 Soil'+suffix]
        obj=src.copy();obj.data=src.data.copy();obj.name=VARIANT+' Soil'+suffix
        scene.collection.objects.link(obj)
        for slot in obj.material_slots:
            material=slot.material.copy();material.name=material.name.replace('A-Sunny-r6',VARIANT)
            slot.material=material
    obj=scene.objects[VARIANT+' Soil relief'];mesh=obj.data
    assert len(mesh.vertices)%12==0
    identities=[];blocks=[]
    for start in range(0,len(mesh.vertices),12):
        center=sum((v.co for v in mesh.vertices[start:start+12]),Vector())/12
        key=(round(center.x%2,5),round(center.y%2,5))
        if key not in identities:identities.append(key)
        blocks.append(identities.index(key))
    assert len(identities)==130,len(identities)
    # One broad fragment, four gravel pieces and fifteen chips; wrapped copies
    # share identities, so deletion never opens a seam at the texture border.
    removed={0,7,13,21,28,*random.Random(105907).sample(range(30,130),15)}
    bm=bmesh.new();bm.from_mesh(mesh);bm.verts.ensure_lookup_table()
    bmesh.ops.delete(bm,geom=[v for v in bm.verts if blocks[v.index//12] in removed],context='VERTS')
    for vertex in bm.verts:vertex.co.z*=.9
    bm.to_mesh(mesh);bm.free();mesh.update()
    r6.r5.soften_faces(mesh)
    for material in mesh.materials:
        surface=material.node_tree.nodes['Surface']
        mineral=surface.inputs['Base Color'].links[0].from_socket
        soil=material.node_tree.nodes['Soil coated edges'].inputs[2].links[0].from_socket
        coat=g.node(material,'ShaderNodeMixRGB','Settled soil dust')
        coat.inputs[0].default_value=.16
        g.wire(material,mineral,coat.inputs[1]);g.wire(material,soil,coat.inputs[2])
        d.assign(material,coat.outputs[0],surface.inputs['Base Color'])
    receiver=bpy.data.materials.new(VARIANT+' Soil receiver');receiver.use_nodes=True
    c.plane(VARIANT+' Soil target',-.025,receiver)
    scene.cycles.samples=32
    return {'variant':VARIANT,'readableStones':25,'chips':85,'removed':len(removed),'preservedPlacement':True}


def bake(channel):
    bind()
    return d.bake('Soil',channel)


def save_source():
    bind();return c.save_source()
