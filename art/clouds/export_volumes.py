"""Task 134: export depth-bearing derivatives of the existing approved cloud forms via Blender MCP."""
import bpy, os
from mathutils import Vector
root=os.path.dirname(os.path.abspath(__file__))
source=bpy.data.scenes['Sunny Clouds Authoring 130']
previous=bpy.context.window.scene
scene=bpy.data.scenes.get('Sunny Cloud Volumes 134') or bpy.data.scenes.new('Sunny Cloud Volumes 134')
try:
    bpy.context.window.scene=source
    graph=bpy.context.evaluated_depsgraph_get()
    exported=[]
    for i, original in enumerate(sorted([o for o in source.objects if o.type=='MESH'],key=lambda o:o.name)):
        name='CloudVolume_'+str(i+1)
        old=scene.objects.get(name)
        if old:bpy.data.objects.remove(old,do_unlink=True)
        mesh=bpy.data.meshes.new_from_object(original.evaluated_get(graph),depsgraph=graph)
        # Atlas vertical Y becomes physical Z-up. Broaden the old shallow Z
        # thickness into a full depth-bearing volume, preserving its crown.
        for v in mesh.vertices:
            x,y,z=v.co
            v.co=Vector((x,-z*2.4,y))
        mesh.update()
        low=min(v.co.z for v in mesh.vertices);high=max(v.co.z for v in mesh.vertices)
        colors=mesh.color_attributes.new(name='CloudTint',type='FLOAT_COLOR',domain='CORNER')
        for polygon in mesh.polygons:
            polygon.use_smooth=False
            for li in polygon.loop_indices:
                v=mesh.vertices[mesh.loops[li].vertex_index]
                height=(v.co.z-low)/(high-low)
                # Pale authored volume shading; neither camera-facing projection
                # nor runtime light/shadow changes can turn the underside dark.
                t=max(0,min(1,height*.72+(polygon.normal.z*.5+.5)*.28))
                colors.data[li].color=(.76+.22*t,.88+.11*t,.90+.09*t,1)
        mesh.materials.clear()
        obj=bpy.data.objects.new(name,mesh);scene.collection.objects.link(obj)
        exported.append(obj)
    bpy.context.window.scene=scene
    for o in bpy.context.selected_objects:o.select_set(False)
    for o in exported:o.select_set(True)
    bpy.context.view_layer.objects.active=exported[0]
    bpy.ops.export_scene.fbx(filepath=os.path.join(root,'CloudVolumes.fbx'),use_selection=True,
        object_types={'MESH'},add_leaf_bones=False,bake_anim=False,axis_forward='-Z',axis_up='Y',
        apply_unit_scale=True,bake_space_transform=True,colors_type='LINEAR')
    recipe=bpy.data.texts.get('Cloud134_export_volumes.py') or bpy.data.texts.new('Cloud134_export_volumes.py')
    recipe.clear();recipe.write(open(__file__,encoding='utf-8').read())
    bpy.data.libraries.write(os.path.join(root,'Clouds.blend'),{source,scene,recipe},fake_user=True)
    result={'exports':len(exported),'triangles':sum(sum(len(p.vertices)-2 for p in o.data.polygons) for o in exported),'objects':[{'name':o.name,'size':list(o.dimensions)} for o in exported]}
finally:
    bpy.context.window.scene=previous
