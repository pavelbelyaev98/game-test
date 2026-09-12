import bpy, os
root = os.path.dirname(os.path.abspath(__file__))
os.makedirs(root, exist_ok=True)
original = bpy.context.window.scene
scene = bpy.data.scenes.new('Sunny Sun Authoring 128')
bpy.context.window.scene = scene
scene.render.engine = 'CYCLES'; scene.cycles.samples = 32
scene.world = bpy.data.worlds.new('Sun review blue'); scene.world.use_nodes=True
scene.world.node_tree.nodes['Background'].inputs['Color'].default_value=(.196,.407,.587,1)
scene.world.node_tree.nodes['Background'].inputs['Strength'].default_value=1
scene.render.film_transparent = True
scene.view_settings.view_transform='Standard'
scene.render.resolution_x=512;scene.render.resolution_y=512;scene.render.resolution_percentage=100
scene.render.image_settings.file_format='PNG';scene.render.image_settings.color_mode='RGBA'
mat=bpy.data.materials.new('Warm cartoon sun and restrained halo');mat.use_nodes=True
n=mat.node_tree.nodes;l=mat.node_tree.links;n.clear()
uv=n.new('ShaderNodeTexCoord');sub=n.new('ShaderNodeVectorMath');sub.operation='SUBTRACT';sub.inputs[1].default_value=(.5,.5,0);l.new(uv.outputs['UV'],sub.inputs[0])
length=n.new('ShaderNodeVectorMath');length.operation='LENGTH';l.new(sub.outputs['Vector'],length.inputs[0])
color=n.new('ShaderNodeValToRGB');l.new(length.outputs['Value'],color.inputs[0])
color.color_ramp.elements.remove(color.color_ramp.elements[1])
for i,(pos,rgba) in enumerate([(0,(1,.95,.64,1)),(.285,(1,.88,.40,1)),(.305,(1,.76,.20,1)),(.44,(1,.70,.15,1))]):
 e=color.color_ramp.elements[0] if i==0 else color.color_ramp.elements.new(pos);e.position=pos;e.color=rgba
alpha=n.new('ShaderNodeValToRGB');l.new(length.outputs['Value'],alpha.inputs[0]);alpha.color_ramp.elements.remove(alpha.color_ramp.elements[1])
for i,(pos,a) in enumerate([(0,1),(.293,1),(.303,.16),(.345,.05),(.43,0),(.49,0)]):
 e=alpha.color_ramp.elements[0] if i==0 else alpha.color_ramp.elements.new(pos);e.position=pos;e.color=(a,a,a,1)
em=n.new('ShaderNodeEmission');l.new(color.outputs['Color'],em.inputs['Color'])
transparent=n.new('ShaderNodeBsdfTransparent');mix=n.new('ShaderNodeMixShader');l.new(alpha.outputs['Color'],mix.inputs[0]);l.new(transparent.outputs[0],mix.inputs[1]);l.new(em.outputs[0],mix.inputs[2]);out=n.new('ShaderNodeOutputMaterial');l.new(mix.outputs[0],out.inputs['Surface'])
bpy.ops.mesh.primitive_plane_add(size=2);plane=bpy.context.object;plane.name='Authored sun disc receiver';plane.data.materials.append(mat)
bpy.ops.object.camera_add(location=(0,0,4));camera=bpy.context.object;camera.rotation_euler=(0,0,0);camera.data.type='ORTHO';camera.data.ortho_scale=2;scene.camera=camera
scene.render.filepath=root+'/Sun_Disc.png';bpy.ops.render.render(write_still=True)
scene.render.film_transparent=False;camera.data.ortho_scale=6
scene.render.resolution_x=960;scene.render.resolution_y=540
scene.render.filepath=root+'/preview.png';bpy.ops.render.render(write_still=True)
bpy.data.libraries.write(root+'/Sun.blend',{scene},fake_user=True)
bpy.context.window.scene=original
result={'texture':root+'/Sun_Disc.png','preview':root+'/preview.png','source':root+'/Sun.blend','original_scene':original.name}
