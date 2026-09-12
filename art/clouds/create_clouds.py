import bpy, math, random, os, json
from mathutils import Vector

root = os.path.dirname(os.path.abspath(__file__))
os.makedirs(root, exist_ok=True)
name = 'Sunny Clouds Authoring 130'
if bpy.data.scenes.get(name):
    raise RuntimeError('Cloud source already exists; refine that source instead of duplicating it.')
previous = bpy.context.window.scene
scene = bpy.data.scenes.new(name)
bpy.context.window.scene = scene
try:
    scene.render.engine = 'CYCLES'; scene.cycles.samples = 48
    scene.render.film_transparent = True
    scene.world = bpy.data.worlds.new('Cloud130 Sky Fill'); scene.world.use_nodes = True
    scene.world.node_tree.nodes['Background'].inputs['Color'].default_value = (.48,.67,.79,1)
    scene.world.node_tree.nodes['Background'].inputs['Strength'].default_value = .6
    scene.view_settings.view_transform = 'Standard'
    material = bpy.data.materials.new('Cloud130 Soft Water Vapour'); material.use_nodes = True
    bsdf = material.node_tree.nodes['Principled BSDF']
    bsdf.inputs['Base Color'].default_value = (.92,.94,.91,1)
    bsdf.inputs['Roughness'].default_value = .95
    bsdf.inputs['Subsurface Weight'].default_value = .12
    clouds=[]
    for variant in range(4):
        rng=random.Random(130+variant)
        pieces=[]
        width=(1,.82,1.08,.93)[variant]
        for i in range(9):
            x=(i-4)*.58*width
            rise=math.sin((i/8)*math.pi)
            y=-.2+rise*(.45+rng.random()*.55)
            bpy.ops.mesh.primitive_uv_sphere_add(segments=24,ring_count=16,
                location=(x,y,rng.uniform(-.25,.2)))
            obj=bpy.context.object;obj.name=f'Cloud130 lobe {variant}-{i}'
            obj.scale=(rng.uniform(.8,1.15)*width,rng.uniform(.55,.83)+rise*.3,rng.uniform(.7,1.1))
            bpy.ops.object.transform_apply(location=False,rotation=False,scale=True)
            pieces.append(obj)
        bpy.ops.mesh.primitive_uv_sphere_add(segments=32,ring_count=20,location=(0,-.45,0))
        base=bpy.context.object;base.name=f'Cloud130 base {variant}'
        base.scale=(2.75*width,.6,.8)
        bpy.ops.object.transform_apply(location=False,rotation=False,scale=True)
        pieces.append(base)
        for obj in bpy.context.selected_objects:obj.select_set(False)
        for obj in pieces:obj.select_set(True)
        bpy.context.view_layer.objects.active=pieces[0]
        bpy.ops.object.join();cloud=bpy.context.object;cloud.name=f'Cloud130 Shape {variant+1}'
        remesh=cloud.modifiers.new('Merge soft lobes','REMESH');remesh.mode='VOXEL';remesh.voxel_size=.12
        bpy.ops.object.modifier_apply(modifier=remesh.name)
        smooth=cloud.modifiers.new('Soften lobe joins','SMOOTH');smooth.factor=1.1;smooth.iterations=4
        bpy.ops.object.modifier_apply(modifier=smooth.name)
        for poly in cloud.data.polygons:poly.use_smooth=True
        cloud.data.materials.append(material)
        # The joined origin remains at the first lobe; place its geometry by offset.
        cloud.location.x += -5 if variant%2==0 else 5
        cloud.location.y += -5 if variant<2 else 5
        clouds.append(cloud)
    bpy.ops.object.light_add(type='AREA',location=(-7,10,14))
    lamp=bpy.context.object;lamp.name='Cloud130 Soft Sun';lamp.data.energy=2200;lamp.data.size=8
    lamp.rotation_euler=(-lamp.location).to_track_quat('-Z','Y').to_euler()
    bpy.ops.object.camera_add(location=(0,0,30))
    camera=bpy.context.object;camera.name='Cloud130 Atlas Camera';camera.data.type='ORTHO';camera.data.ortho_scale=20
    camera.rotation_euler=(0,0,0);scene.camera=camera
    scene.render.resolution_x=scene.render.resolution_y=1024;scene.render.resolution_percentage=100
    scene.render.image_settings.file_format='PNG';scene.render.image_settings.color_mode='RGBA'
    scene.render.filepath=os.path.join(root,'CloudAtlas.png')
    bpy.ops.render.render(write_still=True)
    text=bpy.data.texts.new('Cloud130_create_clouds.py')
    with open(__file__,encoding='utf-8') as recipe:text.write(recipe.read())
    bpy.data.libraries.write(os.path.join(root,'Clouds.blend'),{scene,text},fake_user=True)
    scene.render.film_transparent=False;scene.render.filepath=os.path.join(root,'preview.png')
    bpy.ops.render.render(write_still=True)
    print(json.dumps({'source':os.path.join(root,'Clouds.blend'),'atlas':os.path.join(root,'CloudAtlas.png'),'shapes':len(clouds)}))
finally:
    bpy.context.window.scene=previous
