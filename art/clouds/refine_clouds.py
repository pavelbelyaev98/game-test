"""Task 132: refine the approved source through Blender MCP; retain editable modifiers."""
import bpy, os, json

root = os.path.dirname(os.path.abspath(__file__))
scene = bpy.data.scenes.get('Sunny Clouds Authoring 130')
if scene is None:
    with bpy.data.libraries.load(os.path.join(root, 'Clouds.blend')) as (source, target):
        target.scenes = ['Sunny Clouds Authoring 130']
    scene = target.scenes[0]
previous = bpy.context.window.scene
bpy.context.window.scene = scene
try:
    material = bpy.data.materials.get('Cloud132 Pale Cumulus') or bpy.data.materials.new('Cloud132 Pale Cumulus')
    material.use_nodes = True
    nodes = material.node_tree.nodes
    nodes.clear()
    output = nodes.new('ShaderNodeOutputMaterial')
    emission = nodes.new('ShaderNodeEmission')
    coordinates = nodes.new('ShaderNodeTexCoord')
    separate = nodes.new('ShaderNodeSeparateXYZ')
    ramp = nodes.new('ShaderNodeValToRGB')
    ramp.color_ramp.elements[0].color = (.68, .88, .86, 1)
    ramp.color_ramp.elements[1].color = (.85, .97, .92, 1)
    links = material.node_tree.links
    links.new(coordinates.outputs['Generated'], separate.inputs[0])
    links.new(separate.outputs['Y'], ramp.inputs[0])
    links.new(ramp.outputs['Color'], emission.inputs['Color'])
    links.new(emission.outputs[0], output.inputs['Surface'])
    for variant, cloud in enumerate(sorted([o for o in scene.objects if o.type == 'MESH'], key=lambda o: o.name)):
        # Rework the existing forms into a clear cumulus family: a broad base,
        # one tall crown and two smaller shoulders. Keep the original object names.
        pieces = []
        crown = (1.55, 1.75, 1.45, 1.6)[variant]
        mirror = -1 if variant & 1 else 1
        lobes = [((-1.9*mirror, -.05, 0), (1.05, .85, .8)),
                 ((-.65*mirror, .45, 0), (1.35, crown, .95)),
                 ((1.15*mirror, .12, 0), (1.65, 1.1, .85)),
                 ((0, -.55, 0), (2.6, .55, .75))]
        for position, scale in lobes:
            bpy.ops.mesh.primitive_uv_sphere_add(segments=24, ring_count=16, location=position)
            piece = bpy.context.object
            piece.scale = scale
            bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
            pieces.append(piece)
        for obj in bpy.context.selected_objects:
            obj.select_set(False)
        for piece in pieces:
            piece.select_set(True)
        bpy.context.view_layer.objects.active = pieces[0]
        bpy.ops.object.join()
        merged = bpy.context.object
        bpy.ops.object.transform_apply(location=True, rotation=False, scale=False)
        remesh = merged.modifiers.new('Join cumulus lobes', 'REMESH')
        remesh.mode = 'VOXEL'
        remesh.voxel_size = .1
        bpy.ops.object.modifier_apply(modifier=remesh.name)
        smooth = merged.modifiers.new('Round lobe joins', 'SMOOTH')
        smooth.factor = .7
        smooth.iterations = 2
        bpy.ops.object.modifier_apply(modifier=smooth.name)
        cloud.data = merged.data
        cloud.location = (-5 if variant % 2 == 0 else 5, -5 if variant < 2 else 5, 0)
        bpy.data.objects.remove(merged, do_unlink=True)
        cloud.data.materials.clear()
        cloud.data.materials.append(material)
        modifier = cloud.modifiers.get('Broad cumulus silhouette') or cloud.modifiers.new('Broad cumulus silhouette', 'DECIMATE')
        modifier.ratio = .07
        for polygon in cloud.data.polygons:
            polygon.use_smooth = False
    scene.cycles.samples = 32
    scene.view_settings.view_transform = 'Standard'
    scene.render.film_transparent = True
    scene.render.filepath = os.path.join(root, 'CloudAtlas.png')
    bpy.ops.render.render(write_still=True)
    recipe = bpy.data.texts.get('Cloud132_refine_clouds.py') or bpy.data.texts.new('Cloud132_refine_clouds.py')
    recipe.clear()
    with open(__file__, encoding='utf-8') as source:
        recipe.write(source.read())
    bpy.data.libraries.write(os.path.join(root, 'Clouds.blend'), {scene, recipe}, fake_user=True)
    scene.world.node_tree.nodes['Background'].inputs['Color'].default_value = (.0134, .554, .687, 1)
    scene.world.node_tree.nodes['Background'].inputs['Strength'].default_value = 1
    scene.render.film_transparent = False
    scene.render.filepath = os.path.join(root, 'preview.png')
    bpy.ops.render.render(write_still=True)
    result = {'source': os.path.join(root, 'Clouds.blend'), 'atlas': os.path.join(root, 'CloudAtlas.png'), 'shapes': 4}
finally:
    bpy.context.window.scene = previous
