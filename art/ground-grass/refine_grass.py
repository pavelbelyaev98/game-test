import bpy, math, os, random, json
from mathutils import Vector

# Task 129: refine the approved clump in its existing source; preserve topology,
# object/mesh names, original blade roots and all unrelated Blender scenes.
root = os.path.dirname(os.path.abspath(__file__))
near = bpy.data.objects.get('GrassClump')
far = bpy.data.objects.get('GrassClump_LOD1')
if near is None or far is None:
    raise RuntimeError('Open GroundGrass.blend, or run create_grass.py first.')
scene = near.users_scene[0]
previous_scene = bpy.context.window.scene
bpy.context.window.scene = scene
try:
    mat = near.data.materials[0]
    nodes, links = mat.node_tree.nodes, mat.node_tree.links
    ramp = nodes.get('Color Ramp')
    palette = ((0, (0.05, 0.13, 0.01, 1)),
               (0.42, (0.14, 0.32, 0.026, 1)),
               (1, (0.29, 0.49, 0.075, 1)))
    for element, (position, color) in zip(ramp.color_ramp.elements, palette):
        element.position = position
        element.color = color
    variation = nodes.get('Blade palette') or nodes.new('ShaderNodeValToRGB')
    variation.name = 'Blade palette'
    variation.color_ramp.elements[0].color = (0.79, 0.94, 0.76, 1)
    variation.color_ramp.elements[-1].color = (1, 0.95, 0.86, 1)
    if len(variation.color_ramp.elements) == 2:
        variation.color_ramp.elements.new(0.48)
    variation.color_ramp.elements[1].color = (0.86, 1, 1, 1)
    mix = nodes.get('Blade color') or nodes.new('ShaderNodeMixRGB')
    mix.name = 'Blade color'; mix.blend_type = 'MULTIPLY'; mix.inputs[0].default_value = 1
    links.new(nodes['Separate XYZ'].outputs['X'], variation.inputs[0])
    links.new(ramp.outputs['Color'], mix.inputs[1])
    links.new(variation.outputs['Color'], mix.inputs[2])
    links.new(mix.outputs[0], nodes['Principled BSDF'].inputs['Base Color'])

    for obj in (near, far):
        mesh = obj.data
        count = len(mesh.vertices) // 15
        assert len(mesh.vertices) == count * 15
        uv = mesh.uv_layers['UVMap']
        motion = mesh.uv_layers.get('BladeMotion') or mesh.uv_layers.new(name='BladeMotion')
        uv_values, motion_values = [], []
        rng = random.Random(129)
        for blade in range(count):
            start = blade * 15
            base = sum((mesh.vertices[start + c].co.copy() for c in range(3)), Vector()) / 3
            # Fixed orientation from the original root locations avoids compounded edits.
            outward = Vector((base.x, base.y, 0)).normalized()
            side = Vector((-outward.y, outward.x, 0))
            height = rng.uniform(0.255, 0.435)
            width = rng.uniform(0.028, 0.043)
            bend = rng.uniform(0.042, 0.1)
            side_bend = rng.uniform(-0.03, 0.03)
            twist = rng.uniform(-0.35, 0.35)
            phase = rng.random()
            for row, t in enumerate((0, 0.23, 0.5, 0.76, 1)):
                spread = width * (0.82 + 0.3 * math.sin(math.pi * t)) * (1 - t ** 1.25)
                center = base + outward * (bend * t * t) + side * (side_bend * t ** 3) + Vector((0, 0, height * t))
                cross = side * math.cos(twist * t * t) + outward * math.sin(twist * t * t)
                for col in range(3):
                    point = center + cross * ((col - 1) * spread / 2)
                    if col == 1:
                        point -= outward * (0.0008 * math.sin(t * math.pi))
                    mesh.vertices[start + row * 3 + col].co = point
                    uv_values.append(((blade + 0.13 + col * 0.37) / 12, t))
                    motion_values.append((phase, height))
        for poly in mesh.polygons:
            poly.use_smooth = True
            for li in poly.loop_indices:
                vertex = mesh.loops[li].vertex_index
                uv.data[li].uv = uv_values[vertex]
                motion.data[li].uv = motion_values[vertex]
        mesh.uv_layers.active = uv
        uv.active_render = True
        mesh.update()

    for obj in bpy.context.selected_objects:
        obj.select_set(False)
    near.hide_render = False
    near.select_set(True)
    bpy.context.view_layer.objects.active = near
    atlas = nodes['Image Texture'].image
    nodes.active = nodes['Image Texture']
    scene.render.bake.use_pass_direct = False
    scene.render.bake.use_pass_indirect = False
    scene.render.bake.use_pass_color = True
    scene.render.bake.margin = 2
    bpy.ops.object.bake(type='DIFFUSE')
    atlas.filepath_raw = os.path.join(root, 'Grass_Albedo.png')
    atlas.file_format = 'PNG'; atlas.save(); atlas.pack()

    far.hide_viewport = False
    far.select_set(True)
    bpy.ops.export_scene.fbx(filepath=os.path.join(root, 'GrassClumps.fbx'),
        use_selection=True, object_types={'MESH'}, axis_forward='-Z', axis_up='Y',
        apply_unit_scale=True, bake_space_transform=True, add_leaf_bones=False,
        bake_anim=False, use_mesh_modifiers=True, mesh_smooth_type='FACE')
    near.hide_render = True; far.hide_viewport = True
    recipe = bpy.data.texts.get('Grass129_refine_grass.py') or bpy.data.texts.new('Grass129_refine_grass.py')
    recipe.clear()
    with open(__file__, encoding='utf-8') as source:
        recipe.write(source.read())
    bpy.data.libraries.write(os.path.join(root, 'GroundGrass.blend'), {scene, recipe}, fake_user=True)
    scene.render.filepath = os.path.join(root, 'preview.png')
    bpy.ops.render.render(write_still=True)
    print(json.dumps({'height': near.dimensions.z,
        'triangles': sum(len(p.vertices)-2 for p in near.data.polygons),
        'uv_layers': [u.name for u in near.data.uv_layers], 'source': os.path.join(root,'GroundGrass.blend')}))
finally:
    bpy.context.window.scene = previous_scene
