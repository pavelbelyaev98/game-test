"""Task 105: original cartoon soil/turf, authored and baked through Blender MCP.

Run initialize(), then bake(variant, kind, channel) separately for responsive MCP
calls. Exports are staged outside Unity; promote only a complete six-map set.
The existing GroundTextures.blend retains the previous editable ground too.
"""
import bpy
import math
import random
import types
from pathlib import Path

ROOT = Path(r'C:/Users/pavel/Desktop/Dev/CompanyProjects/game-test')
SOURCE = ROOT / 'art/ground-textures'
SIZE = 512
VARIANTS = {
    'A-Sunny': {'soil': ((.235,.085,.032),(.37,.162,.064),(.46,.225,.10)),
                'turf': ((.055,.255,.022),(.13,.41,.040),(.24,.51,.073)),
                'scale': 2.4, 'contrast': .16, 'relief': .002, 'blades': 180},
    'B-Soft': {'soil': ((.29,.15,.08),(.41,.235,.135),(.49,.295,.18)),
               'turf': ((.105,.28,.059),(.19,.40,.09),(.30,.51,.145)),
               'scale': 1.65, 'contrast': .10, 'relief': .001, 'blades': 100},
    'C-Bold': {'soil': ((.24,.09,.026),(.39,.18,.05),(.53,.285,.093)),
               'turf': ((.023,.24,.038),(.045,.40,.060),(.12,.54,.10)),
               'scale': 3.4, 'contrast': .23, 'relief': .003, 'blades': 240},
}


def initialize():
    global g, scene
    if bpy.data.scenes.get('Cartoon ground authoring'):
        raise RuntimeError('Use bind() to reuse the existing cartoon source.')
    if not bpy.data.scenes.get('Ground texture authoring'):
        with bpy.data.libraries.load(str(SOURCE/'GroundTextures.blend'), link=False) as (src,dst):
            dst.scenes = ['Ground texture authoring']
    scene = bpy.data.scenes.new('Cartoon ground authoring')
    bind()
    scene.render.engine = 'CYCLES'
    scene.cycles.samples = 4
    scene.cycles.use_denoising = False
    scene.render.bake.margin = 0
    scene.view_settings.view_transform = 'Standard'
    for variant, values in VARIANTS.items():
        for kind in ('Soil','Turf'):
            material = surface_material(variant, kind, values)
            bed = plane(variant+' '+kind+' source', 0, material)
            receiver = bpy.data.materials.new(variant+' '+kind+' receiver')
            receiver.use_nodes = True
            plane(variant+' '+kind+' target', -.02, receiver)
            if kind == 'Turf': blades(variant, values)
    return {'scene':scene.name, 'objects':len(scene.objects), 'resolution':SIZE}


def bind():
    global g, scene
    g = types.ModuleType('cartoon_ground_helpers')
    exec(compile((SOURCE/'create_ground.py').read_text(encoding='utf-8'), 'create_ground.py', 'exec'),g.__dict__)
    scene = bpy.data.scenes['Cartoon ground authoring']
    bpy.context.window.scene = scene


def surface_material(variant, kind, values):
    m = bpy.data.materials.new(variant+' '+kind+' painted surface')
    m.use_nodes = True
    m.node_tree.nodes.clear()
    coords = g.periodic_coords(m)
    broad = g.noise(m,coords,values['scale'],1,'Broad painted patches')
    middle = g.noise(m,coords,9,1,'Quiet brush variation')
    mix = g.calc(m,'ADD',g.calc(m,'MULTIPLY',broad,.86),g.calc(m,'MULTIPLY',middle,.14))
    palette = values[kind.lower()]
    colour = g.ramp(m,mix,[(.19,palette[0]),(.5,palette[1]),(.81,palette[2])],'Cartoon palette')
    m.node_tree.nodes['Cartoon palette'].color_ramp.interpolation = 'EASE'
    # Sparse mineral shapes are painted into earth with low contrast, never
    # rendered as collectible-sized protruding stones or peppery micro-grit.
    if kind == 'Soil':
        cell = g.node(m,'ShaderNodeTexVoronoi','Small angular mineral accents')
        cell.voronoi_dimensions = '4D'
        cell.distance = 'MANHATTAN'
        g.wire(m,coords[0],cell.inputs['Vector'])
        g.wire(m,coords[1],cell.inputs['W'])
        cell.inputs['Scale'].default_value = 4.7
        mask = g.calc(m,'LESS_THAN',cell.outputs['Distance'],.45)
        accent = g.node(m,'ShaderNodeMixRGB','Subtle embedded accents')
        g.wire(m,g.calc(m,'MULTIPLY',mask,values['contrast']),accent.inputs[0])
        g.wire(m,colour,accent.inputs[1])
        accent.inputs[2].default_value = (*palette[0],1)
        colour = accent.outputs[0]
    bump = g.node(m,'ShaderNodeBump','Soft broad relief')
    g.wire(m,middle,bump.inputs['Height'])
    bump.inputs['Distance'].default_value = values['relief']
    bump.inputs['Strength'].default_value = .25
    p = g.node(m,'ShaderNodeBsdfPrincipled','Surface')
    g.wire(m,colour,p.inputs['Base Color'])
    g.wire(m,bump.outputs['Normal'],p.inputs['Normal'])
    p.inputs['Roughness'].default_value = .93
    out = g.node(m,'ShaderNodeOutputMaterial','Output')
    g.wire(m,p.outputs[0],out.inputs[0])
    g.node(m,'ShaderNodeEmission','Bake emission')
    return m


def plane(name,z,material):
    bpy.ops.mesh.primitive_plane_add(size=2,location=(1,1,z))
    obj = bpy.context.object
    obj.name = name
    obj.data.materials.append(material)
    return obj


def blades(variant,values):
    rng = random.Random(10523)
    mats = []
    for i in range(5):
        m = bpy.data.materials.new(variant+' painted grass stroke '+str(i))
        m.use_nodes = True
        m.node_tree.nodes.clear()
        p = g.node(m,'ShaderNodeBsdfPrincipled','Surface')
        base = values['turf'][1]
        # Broad directional strokes remain near the lawn value, avoiding
        # bright scattered triangles which could read as loose objects.
        p.inputs['Base Color'].default_value = (base[0]*(.85+i*.08),base[1]*(.91+i*.045),base[2]*(.8+i*.1),1)
        p.inputs['Roughness'].default_value = .93
        out = g.node(m,'ShaderNodeOutputMaterial','Output')
        g.wire(m,p.outputs[0],out.inputs[0])
        g.node(m,'ShaderNodeEmission','Bake emission')
        mats.append(m)
    verts,faces,slots = [],[],[]
    for i in range(values['blades']):
        x,y = rng.uniform(0,2),rng.uniform(0,2)
        heading = rng.uniform(0,math.tau)
        for fan in (-.5,0,.5):
            angle = heading+fan
            length,width = rng.uniform(.055,.11),rng.uniform(.014,.024)
            dx,dy = math.cos(angle),math.sin(angle)
            points = [(x-dy*width/2,y+dx*width/2,.0004),
                      (x+dy*width/2,y-dx*width/2,.0004),
                      (x+dx*length*.55+dy*width*.25,y+dy*length*.55-dx*width*.25,.001),
                      (x+dx*length,y+dy*length,.0005)]
            slot = rng.randrange(len(mats))
            for ox in (-2,0,2):
                for oy in (-2,0,2):
                    if not any(-.12<px+ox<2.12 and -.12<py+oy<2.12 for px,py,pz in points):continue
                    start = len(verts)
                    verts.extend((px+ox,py+oy,pz) for px,py,pz in points)
                    faces.append(tuple(range(start,start+4)))
                    slots.append(slot)
    mesh = bpy.data.meshes.new(variant+' wrapped grass strokes')
    mesh.from_pydata(verts,[],faces)
    mesh.update()
    obj = bpy.data.objects.new(variant+' Turf strokes',mesh)
    scene.collection.objects.link(obj)
    for m in mats:mesh.materials.append(m)
    for face,slot in zip(mesh.polygons,slots):face.material_index=slot


def bake(variant,kind,channel):
    bpy.context.window.scene = scene
    bpy.ops.object.select_all(action='DESELECT')
    for obj in scene.objects:obj.hide_render=True
    target = scene.objects[variant+' '+kind+' target']
    sources = [scene.objects[variant+' '+kind+' source']]
    if kind=='Turf':sources.append(scene.objects[variant+' Turf strokes'])
    for obj in [target,*sources]:
        obj.hide_render=False
        obj.select_set(True)
    bpy.context.view_layer.objects.active=target
    mats={m for obj in sources for m in obj.data.materials}
    for m in mats:
        p,out,em = [m.node_tree.nodes[n] for n in ('Surface','Output','Bake emission')]
        if channel=='Normal':g.wire(m,p.outputs[0],out.inputs[0])
        else:
            source = p.inputs['Base Color']
            color = source.links[0].from_socket if source.is_linked else tuple(source.default_value)
            # Soil mask retains the shader's R roughness / G occlusion contract.
            for link in list(em.inputs['Color'].links):m.node_tree.links.remove(link)
            g.wire(m,color if channel=='Albedo' else (.93,1,1,1),em.inputs['Color'])
            g.wire(m,em.outputs[0],out.inputs[0])
    receiver=target.data.materials[0]
    n=receiver.node_tree.nodes.get(channel) or g.node(receiver,'ShaderNodeTexImage',channel)
    im=n.image or bpy.data.images.new(variant+' '+kind+' '+channel,SIZE,SIZE,alpha=False)
    im.colorspace_settings.name='sRGB' if channel=='Albedo' else 'Non-Color'
    n.image=im
    receiver.node_tree.nodes.active=n
    bpy.ops.object.bake(type='NORMAL' if channel=='Normal' else 'EMIT',use_selected_to_active=True,cage_extrusion=.06,max_ray_distance=.12,margin=0)
    dest=SOURCE/'trials'/variant
    dest.mkdir(parents=True,exist_ok=True)
    im.filepath_raw=str(dest/(kind+'_'+channel+'.png'))
    im.file_format='PNG'
    im.save()
    im.pack()
    for m in mats:g.wire(m,m.node_tree.nodes['Surface'].outputs[0],m.node_tree.nodes['Output'].inputs[0])
    return {'path':im.filepath_raw,'resolution':SIZE}


def save_source():
    sources={scene,bpy.data.scenes['Ground texture authoring']}
    for filename in ('cartoon_ground.py','sunny_detail.py','living_ground.py','varied_soil.py','scattered_soil.py','crisp_ground.py','settled_soil.py','solid_minerals.py','create_ground.py','rocky_soil.py','LICENSE.txt','README.md'):
        text=bpy.data.texts.get(filename) or bpy.data.texts.new(filename)
        text.clear()
        text.write((SOURCE/filename).read_text(encoding='utf-8'))
        sources.add(text)
    bpy.data.libraries.write(str(SOURCE/'GroundTextures.blend'),sources,fake_user=True,compress=True)
    return str(SOURCE/'GroundTextures.blend')
