"""Sunny r4 embedded geology, authored through Blender MCP; retains all trials.

Broad irregular fragments, broken chips and mineral grit share the approved
warm soil matrix. Nothing from reference screenshots is sampled or imported.
"""
import bpy
import math
import random
import types
from mathutils import Vector, Euler
from pathlib import Path

SOURCE = Path(r'C:/Users/pavel/Desktop/Dev/CompanyProjects/game-test/art/ground-textures')
VARIANT = 'A-Sunny-r4'


def bind():
    global r, d, c, g, scene
    r = types.ModuleType('living_ground')
    exec(compile((SOURCE/'living_ground.py').read_text(), 'living_ground.py', 'exec'), r.__dict__)
    r.bind()
    d,c,g,scene = r.d,r.c,r.g,r.scene
    r.VARIANT = d.VARIANT = VARIANT


def material(index=-1):
    m = r.soil_material(index)
    p = m.node_tree.nodes['Surface']
    coords = g.periodic_coords(m)
    # Sparse centimeter-scale mineral inclusions survive 1024px mip filtering.
    noise = g.noise(m, coords, 43, 2, 'Scattered mineral particles')
    color = p.inputs['Base Color'].links[0].from_socket
    grit = g.node(m, 'ShaderNodeMixRGB', 'Small pale mineral grains')
    g.wire(m, g.calc(m, 'MULTIPLY', g.calc(m, 'GREATER_THAN', noise, .74), .6), grit.inputs[0])
    g.wire(m, color, grit.inputs[1]); grit.inputs[2].default_value = (.49,.38,.23,1)
    pores = g.node(m, 'ShaderNodeMixRGB', 'Small dark pores')
    g.wire(m, g.calc(m, 'MULTIPLY', g.calc(m, 'LESS_THAN', noise, .27), .4), pores.inputs[0])
    g.wire(m, grit.outputs[0], pores.inputs[1]); pores.inputs[2].default_value = (.037,.023,.013,1)
    d.assign(m, pores.outputs[0], p.inputs['Base Color'])
    if index >= 0:
        # Slate gray, weathered ochre, rust and pale sandstone; close values
        # remain earthy but are no longer one uniformly brown-gray population.
        palette = [(.085,.092,.085), (.17,.146,.113), (.24,.205,.145),
                   (.13,.103,.081), (.205,.125,.065), (.205,.207,.174),
                   (.072,.073,.065), (.19,.183,.149)]
        base = palette[index % len(palette)]
        ramp = m.node_tree.nodes['Distinct embedded stone faces'].color_ramp
        ramp.elements[0].color = tuple(v*.7 for v in base)+(1,)
        ramp.elements[-1].color = tuple(v*1.3 for v in base)+(1,)
        ramp.elements[0].position = .32; ramp.elements[-1].position = .68
        mottling = g.noise(m, coords, 7, 3, 'Weathered mineral patches')
        d.assign(m, mottling, m.node_tree.nodes['Distinct embedded stone faces'].inputs[0])
        coat = m.node_tree.nodes['Soil coated edges']
        weathering = g.calc(m, 'ADD', .14, g.calc(m, 'MULTIPLY', coat.inputs[0].links[0].from_socket, .86))
        d.assign(m, weathering, coat.inputs[0])
        m.node_tree.nodes['Aggregate relief'].inputs['Distance'].default_value = .0018
    return m


def initialize():
    bind()
    if scene.objects.get(VARIANT+' Soil source'):
        raise RuntimeError('Existing r4 is retained; bind and bake instead.')
    c.plane(VARIANT+' Soil source', 0, material())
    receiver = bpy.data.materials.new(VARIANT+' Soil receiver'); receiver.use_nodes = True
    c.plane(VARIANT+' Soil target', -.025, receiver)
    bpy.context.window.scene = scene
    bpy.ops.object.select_all(action='DESELECT')
    bpy.ops.mesh.primitive_ico_sphere_add(subdivisions=2, radius=1)
    template = bpy.context.object; mesh = template.data
    base = [v.co.copy() for v in mesh.vertices]
    triangles = [tuple(f.vertices) for f in mesh.polygons]
    bpy.data.objects.remove(template, do_unlink=True); bpy.data.meshes.remove(mesh)
    rng = random.Random(105904)
    vertices,faces,slots,placed = [],[],[],[]
    # Diameters before burial: 10–23cm fragments, 2–9cm gravel, 3–18mm chips.
    # Large crowns have independent squash, burial and erosion, not scaled dots.
    groups = ((19,.05,.115), (74,.012,.044), (350,.0018,.009))
    for count,lo,hi in groups:
        for _ in range(count):
            radius = rng.uniform(lo,hi)
            x,y = rng.random()*2,rng.random()*2
            if lo > .01:
                for attempt in range(80):
                    if all(min(abs(x-a),2-abs(x-a))**2 + min(abs(y-b),2-abs(y-b))**2 > (radius+s+.018)**2 for a,b,s in placed): break
                    x,y = rng.random()*2,rng.random()*2
                placed.append((x,y,radius*.8))
            scale = Vector((radius*rng.uniform(.8,1.5), radius*rng.uniform(.58,1.1), min(.043,radius*rng.uniform(.28,.52))))
            rot = Euler((rng.uniform(-.12,.12),rng.uniform(-.12,.12),rng.random()*math.tau)).to_matrix()
            burial = scale.z*rng.uniform(.34,.73)
            points = []
            shear = rng.uniform(-.3,.3)
            for v in base:
                wobble = rng.uniform(.83,1.15)
                a = rot@Vector(((v.x+v.z*shear)*scale.x*wobble,v.y*scale.y*wobble,v.z*scale.z*rng.uniform(.9,1.08)))
                points.append((x+a.x,y+a.y,a.z-burial))
            oxs=[0]+([2] if min(v[0] for v in points)<.022 else [])+([-2] if max(v[0] for v in points)>1.978 else [])
            oys=[0]+([2] if min(v[1] for v in points)<.022 else [])+([-2] if max(v[1] for v in points)>1.978 else [])
            slot = rng.randrange(8)
            for ox in oxs:
                for oy in oys:
                    offset=len(vertices)
                    vertices.extend((a+ox,b+oy,z) for a,b,z in points)
                    faces.extend(tuple(offset+i for i in f) for f in triangles)
                    slots.extend([slot]*len(triangles))
    mesh=bpy.data.meshes.new(VARIANT+' buried geological fragments')
    mesh.from_pydata(vertices,[],faces);mesh.update()
    uv=mesh.uv_layers.new(name='Periodic planar mineral detail')
    for face in mesh.polygons:
        for li in face.loop_indices:
            v=mesh.vertices[mesh.loops[li].vertex_index].co
            uv.data[li].uv=(v.x/2,v.y/2)
    for i in range(8):mesh.materials.append(material(i))
    assign_palettes(mesh)
    obj=bpy.data.objects.new(VARIANT+' Soil relief',mesh);scene.collection.objects.link(obj)
    helper=types.ModuleType('rocky_soil')
    exec(compile((SOURCE/'rocky_soil.py').read_text(),'rocky_soil.py','exec'),helper.__dict__)
    helper.weather_stone_normals(mesh)
    scene.cycles.samples=16
    return {'variant':VARIANT,'fragments':443,'palettes':8,'vertices':len(vertices),'resolution':1024}


def assign_palettes(mesh):
    # Stable per-fragment identities survive material-slot replacement. Wrapped
    # copies receive the same mineral family at both sides of the seamless tile.
    for start in range(0, len(mesh.vertices), 42):
        center = sum((v.co for v in mesh.vertices[start:start+42]), Vector()) / 42
        seed = (round((center.x % 2)*10000)*73856093) ^ (round((center.y % 2)*10000)*19349663)
        slot = random.Random(seed).randrange(8)
        for face in mesh.polygons[(start//42)*80:(start//42+1)*80]: face.material_index = slot


def bake(channel):
    bind();d.bind=lambda:None;d.SIZE=1024
    return d.bake('Soil',channel)


def save_source():
    bind()
    return c.save_source()
