"""Check the retained source and FBX round trips through connected Blender MCP."""
import bpy
import bmesh
import json
import math
from pathlib import Path

OUT=Path(__file__).resolve().parent
POOLS=('objects','meshes','curves','materials','images','collections','scenes','cameras','lights','worlds','node_groups','fonts','actions')


def ids():
    return {block for name in POOLS for block in getattr(bpy.data,name)}


def close(a,b):
    return len(a)==len(b) and all(abs(x-y)<.00002 for x,y in zip(a,b))


def run():
    manifest=json.loads((OUT/'manifest.json').read_text(encoding='utf-8'))
    current=bpy.context.window.scene
    preserved_path=bpy.data.filepath
    before=ids()
    source={'source_reopened_by_blend_library_read':False,'models':[],'missing_external_images':[]}
    try:
        with bpy.data.libraries.load(str(OUT/'StarterFinds.blend'),link=False) as (available,loaded):
            loaded.scenes=[name for name in available.scenes if name.startswith('SDT_Starter')]
        studio=next(sc for sc in loaded.scenes if sc and '_Studio' in sc.name)
        source['source_reopened_by_blend_library_read']=True
        source['label_artwork_scene_retained']=any('_LabelArtwork' in sc.name for sc in loaded.scenes)
        for record in manifest['variants']:
            obj=next(o for o in studio.objects if o.get('content_id')==record['content_id'])
            mesh=obj.data
            bounds=[max(v.co[i] for v in mesh.vertices)-min(v.co[i] for v in mesh.vertices) for i in range(3)]
            bm=bmesh.new();bm.from_mesh(mesh)
            item={'id':record['content_id'],'finite_vertices':all(math.isfinite(c) for v in mesh.vertices for c in v.co),
                  'uv_range':all(-.00001<=c<=1.00001 for loop in mesh.uv_layers.active.data for c in loop.uv),
                  'uv_count':len(mesh.uv_layers.active.data),'faces':len(mesh.polygons),
                  'nonmanifold_edges':sum(not e.is_manifold for e in bm.edges),
                  'signed_volume_m3':bm.calc_volume(signed=True),'material_count':len(mesh.materials),
                  'export_exists':(OUT/record['fbx']).is_file(),
                  'textures_exist':all((OUT/p).is_file() for p in record['textures'].values()),
                  'dimensions_match_manifest':close(bounds,record['dimensions_m'])}
            bm.free();source['models'].append(item)
        for img in ids()-before:
            if isinstance(img,bpy.types.Image) and img.source=='FILE' and not img.packed_file:
                path=Path(bpy.path.abspath(img.filepath,library=img.library))
                if not path.is_file():source['missing_external_images'].append(str(path))
    finally:
        bpy.context.window.scene=current
        bpy.data.batch_remove(tuple(ids()-before))
    exported=[]
    for record in manifest['variants']:
        before=ids()
        try:
            sc=bpy.data.scenes.new('SDT_Starter_ValidationTemporary');bpy.context.window.scene=sc
            bpy.ops.import_scene.fbx(filepath=str(OUT/record['fbx']),use_anim=False)
            obs=[o for o in sc.objects if o.type=='MESH']
            obj=obs[0];obj.data.calc_loop_triangles()
            bounds=[max(v.co[i] for v in obj.data.vertices)-min(v.co[i] for v in obj.data.vertices) for i in range(3)]
            exported.append({'id':record['content_id'],'mesh_objects':len(obs),
                'dimensions_m':[round(v,6) for v in obj.dimensions],
                'dimension_match':close(obj.dimensions,record['dimensions_m']),
                'uv_layers':len(obj.data.uv_layers),
                'triangle_match':len(obj.data.loop_triangles)==record['triangles'],
                'unit_scale':close(obj.scale,(1,1,1)),
                'origin_at_base':abs(min(v.co.z for v in obj.data.vertices))<.00002})
        finally:
            bpy.context.window.scene=current
            bpy.data.batch_remove(tuple(ids()-before))
    maps=[]
    for path in sorted((OUT/'textures').glob('*.png')):
        image=bpy.data.images.load(str(path),check_existing=False)
        maps.append({'file':path.name,'size':list(image.size),'size_matches_manifest':list(image.size)==[manifest['atlas_size']]*2})
        bpy.data.images.remove(image)
    source_ok=all(x['finite_vertices'] and x['uv_range'] and x['nonmanifold_edges']==0 and x['signed_volume_m3']>0 and x['material_count']==1 and x['export_exists'] and x['textures_exist'] and x['dimensions_match_manifest'] for x in source['models'])
    export_ok=all(x['mesh_objects']==1 and x['dimension_match'] and x['uv_layers']==1 and x['triangle_match'] and x['unit_scale'] and x['origin_at_base'] for x in exported)
    exports={'fbx_roundtrip':exported,'all_passed':export_ok}
    result={'source':source,'exports':exports,'texture_maps':maps,
        'counts':{'categories':manifest['categories'],'models':len(exported),'instances':sum(r['instances'] for r in manifest['variants']),
                  'nominal_value':sum(r['instances']*r['sale_value'] for r in manifest['variants']),
                  'triangles':sum(r['triangles'] for r in manifest['variants']),'texture_maps':len(maps),'atlas_size':manifest['atlas_size']},
        'original_blender_filepath_preserved':bpy.data.filepath==preserved_path,
        'all_passed':source_ok and export_ok and len(exported)==manifest['visual_variants'] and not source['missing_external_images'] and all(x['size_matches_manifest'] for x in maps) and source['label_artwork_scene_retained'],
        'limitation':'Blender delivery checks only; Unity trial integration is Tasks 109/110, with separate runtime evidence. Final model/style acceptance remains Tasks 09/105.'}
    for name,data in [('validation-source.json',source),('validation-exports.json',exports),('validation.json',result)]:
        (OUT/name).write_text(json.dumps(data,indent=2)+'\n',encoding='utf-8')
    return {'all_passed':result['all_passed'],'counts':result['counts'],'missing_images':source['missing_external_images'],'source_passed':source_ok,'exports_passed':export_ok,'filepath_preserved':result['original_blender_filepath_preserved']}
