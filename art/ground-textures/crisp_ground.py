"""Sunny r6: rebake approved r5 soil/r3 turf at a true 2048px resolution.

Execute through Blender MCP. Geometry, mineral placement and palette are retained;
independent copies preserve the previous trials and their packed images.
"""
import bpy
import types
from pathlib import Path

SOURCE = Path(r'C:/Users/pavel/Desktop/Dev/CompanyProjects/game-test/art/ground-textures')
VARIANT = 'A-Sunny-r6'
SIZE = 2048


def bind():
    global r5,d,c,g,scene
    r5=types.ModuleType('scattered_soil')
    exec(compile((SOURCE/'scattered_soil.py').read_text(),'scattered_soil.py','exec'),r5.__dict__)
    r5.bind();d,c,g,scene=r5.d,r5.c,r5.g,r5.scene
    d.VARIANT=VARIANT;d.SIZE=SIZE;d.bind=lambda:None


def initialize():
    bind()
    if scene.objects.get(VARIANT+' Soil source'):
        raise RuntimeError('Retain existing r6; bind and bake it.')
    for kind,previous,detail in (('Soil','A-Sunny-r5',' relief'),('Turf','A-Sunny-r3',' strokes')):
        materials={}
        for suffix in (' source',detail):
            src=scene.objects[previous+' '+kind+suffix]
            obj=src.copy();obj.data=src.data.copy();obj.name=VARIANT+' '+kind+suffix
            scene.collection.objects.link(obj)
            for slot in obj.material_slots:
                original=slot.material
                if original not in materials:
                    material=original.copy();material.name=original.name.replace(previous,VARIANT)
                    materials[original]=material
                slot.material=materials[original]
        receiver=bpy.data.materials.new(VARIANT+' '+kind+' receiver');receiver.use_nodes=True
        c.plane(VARIANT+' '+kind+' target',-.025,receiver)
    scene.cycles.samples=32
    return {'variant':VARIANT,'size':SIZE,'readableStones':30,'chips':100}


def bake(kind,channel):
    bind()
    if kind=='Soil' and channel=='Roughness':
        for suffix,value in ((' source',0),(' relief',1)):
            for material in scene.objects[VARIANT+' Soil'+suffix].data.materials:
                mask=material.node_tree.nodes['Packed surface mask']
                identity=material.node_tree.nodes['Embedded mineral coverage']
                identity.outputs[0].default_value=value
                d.assign(material,identity.outputs[0],mask.inputs[2])
    return d.bake(kind,channel)


def save_source():
    bind();return c.save_source()
