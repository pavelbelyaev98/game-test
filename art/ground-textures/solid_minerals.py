"""Sunny r8: retain the r7 stones, remove dirt-pattern ghosting from their faces.

Run through Blender MCP. Only albedo changes; use r7 normal/surface masks and
r6 turf. The source copies and previous trials remain independently editable.
"""
import bpy
import types
from pathlib import Path

SOURCE = Path(r'C:/Users/pavel/Desktop/Dev/CompanyProjects/game-test/art/ground-textures')
VARIANT = 'A-Sunny-r8'


def bind():
    global r7, d, c, g, scene
    r7 = types.ModuleType('settled_soil')
    exec(compile((SOURCE/'settled_soil.py').read_text(), 'settled_soil.py', 'exec'), r7.__dict__)
    r7.bind()
    d, c, g, scene = r7.d, r7.c, r7.g, r7.scene
    d.VARIANT = VARIANT
    d.SIZE = 2048
    d.bind = lambda: None


def initialize():
    bind()
    if scene.objects.get(VARIANT+' Soil source'):
        raise RuntimeError('Retain existing r8; bind and bake it.')
    for suffix in (' source', ' relief'):
        src = scene.objects['A-Sunny-r7 Soil'+suffix]
        obj = src.copy()
        obj.data = src.data.copy()
        obj.name = VARIANT+' Soil'+suffix
        scene.collection.objects.link(obj)
        for slot in obj.material_slots:
            material = slot.material.copy()
            material.name = material.name.replace('A-Sunny-r7', VARIANT)
            slot.material = material
    # Uniform mineral warmth uses the mean of the existing soil bake, decoded
    # from sRGB. It never projects the dirt's aggregate pattern through a stone.
    soil_srgb = (.630854249, .425170541, .265624285)
    soil_linear = tuple(((v+.055)/1.055)**2.4 for v in soil_srgb)+(1,)
    for material in scene.objects[VARIANT+' Soil relief'].data.materials:
        nodes = material.node_tree.nodes
        coat = nodes['Soil coated edges']
        height = nodes['Stone height'].outputs['Z']
        exposed = g.calc(material, 'MINIMUM', 1, g.calc(material, 'MAXIMUM', 0,
            g.calc(material, 'MULTIPLY', height, 220)))
        edge_dust = g.calc(material, 'ADD', .04, g.calc(material, 'MULTIPLY',
            g.calc(material, 'SUBTRACT', 1, exposed), .12))
        d.assign(material, edge_dust, coat.inputs[0])
        d.assign(material, soil_linear, coat.inputs[2])
        settled = nodes['Settled soil dust']
        settled.inputs[0].default_value = .22
        d.assign(material, soil_linear, settled.inputs[2])
    receiver = bpy.data.materials.new(VARIANT+' Soil receiver')
    receiver.use_nodes = True
    c.plane(VARIANT+' Soil target', -.025, receiver)
    scene.cycles.samples = 32
    return {'variant': VARIANT, 'retainedStoneIdentities': 110,
            'albedoOnly': True, 'matrixUnchanged': True}


def bake():
    bind()
    return d.bake('Soil', 'Albedo')


def save_source():
    bind()
    return c.save_source()
