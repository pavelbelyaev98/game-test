"""Sunny r2: retain r1 colors, restore crisp authored soil and grass definition.

Execute in Blender MCP. initialize() creates an isolated variant inside the
owned cartoon scene. bake(kind, channel) writes staged 1024px maps; save_source()
retains all original/r1/r2 editable sources in GroundTextures.blend.
"""
import bpy
import math
import random
import types
from pathlib import Path

ROOT = Path(r'C:/Users/pavel/Desktop/Dev/CompanyProjects/game-test')
SOURCE = ROOT/'art/ground-textures'
VARIANT = 'A-Sunny-r2'
SIZE = 1024


def bind():
    global c,g,scene
    c = types.ModuleType('cartoon_detail_helpers')
    exec(compile((SOURCE/'cartoon_ground.py').read_text(encoding='utf-8'),'cartoon_ground.py','exec'),c.__dict__)
    c.bind()
    g,scene = c.g,c.scene


def assign(m,source,socket):
    for link in list(socket.links):m.node_tree.links.remove(link)
    g.wire(m,source,socket)


def soil_material(name,stone=-1):
    m=bpy.data.materials.new(name)
    m.use_nodes=True
    m.node_tree.nodes.clear()
    coords=g.periodic_coords(m)
    patches=g.noise(m,coords,2.4,1,'Retained Sunny color drift')
    crumbs=g.noise(m,coords,16,2,'Shaped compacted soil crumbs')
    grain=g.noise(m,coords,48,1,'Readable small aggregates')
    palette=c.VARIANTS['A-Sunny']['soil']
    # Preserve the same mean hue while moving detail from broad blurry clouds
    # into bounded centimetre-scale aggregates and actual embedded geometry.
    variation=g.calc(m,'ADD',g.calc(m,'MULTIPLY',patches,.25),g.calc(m,'MULTIPLY',crumbs,.75))
    color=g.ramp(m,variation,[(.18,palette[0]),(.5,palette[1]),(.83,palette[2])],'Sunny warm earth')
    modulation=g.ramp(m,grain,[(.22,(.60,.60,.60)),(.43,(.90,.90,.90)),(.57,(1.07,1.07,1.07)),(.79,(1.24,1.24,1.24))],'Crisp aggregate variation')
    mix=g.node(m,'ShaderNodeMixRGB','Defined earth texture')
    mix.blend_type='MULTIPLY'
    mix.inputs[0].default_value=.55
    g.wire(m,color,mix.inputs[1])
    g.wire(m,modulation,mix.inputs[2])
    color=mix.outputs[0]
    if stone>=0:
        coating=g.node(m,'ShaderNodeMixRGB','Dirt coated stone color')
        coating.blend_type='MULTIPLY'
        coating.inputs[0].default_value=.65
        g.wire(m,color,coating.inputs[1])
        t=.79+(stone%6)*.066
        coating.inputs[2].default_value=(t*.97,t,t*1.06,1)
        color=coating.outputs[0]
    height=g.calc(m,'ADD',g.calc(m,'MULTIPLY',crumbs,.65),g.calc(m,'MULTIPLY',grain,.35))
    bump=g.node(m,'ShaderNodeBump','Aggregate relief')
    bump.inputs['Distance'].default_value=.0045 if stone<0 else .0025
    bump.inputs['Strength'].default_value=.48
    g.wire(m,height,bump.inputs['Height'])
    ao=g.node(m,'ShaderNodeAmbientOcclusion','Embedded contact')
    ao.inputs['Distance'].default_value=.016
    ao.samples=16
    cavity=g.node(m,'ShaderNodeMixRGB','Restrained contact color')
    cavity.blend_type='MULTIPLY'
    cavity.inputs[0].default_value=.26
    g.wire(m,color,cavity.inputs[1])
    g.wire(m,ao.outputs['AO'],cavity.inputs[2])
    p=g.node(m,'ShaderNodeBsdfPrincipled','Surface')
    g.wire(m,cavity.outputs[0],p.inputs['Base Color'])
    g.wire(m,bump.outputs['Normal'],p.inputs['Normal'])
    g.wire(m,g.calc(m,'ADD',.84,g.calc(m,'MULTIPLY',grain,.10)),p.inputs['Roughness'])
    out=g.node(m,'ShaderNodeOutputMaterial','Output')
    g.wire(m,p.outputs[0],out.inputs[0])
    g.node(m,'ShaderNodeEmission','Bake emission')
    return m


def turf_material(index):
    m=bpy.data.materials.new(VARIANT+' grass blade '+str(index))
    m.use_nodes=True
    m.node_tree.nodes.clear()
    p=g.node(m,'ShaderNodeBsdfPrincipled','Surface')
    green=c.VARIANTS['A-Sunny']['turf'][1]
    factor=.67+index*.050
    p.inputs['Base Color'].default_value=(green[0]*factor,green[1]*factor,green[2]*factor,1)
    p.inputs['Roughness'].default_value=.86
    out=g.node(m,'ShaderNodeOutputMaterial','Output')
    g.wire(m,p.outputs[0],out.inputs[0])
    g.node(m,'ShaderNodeEmission','Bake emission')
    return m


def grass_geometry():
    rng=random.Random(105202)
    materials=[turf_material(i) for i in range(15)]
    verts,faces,slots=[],[],[]
    for clump in range(2800):
        x,y=rng.uniform(0,2),rng.uniform(0,2)
        heading=rng.random()*math.tau
        for blade in range(5):
            angle=heading+rng.uniform(-1.1,1.1)
            length,width=rng.uniform(.045,.105),rng.uniform(.009,.020)
            x0,y0=x+rng.uniform(-.024,.024),y+rng.uniform(-.024,.024)
            dx,dy=math.cos(angle),math.sin(angle)
            bend=rng.uniform(-.018,.018)
            height=rng.uniform(.005,.013)
            # A centre ridge splits each broad leaf into two readable planes.
            points=[(x0-dy*width/2,y0+dx*width/2,.001),
                    (x0+dy*width/2,y0-dx*width/2,.001),
                    (x0+dx*length*.52-dy*width*.42,y0+dy*length*.52+dx*width*.42,height*.65),
                    (x0+dx*length*.52,y0+dy*length*.52,height),
                    (x0+dx*length*.52+dy*width*.42,y0+dy*length*.52-dx*width*.42,height*.65),
                    (x0+dx*length-dy*bend,y0+dy*length+dx*bend,height*.45)]
            slot=rng.randrange(len(materials))
            oxs=[0]+([2] if min(v[0] for v in points)<0 else [])+([-2] if max(v[0] for v in points)>2 else [])
            oys=[0]+([2] if min(v[1] for v in points)<0 else [])+([-2] if max(v[1] for v in points)>2 else [])
            for ox in oxs:
                for oy in oys:
                    start=len(verts)
                    verts.extend((a+ox,b+oy,z) for a,b,z in points)
                    # Counter-clockwise faces keep tangent normals upward.
                    for face in ((0,1,3),(0,3,2),(1,4,3),(2,3,5),(3,4,5)):
                        faces.append(tuple(start+i for i in face));slots.append(slot)
    mesh=bpy.data.meshes.new(VARIANT+' wrapped ridged blade geometry')
    mesh.from_pydata(verts,[],faces);mesh.update()
    obj=bpy.data.objects.new(VARIANT+' Turf strokes',mesh)
    scene.collection.objects.link(obj)
    for material in materials:mesh.materials.append(material)
    for face,slot in zip(mesh.polygons,slots):face.material_index=slot
    return obj


def initialize():
    bind()
    if scene.objects.get(VARIANT+' Soil source'):raise RuntimeError('Refinement already exists; bind and bake it.')
    for kind in ('Soil','Turf'):
        if kind=='Soil':material=soil_material(VARIANT+' compacted earth')
        else:
            material=c.surface_material(VARIANT,'Turf',c.VARIANTS['A-Sunny'])
            # Underlay texture stays subtle beneath the denser authored leaves.
            material.node_tree.nodes['Broad painted patches'].inputs['Scale'].default_value=5
        c.plane(VARIANT+' '+kind+' source',0,material)
        receiver=bpy.data.materials.new(VARIANT+' '+kind+' receiver');receiver.use_nodes=True
        c.plane(VARIANT+' '+kind+' target',-.025,receiver)
    original=bpy.data.scenes['Ground texture authoring'].objects['Embedded stone source - wrapped']
    rocks=bpy.data.objects.new(VARIANT+' Soil relief',original.data.copy())
    scene.collection.objects.link(rocks)
    rocks.data.materials.clear()
    for i in range(18):rocks.data.materials.append(soil_material(VARIANT+' embedded stone '+str(i),i))
    grass_geometry()
    scene.cycles.samples=16
    return {'variant':VARIANT,'resolution':SIZE,'grassBlades':14000,'reusedEmbeddedStoneSource':True}


def bake(kind,channel):
    bind()
    bpy.ops.object.select_all(action='DESELECT')
    for obj in scene.objects:obj.hide_render=True
    target=scene.objects[VARIANT+' '+kind+' target']
    sources=[scene.objects[VARIANT+' '+kind+' source'],scene.objects[VARIANT+' '+kind+(' relief' if kind=='Soil' else ' strokes')]]
    for obj in [target,*sources]:obj.hide_render=False;obj.select_set(True)
    bpy.context.view_layer.objects.active=target
    mats={m for obj in sources for m in obj.data.materials}
    for m in mats:
        p,out,em=[m.node_tree.nodes[n] for n in ('Surface','Output','Bake emission')]
        if channel=='Normal':assign(m,p.outputs[0],out.inputs[0])
        else:
            color=p.inputs['Base Color']
            color=color.links[0].from_socket if color.is_linked else tuple(color.default_value)
            if channel=='Roughness':
                roughness=p.inputs['Roughness']
                mask=m.node_tree.nodes.get('Packed surface mask') or g.node(m,'ShaderNodeCombineColor','Packed surface mask')
                assign(m,roughness.links[0].from_socket if roughness.is_linked else roughness.default_value,mask.inputs[0])
                ao=m.node_tree.nodes.get('Embedded contact')
                assign(m,ao.outputs['AO'] if ao else 1,mask.inputs[1])
                mask.inputs[2].default_value=1
                color=mask.outputs[0]
            assign(m,color,em.inputs['Color']);assign(m,em.outputs[0],out.inputs[0])
    material=target.data.materials[0]
    node=material.node_tree.nodes.get(channel) or g.node(material,'ShaderNodeTexImage',channel)
    im=node.image or bpy.data.images.new(VARIANT+' '+kind+' '+channel,SIZE,SIZE,alpha=False)
    im.colorspace_settings.name='sRGB' if channel=='Albedo' else 'Non-Color'
    node.image=im;material.node_tree.nodes.active=node
    bpy.ops.object.bake(type='NORMAL' if channel=='Normal' else 'EMIT',use_selected_to_active=True,cage_extrusion=.10,max_ray_distance=.16,margin=0)
    dest=SOURCE/'trials'/VARIANT;dest.mkdir(parents=True,exist_ok=True)
    im.filepath_raw=str(dest/(kind+'_'+channel+'.png'));im.file_format='PNG';im.save();im.pack()
    for m in mats:assign(m,m.node_tree.nodes['Surface'].outputs[0],m.node_tree.nodes['Output'].inputs[0])
    return {'path':im.filepath_raw,'size':SIZE}


def save_source():
    bind()
    return c.save_source()
