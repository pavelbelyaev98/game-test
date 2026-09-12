"""Sunny r3: tactile soil and layered turf. Execute only through Blender MCP.

Retains r2 geometry in independent copies. Source geometry is baked to the
existing six maps; no reference-game pixels or runtime mesh are generated here.
"""
import bpy
import types
from pathlib import Path

ROOT = Path(r'C:/Users/pavel/Desktop/Dev/CompanyProjects/game-test')
SOURCE = ROOT/'art/ground-textures'
VARIANT = 'A-Sunny-r3'
SIZE = 1024


def bind():
    global d,g,c,scene
    d=types.ModuleType('sunny_surface_helpers')
    exec(compile((SOURCE/'sunny_detail.py').read_text(), 'sunny_detail.py','exec'), d.__dict__)
    d.bind()
    g,c,scene=d.g,d.c,d.scene
    d.VARIANT=VARIANT


def soil_material(index=-1):
    m=d.soil_material(VARIANT+' earth' if index<0 else VARIANT+' stone '+str(index))
    n=m.node_tree.nodes
    p=n['Surface']
    coords=g.periodic_coords(m)
    fleck=g.noise(m,coords,125,1,'Small mineral grains')
    grain=g.ramp(m,fleck,[(.25,(.52,.43,.34)),(.48,(.94,.92,.89)),(.65,(1.25,1.16,1.01)),(.76,(1.9,1.7,1.35))],'Warm mineral flecks')
    mix=g.node(m,'ShaderNodeMixRGB','Visible grains')
    mix.blend_type='MULTIPLY';mix.inputs[0].default_value=.72
    g.wire(m,p.inputs['Base Color'].links[0].from_socket,mix.inputs[1]);g.wire(m,grain,mix.inputs[2])
    d.assign(m,mix.outputs[0],p.inputs['Base Color'])
    n['Shaped compacted soil crumbs'].inputs['Scale'].default_value=10
    n['Crisp aggregate variation'].color_ramp.elements[0].color=(.75,.75,.75,1)
    n['Aggregate relief'].inputs['Distance'].default_value=.0028
    if index>=0:
        dust=g.noise(m,coords,6,1,'Patchy adhering soil')
        palettes=[(.115,.09,.067),(.16,.135,.10),(.24,.19,.12),(.095,.086,.068),(.27,.25,.19),(.17,.15,.12)]
        base=palettes[index%6]
        stone=g.ramp(m,fleck,[(.2,tuple(v*.72 for v in base)),(.8,tuple(v*1.25 for v in base))],'Distinct embedded stone faces')
        coat=g.node(m,'ShaderNodeMixRGB','Soil coated edges')
        geo=g.node(m,'ShaderNodeNewGeometry','Stone geometry')
        xyz=g.node(m,'ShaderNodeSeparateXYZ','Stone height')
        g.wire(m,geo.outputs['Position'],xyz.inputs[0])
        exposed=g.calc(m,'MINIMUM',1,g.calc(m,'MAXIMUM',0,g.calc(m,'MULTIPLY',xyz.outputs['Z'],220)))
        coating=g.calc(m,'ADD',g.calc(m,'MULTIPLY',g.calc(m,'SUBTRACT',1,exposed),.48),g.calc(m,'MULTIPLY',dust,.25))
        g.wire(m,coating,coat.inputs[0]);g.wire(m,stone,coat.inputs[1]);g.wire(m,mix.outputs[0],coat.inputs[2])
        d.assign(m,coat.outputs[0],p.inputs['Base Color'])
    return m


def grass_material(index):
    m=d.turf_material(index)
    p=m.node_tree.nodes['Surface']
    base=tuple(v*1.2 for v in p.inputs['Base Color'].default_value[:3])+(1,)
    geo=g.node(m,'ShaderNodeNewGeometry','Blade geometry')
    sep=g.node(m,'ShaderNodeSeparateXYZ','Blade ridge normal')
    g.wire(m,geo.outputs['Normal'],sep.inputs[0])
    ridge=g.calc(m,'ADD',.62,g.calc(m,'MULTIPLY',sep.outputs['Z'],.38))
    color=g.node(m,'ShaderNodeMixRGB','Painted ridge')
    color.blend_type='MULTIPLY';color.inputs[0].default_value=1
    color.inputs[1].default_value=base;g.wire(m,ridge,color.inputs[2])
    ao=g.node(m,'ShaderNodeAmbientOcclusion','Embedded contact')
    ao.inputs['Distance'].default_value=.025;ao.samples=16
    contact=g.node(m,'ShaderNodeMixRGB','Blade root contact')
    contact.blend_type='MULTIPLY';contact.inputs[0].default_value=.18
    g.wire(m,color.outputs[0],contact.inputs[1]);g.wire(m,ao.outputs['AO'],contact.inputs[2])
    d.assign(m,contact.outputs[0],p.inputs['Base Color'])
    return m


def initialize():
    bind()
    if scene.objects.get(VARIANT+' Soil source'):raise RuntimeError('Use bind/bake for existing r3.')
    for kind in ('Soil','Turf'):
        mat=soil_material() if kind=='Soil' else c.surface_material(VARIANT,'Turf',c.VARIANTS['A-Sunny'])
        if kind=='Turf':
            ramp=mat.node_tree.nodes['Painted surface palette'].color_ramp if mat.node_tree.nodes.get('Painted surface palette') else None
            if ramp:
                for e in ramp.elements:e.color=tuple(v*.65 for v in e.color[:3])+(1,)
        c.plane(VARIANT+' '+kind+' source',0,mat)
        receiver=bpy.data.materials.new(VARIANT+' '+kind+' receiver');receiver.use_nodes=True
        c.plane(VARIANT+' '+kind+' target',-.025,receiver)
        suffix=' relief' if kind=='Soil' else ' strokes'
        src=scene.objects['A-Sunny-r2 '+kind+suffix]
        obj=bpy.data.objects.new(VARIANT+' '+kind+suffix,src.data.copy());scene.collection.objects.link(obj)
        obj.data.materials.clear()
        for i in range(18 if kind=='Soil' else 15):obj.data.materials.append(soil_material(i) if kind=='Soil' else grass_material(i))
    scene.cycles.samples=16
    return {'variant':VARIANT,'size':SIZE,'authored':'original retained Blender geometry'}


def bake(kind,channel):
    bind()
    # Reuse the deterministic r2 bake; turf surface mask now preserves contact
    # occlusion in G too. B stays reserved for the original authored surface.
    d.bind=lambda:None
    d.VARIANT=VARIANT;d.SIZE=SIZE
    if kind=='Turf':
        for i,mat in enumerate(scene.objects[VARIANT+' Turf strokes'].data.materials):
            mat.node_tree.nodes['Blade root contact'].inputs[0].default_value=.18
            green=c.VARIANTS['A-Sunny']['turf'][1]
            factor=(.67+i*.05)*1.2
            mat.node_tree.nodes['Painted ridge'].inputs[1].default_value=tuple(v*factor for v in green)+(1,)
    return d.bake(kind,channel)


def save_source():
    bind()
    text=bpy.data.texts.get('living_ground.py') or bpy.data.texts.new('living_ground.py')
    text.clear();text.write((SOURCE/'living_ground.py').read_text())
    return c.save_source()
