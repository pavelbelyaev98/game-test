"""Original excavator source. Execute through the connected Blender MCP instance."""
import bpy, math, json
from pathlib import Path
from mathutils import Vector

out = Path(r'C:/Users/pavel/Desktop/Dev/CompanyProjects/something-down-there/art/earthshaper')
out.mkdir(parents=True, exist_ok=True)
scene = bpy.data.scenes.new('SDT_Excavator')
bpy.context.window.scene = scene
scene.unit_settings.system = 'METRIC'
scene.render.engine = 'CYCLES'
scene.cycles.samples = 16
scene.render.bake.margin = 8

def material(name, color, metal=0):
    m=bpy.data.materials.new('Excavator_'+name); m.use_nodes=True
    n=m.node_tree.nodes; l=m.node_tree.links; p=n.get('Principled BSDF')
    p.inputs['Metallic'].default_value=metal; p.inputs['Roughness'].default_value=.52
    noise=n.new('ShaderNodeTexNoise'); noise.inputs['Scale'].default_value=32; noise.inputs['Detail'].default_value=2
    ramp=n.new('ShaderNodeValToRGB'); ramp.color_ramp.elements[0].position=.18; ramp.color_ramp.elements[1].position=.82
    ramp.color_ramp.elements[0].color=tuple(c*.63 for c in color)+(1,)
    ramp.color_ramp.elements[1].color=tuple(min(1,c*1.13) for c in color)+(1,)
    l.new(noise.outputs['Fac'],ramp.inputs[0]); l.new(ramp.outputs['Color'],p.inputs['Base Color'])
    m.diffuse_color=tuple(color)+(1,); return m

paint=material('teal_enamel',(.065,.39,.36)); yellow=material('ochre_enamel',(.92,.51,.10))
steel=material('brushed_steel',(.30,.38,.42),.6); dark=material('rubber',(.025,.038,.041))
brass=material('copper',(.56,.25,.095),.55); cream=material('ceramic',(.77,.76,.57))
parts={}
def finish(o,name,mat,group='Body',bevel=.015):
    o.name='Excavator_'+name; o.data.materials.append(mat)
    if bevel:
        mod=o.modifiers.new('Machined soft edges','BEVEL'); mod.width=bevel; mod.segments=2
        bpy.context.view_layer.objects.active=o
        bpy.ops.object.modifier_apply(modifier=mod.name)
    for f in o.data.polygons: f.use_smooth=False
    parts.setdefault(group,[]).append(o); return o
def box(name,loc,scale,mat,group='Body',rot=(0,0,0),bevel=.015):
    bpy.ops.mesh.primitive_cube_add(size=1,location=loc,rotation=rot); o=bpy.context.object; o.scale=scale
    bpy.ops.object.transform_apply(location=False,rotation=False,scale=True)
    return finish(o,name,mat,group,bevel)
def cyl(name,loc,radius,depth,mat,group='Body',axis='Y',vertices=20):
    rotation=(math.pi/2,0,0) if axis=='Y' else (0,math.pi/2,0) if axis=='X' else (0,0,0)
    bpy.ops.mesh.primitive_cylinder_add(vertices=vertices,radius=radius,depth=depth,location=loc,rotation=rotation)
    return finish(bpy.context.object,name,mat,group,.006)

# One readable industrial body: rolled housing, battered scoop lip, insulated
# handle, coil feed and exposed hardware. All forms originate in Blender.
box('main_housing',(0,0,.02),(.32,.52,.24),paint,bevel=.05)
box('top_casing',(0,.005,.17),(.26,.38,.085),yellow,bevel=.035)
box('rear_bumper',(0,.26,.035),(.34,.07,.25),dark,bevel=.025)
box('grip',(0,.18,-.19),(.105,.13,.26),dark,rot=(.25,0,0),bevel=.03)
for i in range(5): box('grip_rib'+str(i),(0,.174+i*.007,-.1-i*.042),(.112,.135,.012),steel,bevel=.003)
cyl('coupler',(0,-.31,.04),.155,.13,steel)
cyl('collar',(0,-.385,.04),.176,.052,yellow)
cyl('barrel',(0,-.49,.04),.126,.22,dark)
cyl('muzzle_band',(0,-.60,.04),.145,.04,steel)
cyl('recess',(0,-.625,.04),.122,.014,dark)

# Rotor teeth and central spindle remain a separate animated authored mesh.
cyl('spindle',(0,-.65,.04),.038,.12,brass,'Rotor')
for i in range(8):
    a=i*math.tau/8
    o=box('rotor_tooth'+str(i),(.087*math.cos(a),-.65,.04+.087*math.sin(a)),(.045,.09,.062),steel,'Rotor',rot=(0,-a,0),bevel=.008)

# Split scoop cheeks open in fan mode; the ring never blocks the centre aim.
for side,group in [(-1,'JawLeft'),(1,'JawRight')]:
    box(group+'_blade',(side*.15,-.575,-.085),(.13,.33,.045),yellow,group,rot=(.18,side*.14,side*.05),bevel=.012)
    box(group+'_edge',(side*.15,-.744,-.104),(.14,.055,.028),steel,group,bevel=.006)
    cyl(group+'_hinge',(side*.174,-.375,-.035),.045,.035,brass,group,axis='X')

for side in [-1,1]:
    for y in [-.18,.16]:
        cyl('housing_bolt',(side*.168,y,.04),.022,.018,steel,axis='X',vertices=6)
    for i in range(5): box('cooling_vent',(side*.168,-.095+i*.045,.095),(.01,.025,.064),dark,bevel=.003)
box('indicator_panel',(.05,-.015,.222),(.13,.20,.02),dark,bevel=.008)
for i in range(4): box('indicator'+str(i),(.05,-.079+i*.044,.236),(.085,.021,.011),cream if i==0 else brass,bevel=.003)

# Purchased attachments make stronger equipment visibly grow from this body.
for side in [-1,1]:
    cyl('coil', (side*.205,.015,-.01), .057,.36,brass,'Coils')
    for i in range(9): cyl('coil_ring',(side*.205,-.14+i*.035,-.01),.067,.014,steel,'Coils')
box('power_pack',(0,.13,-.34),(.26,.28,.12),paint,'PowerPack',bevel=.025)
for side in [-1,1]: box('pack_rail',(side*.143,.13,-.335),(.033,.30,.14),yellow,'PowerPack',bevel=.008)
box('upper_brace',(0,-.105,.28),(.38,.055,.045),steel,'Brace',bevel=.009)
for side in [-1,1]: box('brace_leg',(side*.185,-.04,.20),(.034,.24,.13),yellow,'Brace',bevel=.009)

objects=[]
for group,group_parts in parts.items():
    bpy.ops.object.select_all(action='DESELECT')
    for o in group_parts:o.select_set(True)
    bpy.context.view_layer.objects.active=group_parts[0]; bpy.ops.object.join(); o=bpy.context.object; o.name=group
    pivots={'Rotor':(0,-.65,.04),'JawLeft':(-.174,-.375,-.035),'JawRight':(.174,-.375,-.035)}
    scene.cursor.location=pivots.get(group,(0,0,0)); bpy.ops.object.origin_set(type='ORIGIN_CURSOR')
    bpy.ops.object.transform_apply(location=False,rotation=True,scale=True)
    objects.append(o)

# Unwrap the batch together into one atlas; bake material colour and ambient
# crevice shading, retaining the original procedural materials in this source.
bpy.ops.object.select_all(action='DESELECT')
for o in objects:o.select_set(True)
bpy.context.view_layer.objects.active=objects[0]
bpy.ops.object.mode_set(mode='EDIT'); bpy.ops.mesh.select_all(action='SELECT'); bpy.ops.uv.smart_project(island_margin=.012); bpy.ops.object.mode_set(mode='OBJECT')
for kind in ['BaseColor','Occlusion']:
    image=bpy.data.images.new('Excavator_'+kind,width=1024,height=1024,alpha=False)
    for m in [paint,yellow,steel,dark,brass,cream]:
        nodes=m.node_tree.nodes; tex=nodes.new('ShaderNodeTexImage'); tex.image=image; nodes.active=tex
    scene.render.bake.use_clear=True
    if kind=='BaseColor':
        scene.render.bake.use_pass_direct=False; scene.render.bake.use_pass_indirect=False; scene.render.bake.use_pass_color=True
        bpy.ops.object.bake(type='DIFFUSE')
    else:bpy.ops.object.bake(type='AO')
    image.filepath_raw=str(out/(kind+'.png')); image.file_format='PNG'; image.save(); image.pack()

bpy.ops.object.select_all(action='DESELECT')
for o in objects:o.select_set(True)
bpy.context.view_layer.objects.active=objects[0]
bpy.ops.export_scene.fbx(filepath=str(out/'Excavator.fbx'),use_selection=True,object_types={'MESH'},add_leaf_bones=False,bake_anim=False,axis_forward='-Z',axis_up='Y')
scene.render.resolution_x=1280; scene.render.resolution_y=960; scene.render.resolution_percentage=100
bpy.ops.object.camera_add(location=(1.35,-1.75,1.0)); camera=bpy.context.object
camera.rotation_euler=(Vector((0,-.18,0))-camera.location).to_track_quat('-Z','Y').to_euler(); camera.data.lens=50; scene.camera=camera
for loc,power,size in [((1,-1,3),500,4),((-2,-.1,1),320,3)]:
    bpy.ops.object.light_add(type='AREA',location=loc); light=bpy.context.object; light.data.energy=power; light.data.shape='DISK'; light.data.size=size
    light.rotation_euler=(Vector((0,-.2,0))-light.location).to_track_quat('-Z','Y').to_euler()
scene.world=bpy.data.worlds.new('ExcavatorStudio'); scene.world.use_nodes=True; scene.world.node_tree.nodes['Background'].inputs[0].default_value=(.09,.13,.16,1)
scene.view_settings.view_transform='AgX'
bpy.data.libraries.write(str(out/'Excavator.blend'),{scene},fake_user=True)
manifest={'tool':'Blender MCP','objects':[{ 'name':o.name,'vertices':len(o.data.vertices),'polygons':len(o.data.polygons)} for o in objects], 'textures':['BaseColor.png','Occlusion.png'],'export':'Excavator.fbx'}
(out/'manifest.json').write_text(json.dumps(manifest,indent=2))
result=manifest
