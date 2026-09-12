"""Refine the approved distant mesh through Blender MCP; retain all twelve blades."""
import bpy, os

root = os.path.dirname(os.path.abspath(__file__))
near = bpy.data.objects.get('GrassClump')
far = bpy.data.objects.get('GrassClump_LOD1')
if near is None or far is None:
    raise RuntimeError('Load the approved GroundGrass.blend before refining its existing meshes.')
scene = near.users_scene[0]
previous = bpy.context.window.scene
bpy.context.window.scene = scene
try:
    source = near.data
    assert len(source.vertices) == 180
    retained = [blade * 15 + row * 3 + column for blade in range(12) for row in (0, 2, 4) for column in range(3)]
    vertices = [source.vertices[i].co.copy() for i in retained]
    normals = [source.vertices[i].normal.copy() for i in retained]
    faces = []
    for blade in range(12):
        a = blade * 9
        faces.extend([(a, a+1, a+4, a+3), (a+1, a+2, a+5, a+4),
                      (a+3, a+4, a+7), (a+4, a+5, a+7)])
    mesh = bpy.data.meshes.new('GrassClump_LOD1_12Blades')
    mesh.from_pydata(vertices, [], faces)
    mesh.update()
    for source_layer in source.uv_layers:
        values = {}
        for loop in source.loops:
            values[loop.vertex_index] = source_layer.data[loop.index].uv.copy()
        layer = mesh.uv_layers.new(name=source_layer.name)
        for loop in mesh.loops:
            layer.data[loop.index].uv = values[retained[loop.vertex_index]]
    for polygon in mesh.polygons:
        polygon.use_smooth = True
    if hasattr(mesh, 'normals_split_custom_set_from_vertices'):
        mesh.normals_split_custom_set_from_vertices(normals)
    for material in source.materials:
        mesh.materials.append(material)
    far.data = mesh
    far.matrix_world = near.matrix_world.copy()
    for obj in bpy.context.selected_objects:
        obj.select_set(False)
    near.select_set(True)
    far.hide_viewport = False
    far.select_set(True)
    bpy.context.view_layer.objects.active = near
    bpy.ops.export_scene.fbx(filepath=os.path.join(root, 'GrassClumps.fbx'),
        use_selection=True, object_types={'MESH'}, axis_forward='-Z', axis_up='Y',
        apply_unit_scale=True, bake_space_transform=True, add_leaf_bones=False,
        bake_anim=False, use_mesh_modifiers=True, mesh_smooth_type='FACE')
    far.hide_viewport = True
    recipe = bpy.data.texts.get('Grass133_optimize_grass.py') or bpy.data.texts.new('Grass133_optimize_grass.py')
    recipe.clear()
    with open(__file__, encoding='utf-8') as text:
        recipe.write(text.read())
    bpy.data.libraries.write(os.path.join(root, 'GroundGrass.blend'), {scene, recipe}, fake_user=True)
    result = {'blades': 12, 'nearTriangles': sum(len(p.vertices)-2 for p in source.polygons),
              'farTriangles': sum(len(p.vertices)-2 for p in mesh.polygons), 'source': os.path.join(root, 'GroundGrass.blend')}
finally:
    bpy.context.window.scene = previous
