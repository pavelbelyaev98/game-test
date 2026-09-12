import bpy, math, random, os
from mathutils import Vector

# Original approved grass authoring: separate scene; preserves existing content.
root = os.path.dirname(os.path.abspath(__file__))
os.makedirs(root, exist_ok=True)
if bpy.data.objects.get('GrassClump') or bpy.data.meshes.get('GrassClump'):
    raise RuntimeError('Generate in a fresh Blender file to preserve existing authored grass; export edited source meshes directly from GroundGrass.blend.')
scene = bpy.data.scenes.new('Sunny Grass Review 127')
bpy.context.window.scene = scene
scene.unit_settings.system = 'METRIC'
scene.render.engine = 'CYCLES'
scene.cycles.samples = 16
scene.world = bpy.data.worlds.new('Grass Review Sky')
scene.world.use_nodes = True
scene.world.node_tree.nodes['Background'].inputs['Color'].default_value = (0.67, 0.79, 1, 1)
scene.world.node_tree.nodes['Background'].inputs['Strength'].default_value = 0.65

mat = bpy.data.materials.new('Sunny Grass Authored')
mat.use_nodes = True
n, l = mat.node_tree.nodes, mat.node_tree.links
bsdf = n.get('Principled BSDF')
bsdf.inputs['Roughness'].default_value = 0.86
uv = n.new('ShaderNodeTexCoord')
xy = n.new('ShaderNodeSeparateXYZ'); l.new(uv.outputs['UV'], xy.inputs[0])
ramp = n.new('ShaderNodeValToRGB'); l.new(xy.outputs['Y'], ramp.inputs[0])
ramp.color_ramp.elements[0].color = (0.055, 0.14, 0.009, 1)
ramp.color_ramp.elements[1].color = (0.38, 0.59, 0.065, 1)
mid = ramp.color_ramp.elements.new(0.42); mid.color = (0.15, 0.35, 0.018, 1)
l.new(ramp.outputs['Color'], bsdf.inputs['Base Color'])

def clump(name, count):
    rng = random.Random(127)
    verts, faces, uvs, weights = [], [], [], []
    for blade in range(count):
        angle = blade * 2.399963 + rng.uniform(-0.3, 0.3)
        base = Vector((math.cos(angle), math.sin(angle), 0)) * rng.uniform(0.015, 0.075)
        forward = Vector((math.cos(angle), math.sin(angle), 0))
        side = Vector((-math.sin(angle), math.cos(angle), 0))
        height = rng.uniform(0.11, 0.235)
        width = rng.uniform(0.017, 0.03)
        bend = rng.uniform(0.025, 0.065)
        start = len(verts)
        for row, t in enumerate((0, 0.23, 0.5, 0.76, 1)):
            spread = (0.56 + 0.75 * math.sin(math.pi * t)) * (1 - t) * width
            center = base + forward * (bend * t * t) + Vector((0, 0, height * t))
            for col in range(3):
                p = center + side * ((col - 1) * spread / 2)
                if col == 1: p += forward * (-0.0035 * math.sin(t * math.pi))
                verts.append(tuple(p)); uvs.append((col / 2, t)); weights.append(t * t)
            if row:
                a = start + (row - 1) * 3; b = start + row * 3
                if row == 4:
                    faces.extend([(a, a+1, b+1), (a+1, a+2, b+1)])
                else:
                    faces.extend([(a, a+1, b+1, b), (a+1, a+2, b+2, b+1)])
    mesh = bpy.data.meshes.new(name); mesh.from_pydata(verts, [], faces); mesh.update()
    layer = mesh.uv_layers.new(name='UVMap')
    color = mesh.color_attributes.new(name='WindWeights', type='FLOAT_COLOR', domain='POINT')
    for i, w in enumerate(weights): color.data[i].color = (w, 1, 1, 1)
    for poly in mesh.polygons:
        for li in poly.loop_indices: layer.data[li].uv = uvs[mesh.loops[li].vertex_index]
    obj = bpy.data.objects.new(name, mesh); scene.collection.objects.link(obj); mesh.materials.append(mat)
    return obj

near = clump('GrassClump', 12)
far = clump('GrassClump_LOD1', 6); far.hide_render = True; far.hide_viewport = True

# Bake the authored root-to-tip color into an exportable atlas.
atlas = bpy.data.images.new('Grass_Albedo', width=128, height=256, alpha=False)
tex = n.new('ShaderNodeTexImage'); tex.image = atlas; n.active = tex
near.select_set(True); bpy.context.view_layer.objects.active = near
scene.render.bake.use_pass_direct = False; scene.render.bake.use_pass_indirect = False
scene.render.bake.use_pass_color = True; scene.render.bake.margin = 8
bpy.ops.object.bake(type='DIFFUSE')
atlas.filepath_raw = root + '/Grass_Albedo.png'; atlas.file_format = 'PNG'; atlas.save(); atlas.pack()

# Export only the proposed blade meshes, preserving vertex wind weights.
for obj in bpy.context.selected_objects: obj.select_set(False)
near.select_set(True); far.hide_viewport = False; far.select_set(True)
bpy.context.view_layer.objects.active = near
bpy.ops.export_scene.fbx(filepath=root+'/GrassClumps.fbx', use_selection=True, object_types={'MESH'},
    axis_forward='-Z', axis_up='Y', apply_unit_scale=True, bake_space_transform=True,
    add_leaf_bones=False, bake_anim=False, use_mesh_modifiers=True, mesh_smooth_type='FACE')
far.hide_viewport = True

# A 1.6 m patch over a preview-only plane using the accepted turf map.
for obj in bpy.context.selected_objects: obj.select_set(False)
turf = bpy.data.materials.new('Accepted Sunny Turf - preview only'); turf.use_nodes=True
tn = turf.node_tree.nodes; tt = tn.new('ShaderNodeTexImage')
tt.image = bpy.data.images.load(os.path.join(root, '../../unity/Assets/Content/GroundTextures/Turf_Albedo.png'), check_existing=True)
turf.node_tree.links.new(tt.outputs['Color'], tn.get('Principled BSDF').inputs['Base Color'])
tn.get('Principled BSDF').inputs['Roughness'].default_value=0.9
bpy.ops.mesh.primitive_plane_add(size=1.8); plane=bpy.context.object; plane.name='Review Ground Only'; plane.data.materials.append(turf)
near.hide_render=True
rng=random.Random(74)
for y in range(12):
    for x in range(12):
        ob=bpy.data.objects.new('Review instance', near.data); scene.collection.objects.link(ob)
        ob.location=((x-5.5)*0.13+rng.uniform(-0.04,0.04), (y-5.5)*0.13+rng.uniform(-0.04,0.04),0)
        ob.rotation_euler.z=rng.uniform(0, math.tau); s=rng.uniform(0.8,1.1); ob.scale=(s,s,s)
bpy.ops.object.light_add(type='AREA', location=(-2,-3,5)); sun=bpy.context.object
sun.data.energy=450; sun.data.shape='DISK'; sun.data.size=4
bpy.ops.object.camera_add(location=(1.7,-2.5,1.55)); camera=bpy.context.object
camera.rotation_euler=(Vector((0,0,0.07))-camera.location).to_track_quat('-Z','Y').to_euler()
camera.data.type='ORTHO'; camera.data.ortho_scale=2.1; scene.camera=camera
scene.render.resolution_x=1000; scene.render.resolution_y=800; scene.render.resolution_percentage=100
scene.view_settings.view_transform='AgX'
scene.render.image_settings.file_format='PNG'; scene.render.filepath=root+'/preview.png'
bpy.data.libraries.write(root+'/GroundGrass.blend', {scene}, fake_user=True)
bpy.ops.render.render(write_still=True)
result={'preview':root+'/preview.png','source':root+'/GroundGrass.blend','near_triangles':sum(len(p.vertices)-2 for p in near.data.polygons),'far_triangles':sum(len(p.vertices)-2 for p in far.data.polygons)}
