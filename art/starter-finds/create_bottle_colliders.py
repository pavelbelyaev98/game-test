"""Build retained low-polygon convex bottle collision hulls through Blender MCP."""
import bpy
import bmesh
import math
import json
from pathlib import Path
from mathutils import Vector

ROOT=Path(__file__).resolve().parent
PREFIX='SDT_Starter'

def build():
    catalog=json.loads((ROOT/'catalog.json').read_text(encoding='utf-8'))
    studio=bpy.data.scenes[PREFIX+'_Studio']
    collection=bpy.data.collections.get(PREFIX+'_Collision')
    if collection is None:
        collection=bpy.data.collections.new(PREFIX+'_Collision');studio.collection.children.link(collection)
    original_scene=bpy.context.window.scene;bpy.context.window.scene=studio
    result=[]
    try:
        for entry in catalog['variants']:
            visible=bpy.data.objects[PREFIX+'_'+entry['content_id']]
            points=[v.co.copy() for v in visible.data.vertices]
            directions=[Vector((s if i==axis else 0 for i in range(3))) for axis in range(3) for s in (-1,1)]
            for i in range(106):
                z=1-2*(i+.5)/106;r=math.sqrt(1-z*z);a=i*2.39996323
                directions.append(Vector((math.cos(a)*r,math.sin(a)*r,z)))
            # At most 112 support vertices => at most 220 triangulated hull faces.
            chosen={tuple(max(points,key=lambda p:p.dot(d))) for d in directions}
            bm=bmesh.new()
            for p in sorted(chosen):bm.verts.new(p)
            hull=bmesh.ops.convex_hull(bm,input=list(bm.verts),use_existing_faces=False)
            unused=[g for g in hull['geom_interior']+hull['geom_unused'] if isinstance(g,bmesh.types.BMVert) and g.is_valid]
            if unused:bmesh.ops.delete(bm,geom=unused,context='VERTS')
            bmesh.ops.triangulate(bm,faces=list(bm.faces));bmesh.ops.recalc_face_normals(bm,faces=list(bm.faces))
            name=PREFIX+'_Collision_'+entry['content_id'];obj=bpy.data.objects.get(name)
            if obj is None:
                mesh=bpy.data.meshes.new(name);obj=bpy.data.objects.new(name,mesh);collection.objects.link(obj)
            bm.to_mesh(obj.data);bm.free();obj.data.update();obj.data.calc_loop_triangles()
            assert len(obj.data.loop_triangles)<=220
            obj['collision_for']=entry['content_id'];obj.hide_render=True;obj.hide_set(False)
            for o in bpy.context.view_layer.objects:o.select_set(False)
            obj.select_set(True);bpy.context.view_layer.objects.active=obj
            obj.location=(0,0,0);obj.rotation_euler=(0,0,0);obj.scale=(1,1,1)
            path=ROOT/entry['collision_fbx'];path.parent.mkdir(exist_ok=True)
            bpy.ops.export_scene.fbx(filepath=str(path),use_selection=True,object_types={'MESH'},apply_unit_scale=True,
                use_space_transform=True,bake_space_transform=False,axis_forward='-Z',axis_up='Y',add_leaf_bones=False,
                use_mesh_modifiers=True,use_triangles=True,bake_anim=False,path_mode='AUTO')
            result.append({'id':entry['content_id'],'triangles':len(obj.data.loop_triangles),'vertices':len(obj.data.vertices),'file':entry['collision_fbx']})
            obj.hide_set(True)
    finally:bpy.context.window.scene=original_scene
    (ROOT/'collision-manifest.json').write_text(json.dumps(result,indent=2)+'\n',encoding='utf-8')
    return result

def validate():
    import importlib.util
    spec=importlib.util.spec_from_file_location('starter_validation',str(ROOT/'validate_starter_finds.py'))
    checks=importlib.util.module_from_spec(spec);spec.loader.exec_module(checks)
    original=bpy.context.window.scene;rows=[]
    catalog=json.loads((ROOT/'catalog.json').read_text(encoding='utf-8'))
    for entry in catalog['variants']:
        before=checks.ids()
        try:
            temp=bpy.data.scenes.new(PREFIX+'_CollisionValidation');bpy.context.window.scene=temp
            bpy.ops.import_scene.fbx(filepath=str(ROOT/entry['collision_fbx']),use_anim=False)
            objects=[o for o in temp.objects if o.type=='MESH'];assert len(objects)==1
            obj=objects[0];obj.data.calc_loop_triangles()
            dims=[max(v.co[i] for v in obj.data.vertices)-min(v.co[i] for v in obj.data.vertices) for i in range(3)]
            bm=bmesh.new();bm.from_mesh(obj.data)
            row={'id':entry['content_id'],'triangles':len(obj.data.loop_triangles),'dimensions_match':checks.close(dims,entry['dimensions_m']),
                'unit_scale':checks.close(obj.scale,(1,1,1)),'manifold':all(e.is_manifold for e in bm.edges),'positive_volume':bm.calc_volume(signed=True)>0}
            bm.free();row['passed']=row['triangles']<=220 and all(row[k] for k in ['dimensions_match','unit_scale','manifold','positive_volume']);rows.append(row)
        finally:
            bpy.context.window.scene=original;bpy.data.batch_remove(tuple(checks.ids()-before))
    result={'all_passed':all(r['passed'] for r in rows),'hulls':rows}
    (ROOT/'collision-validation.json').write_text(json.dumps(result,indent=2)+'\n',encoding='utf-8')
    return result
