"""Turf and its original underlay authoring. Run inside Blender through MCP.

Creates an isolated scene without changing existing objects. Bake one map at a
time with bake_map('Turf', channel), then save_source(). Current soil exports
belong to rocky_soil.py. The turf tile covers one metre.
"""
import bpy
import math
import random
from pathlib import Path

ROOT = Path(r'C:/Users/pavel/Desktop/Dev/CompanyProjects/game-test')
DEST = ROOT / 'unity/Assets/Content/GroundTextures'
SOURCE = ROOT / 'art/ground-textures'
SIZE = 2048


def node(mat, kind, name):
    n = mat.node_tree.nodes.new(kind)
    n.label = n.name = name
    return n


def wire(mat, value, socket):
    if isinstance(value, (int, float, tuple)):
        socket.default_value = value
    else:
        mat.node_tree.links.new(value, socket)


def calc(mat, op, a, b=0):
    n = node(mat, 'ShaderNodeMath', op)
    n.operation = op
    wire(mat, a, n.inputs[0])
    wire(mat, b, n.inputs[1])
    return n.outputs[0]


def ramp(mat, value, stops, name):
    n = node(mat, 'ShaderNodeValToRGB', name)
    r = n.color_ramp
    for e in list(r.elements)[2:]:
        r.elements.remove(e)
    for i, (position, color) in enumerate(stops):
        e = r.elements[i] if i < 2 else r.elements.new(position)
        e.position = position
        e.color = (*color, 1)
    wire(mat, value, n.inputs[0])
    return n.outputs['Color']


def periodic_coords(mat):
    tex = node(mat, 'ShaderNodeTexCoord', 'One metre XY coordinates')
    sep = node(mat, 'ShaderNodeSeparateXYZ', 'Separate position')
    wire(mat, tex.outputs['Generated'], sep.inputs[0])
    # UVs extend beyond the tile on the source bed, preserving periodicity.
    wire(mat, tex.outputs['UV'], sep.inputs[0])
    u = calc(mat, 'MULTIPLY', sep.outputs['X'], math.tau)
    v = calc(mat, 'MULTIPLY', sep.outputs['Y'], math.tau)
    xyz = node(mat, 'ShaderNodeCombineXYZ', 'Seamless four dimensional torus')
    wire(mat, calc(mat, 'COSINE', u), xyz.inputs[0])
    wire(mat, calc(mat, 'SINE', u), xyz.inputs[1])
    wire(mat, calc(mat, 'COSINE', v), xyz.inputs[2])
    return xyz.outputs[0], calc(mat, 'SINE', v)


def noise(mat, coordinates, scale, detail, name):
    n = node(mat, 'ShaderNodeTexNoise', name)
    n.noise_dimensions = '4D'
    wire(mat, coordinates[0], n.inputs['Vector'])
    wire(mat, coordinates[1], n.inputs['W'])
    n.inputs['Scale'].default_value = scale
    n.inputs['Detail'].default_value = detail
    n.inputs['Roughness'].default_value = 0.72
    return n.outputs['Fac']


def make_soil():
    m = bpy.data.materials.new('Garden loam - periodic mineral and crumb layers')
    m.use_nodes = True
    m.node_tree.nodes.clear()
    coords = periodic_coords(m)
    cloud = noise(m, coords, 0.85, 3, 'Subtle broad earth colour')
    crumbs = noise(m, coords, 12, 4, 'Soil aggregates')
    grain = noise(m, coords, 115, 2, 'Fine mineral grains')
    pores = noise(m, coords, 40, 2, 'Pores')
    value = calc(m, 'ADD', calc(m, 'MULTIPLY', cloud, 0.4), calc(m, 'MULTIPLY', crumbs, 0.6))
    earth = ramp(m, value, [(0.18, (0.095, 0.040, 0.019)), (0.5, (0.24, 0.115, 0.051)), (0.8, (0.38, 0.215, 0.108))], 'Warm dry garden earth')
    pebbles = node(m, 'ShaderNodeTexVoronoi', 'Sparse embedded mineral flecks')
    pebbles.voronoi_dimensions = '4D'
    wire(m, coords[0], pebbles.inputs['Vector'])
    wire(m, coords[1], pebbles.inputs['W'])
    pebbles.inputs['Scale'].default_value = 7
    mineral_distance = calc(m, 'ADD', pebbles.outputs['Distance'], calc(m, 'MULTIPLY', grain, 0.2))
    fleck = calc(m, 'MULTIPLY', calc(m, 'MAXIMUM', calc(m, 'SUBTRACT', 0.38, mineral_distance), 0), 22)
    fleck = calc(m, 'MINIMUM', fleck, 1)
    mix = node(m, 'ShaderNodeMixRGB', 'Small grey ochre mineral inclusions')
    wire(m, calc(m, 'MULTIPLY', fleck, 0.5), mix.inputs[0])
    wire(m, earth, mix.inputs[1])
    mix.inputs[2].default_value = (0.25, 0.215, 0.16, 1)
    height = calc(m, 'ADD', calc(m, 'MULTIPLY', crumbs, 0.65), calc(m, 'MULTIPLY', grain, 0.24))
    height = calc(m, 'ADD', height, calc(m, 'MULTIPLY', fleck, 0.13))
    height = calc(m, 'SUBTRACT', height, calc(m, 'MULTIPLY', calc(m, 'LESS_THAN', pores, 0.33), 0.1))
    bump = node(m, 'ShaderNodeBump', 'Millimetre crumb relief')
    wire(m, height, bump.inputs['Height'])
    bump.inputs['Distance'].default_value = 0.009
    bump.inputs['Strength'].default_value = 0.6
    bsdf = node(m, 'ShaderNodeBsdfPrincipled', 'Surface')
    colour_grain = node(m, 'ShaderNodeMixRGB', 'Mineral grain colour modulation')
    colour_grain.blend_type = 'MULTIPLY'
    colour_grain.inputs[0].default_value = 0.38
    wire(m, mix.outputs[0], colour_grain.inputs[1])
    wire(m, calc(m, 'ADD', 0.45, calc(m, 'MULTIPLY', grain, 1.1)), colour_grain.inputs[2])
    wire(m, colour_grain.outputs[0], bsdf.inputs['Base Color'])
    wire(m, bump.outputs['Normal'], bsdf.inputs['Normal'])
    roughness = calc(m, 'ADD', 0.78, calc(m, 'MULTIPLY', grain, 0.18))
    wire(m, roughness, bsdf.inputs['Roughness'])
    out = node(m, 'ShaderNodeOutputMaterial', 'Output')
    wire(m, bsdf.outputs[0], out.inputs['Surface'])
    emission = node(m, 'ShaderNodeEmission', 'Bake emission')
    return m


def plane(name, z, extent, material):
    bpy.ops.mesh.primitive_plane_add(size=extent, location=(0.5, 0.5, z))
    o = bpy.context.object
    o.name = name
    o.data.materials.append(material)
    for loop in o.data.uv_layers.active.data:
        loop.uv = ((loop.uv.x - 0.5) * extent + 0.5, (loop.uv.y - 0.5) * extent + 0.5)
    return o


def make_grass():
    rng = random.Random(77031)
    mats = []
    for i in range(18):
        t = i / 17
        m = bpy.data.materials.new('Turf blade %02d' % i)
        m.use_nodes = True
        m.node_tree.nodes.clear()
        p = node(m, 'ShaderNodeBsdfPrincipled', 'Surface')
        # Olive to fresh garden green, with a few dry straw leaves.
        p.inputs['Base Color'].default_value = (0.10 + t * 0.16, 0.16 + t * 0.17, 0.023 + t * 0.037, 1)
        p.inputs['Roughness'].default_value = 0.84
        out = node(m, 'ShaderNodeOutputMaterial', 'Output')
        wire(m, p.outputs[0], out.inputs[0])
        node(m, 'ShaderNodeEmission', 'Bake emission')
        mats.append(m)
    verts, faces, slots = [], [], []
    for i in range(36000):
        x, y = rng.random(), rng.random()
        angle = rng.random() * math.tau
        length, width = rng.uniform(0.016, 0.043), rng.uniform(0.0014, 0.0031)
        dx, dy = math.cos(angle), math.sin(angle)
        bend = rng.uniform(-0.007, 0.007)
        h = rng.uniform(0.004, 0.018)
        material = rng.randrange(len(mats))
        shape = [(x-dy*width/2, y+dx*width/2, 0.001), (x+dy*width/2,y-dx*width/2,0.001),
                 (x+dx*length*.52+dy*width*.4,y+dy*length*.52-dx*width*.4,h*.7),
                 (x+dx*length*.52-dy*width*.4,y+dy*length*.52+dx*width*.4,h),
                 (x+dx*length-dy*bend,y+dy*length+dx*bend,h*.65)]
        # Exact wrap copies keep cut leaves continuous across tile edges.
        offsets_x = [0] + ([1] if min(v[0] for v in shape)<0 else []) + ([-1] if max(v[0] for v in shape)>1 else [])
        offsets_y = [0] + ([1] if min(v[1] for v in shape)<0 else []) + ([-1] if max(v[1] for v in shape)>1 else [])
        for ox in offsets_x:
            for oy in offsets_y:
                start = len(verts)
                verts.extend((vx+ox,vy+oy,vz) for vx,vy,vz in shape)
                faces.extend([(start,start+1,start+2,start+3),(start+3,start+2,start+4)])
                slots.extend([material,material])
    mesh = bpy.data.meshes.new('Periodic short turf blade source')
    mesh.from_pydata(verts, [], faces)
    mesh.update()
    o = bpy.data.objects.new('Turf source - wrapped leaves', mesh)
    bpy.context.scene.collection.objects.link(o)
    for m in mats:
        mesh.materials.append(m)
    for poly, slot in zip(mesh.polygons, slots):
        poly.material_index = slot
    return o


def initialize():
    global scene, soil, soil_target, turf_target, turf_bed, blades
    if bpy.data.scenes.get('Ground texture authoring'):
        raise RuntimeError('Ground authoring already exists; reuse it instead of overwriting.')
    scene = bpy.data.scenes.new('Ground texture authoring')
    bpy.context.window.scene = scene
    scene.render.engine = 'CYCLES'
    scene.cycles.samples = 8
    scene.cycles.use_denoising = False
    scene.render.bake.margin = 0
    scene.view_settings.view_transform = 'Standard'
    soil = make_soil()
    soil_target = plane('Soil bake tile - 1 metre', 0, 1, soil)
    receiver = bpy.data.materials.new('Turf bake receiver')
    receiver.use_nodes = True
    turf_target = plane('Turf bake tile - 1 metre', -0.005, 1, receiver)
    turf_bed = plane('Turf earth beneath leaves', 0, 1.2, soil)
    blades = make_grass()
    DEST.mkdir(parents=True, exist_ok=True)
    SOURCE.mkdir(parents=True, exist_ok=True)


def bind_existing():
    global scene, soil, soil_target, turf_target, turf_bed, blades
    scene = bpy.data.scenes['Ground texture authoring']
    bpy.context.window.scene = scene
    soil_target = scene.objects['Soil bake tile - 1 metre']
    turf_target = scene.objects['Turf bake tile - 1 metre']
    turf_bed = scene.objects['Turf earth beneath leaves']
    blades = scene.objects['Turf source - wrapped leaves']
    soil = soil_target.data.materials[0]


def bake_map(kind, channel):
    if kind == 'Soil' and scene.objects.get('Rocky soil bake tile - 2 metres'):
        raise RuntimeError('The rocky soil revision owns the Soil exports. Use rocky_soil.py bake() to preserve it.')
    target = soil_target if kind == 'Soil' else turf_target
    mats = [soil] if kind == 'Soil' else [soil, *blades.data.materials]
    for m in mats:
        nodes = m.node_tree.nodes
        p, output, emit = nodes['Surface'], nodes['Output'], nodes['Bake emission']
        if channel == 'Normal':
            wire(m, p.outputs[0], output.inputs['Surface'])
        else:
            socket = p.inputs['Base Color' if channel == 'Albedo' else 'Roughness']
            source = socket.links[0].from_socket if socket.is_linked else socket.default_value
            if not socket.is_linked and channel == 'Roughness':
                source = (source, source, source, 1)
            elif not socket.is_linked:
                source = tuple(source)
            wire(m, source, emit.inputs['Color'])
            wire(m, emit.outputs[0], output.inputs['Surface'])
    bpy.ops.object.select_all(action='DESELECT')
    for o in (soil_target, turf_target, turf_bed, blades):
        o.hide_render = True
    target.hide_render = False
    target.select_set(True)
    if kind == 'Turf':
        for o in (turf_bed, blades):
            o.hide_render = False
            o.select_set(True)
    bpy.context.view_layer.objects.active = target
    material = target.data.materials[0]
    texture_name = kind+'_'+channel+' bake target'
    texture = material.node_tree.nodes.get(texture_name)
    if texture is None:
        texture = node(material, 'ShaderNodeTexImage', texture_name)
    image = texture.image or bpy.data.images.new(kind+'_'+channel, SIZE, SIZE, alpha=False)
    image.colorspace_settings.name = 'sRGB' if channel == 'Albedo' else 'Non-Color'
    texture.image = image
    material.node_tree.nodes.active = texture
    bpy.ops.object.bake(type='NORMAL' if channel == 'Normal' else 'EMIT', use_selected_to_active=kind=='Turf', cage_extrusion=0.08, max_ray_distance=0.15, margin=0)
    image.filepath_raw = str(DEST / (kind+'_'+channel+'.png'))
    image.file_format = 'PNG'
    image.save()
    for m in mats:
        wire(m, m.node_tree.nodes['Surface'].outputs[0], m.node_tree.nodes['Output'].inputs[0])
    return {'file': image.filepath_raw, 'width': image.size[0], 'height': image.size[1]}


def save_source():
    # Pack every export and retain all editable procedural nodes/blade geometry.
    for img in bpy.data.images:
        if img.name.startswith(('Soil_', 'Turf_')):
            img.pack()
    materials = {slot.material for obj in scene.objects for slot in obj.material_slots if slot.material and slot.material.use_nodes}
    for material in materials:
        depths = {}
        def depth(n):
            if n.name in depths:
                return depths[n.name]
            depths[n.name] = 0
            inputs = [link.from_node for socket in n.inputs for link in socket.links]
            depths[n.name] = 1 + max((depth(parent) for parent in inputs), default=-1)
            return depths[n.name]
        rows = {}
        for n in material.node_tree.nodes:
            column = depth(n)
            row = rows.get(column, 0)
            n.location = (column*260, -row*250)
            n.width = 220
            rows[column] = row+1
    sources = {scene}
    for filename in ('create_ground.py', 'rocky_soil.py', 'LICENSE.txt', 'README.md'):
        text = bpy.data.texts.get(filename) or bpy.data.texts.new(filename)
        text.clear()
        text.write((SOURCE/filename).read_text(encoding='utf-8'))
        sources.add(text)
    bpy.data.libraries.write(str(SOURCE/'GroundTextures.blend'), sources, fake_user=True, compress=True)
    return str(SOURCE/'GroundTextures.blend')


if __name__ == '__main__':
    if bpy.data.scenes.get('Ground texture authoring'):
        bind_existing()
    else:
        initialize()
