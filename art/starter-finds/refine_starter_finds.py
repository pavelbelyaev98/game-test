"""Richer original surface/label pass. Run through Blender MCP after create()."""
import bpy
import math
import importlib.util
import sys
from pathlib import Path
from mathutils import Vector

HERE = Path(__file__).resolve().parent
spec = importlib.util.spec_from_file_location('sdt_starter', HERE/'create_starter_finds.py')
m = importlib.util.module_from_spec(spec)
sys.modules['sdt_starter'] = m
spec.loader.exec_module(m)
PREFIX = m.PREFIX
N = m.node
W = m.wire
C = m.calc
M = m.mix
R = m.ramp


def emission(name, color):
    mat = bpy.data.materials.get(PREFIX+'_'+name) or bpy.data.materials.new(PREFIX+'_'+name)
    mat.use_nodes = True
    mat.node_tree.nodes.clear()
    out = N(mat, 'ShaderNodeOutputMaterial', 'Output')
    emit = N(mat, 'ShaderNodeEmission', 'Flat artwork')
    emit.inputs[0].default_value = m.rgba(color)
    W(mat, emit.outputs[0], out.inputs['Surface'])
    return mat


def stroke(scene, name, points, mat, width=.003, closed=False):
    curve = bpy.data.curves.new(PREFIX+'_'+name, 'CURVE')
    curve.dimensions = '3D'; curve.resolution_u = 2
    curve.bevel_depth = width; curve.bevel_resolution = 1
    spline = curve.splines.new('POLY'); spline.points.add(len(points)-1)
    for p, xy in zip(spline.points, points): p.co = (xy[0], xy[1], .02, 1)
    spline.use_cyclic_u = closed
    obj = bpy.data.objects.new(PREFIX+'_'+name, curve)
    scene.collection.objects.link(obj); curve.materials.append(mat)
    return obj


def text_line(scene, name, text, y, height, width, mat):
    curve = bpy.data.curves.new(PREFIX+'_'+name, 'FONT')
    curve.body = text; curve.align_x = 'CENTER'; curve.align_y = 'CENTER'
    curve.size = height; curve.space_character = 1.10; curve.resolution_u = 8
    obj = bpy.data.objects.new(PREFIX+'_'+name, curve)
    scene.collection.objects.link(obj); obj.location = (0, y, .025)
    curve.materials.append(mat)
    bpy.context.view_layer.update()
    if obj.dimensions.x > width: obj.scale.x *= width/obj.dimensions.x
    return obj


def label_art():
    """Retained native Blender text/curves; no downloaded fonts or label art."""
    name = PREFIX+'_LabelArtwork'
    if name in bpy.data.scenes:
        return {'scene': name, 'existing': True}
    old = bpy.context.window.scene
    sc = bpy.data.scenes.new(name); bpy.context.window.scene = sc
    sc.render.engine = 'BLENDER_EEVEE'
    sc.render.resolution_x = sc.render.resolution_y = 1024
    sc.render.resolution_percentage = 100; sc.render.image_settings.file_format = 'PNG'
    sc.view_settings.view_transform = 'Standard'; sc.view_settings.look = 'None'
    sc.view_settings.exposure = 0; sc.view_settings.gamma = 1
    world = bpy.data.worlds.new(PREFIX+'_ArtworkWorld');world.use_nodes=True
    world.node_tree.nodes['Background'].inputs[0].default_value=(1,1,1,1)
    world.node_tree.nodes['Background'].inputs[1].default_value=0;sc.world=world
    camd = bpy.data.cameras.new(PREFIX+'_ArtworkCamera')
    cam = bpy.data.objects.new(PREFIX+'_ArtworkCamera', camd);sc.collection.objects.link(cam)
    cam.location=(0,0,3);camd.type='ORTHO';camd.ortho_scale=1;sc.camera=cam
    black=emission('ArtworkInk','000000');white=emission('ArtworkPaper','ffffff')
    bpy.ops.mesh.primitive_plane_add(size=2)
    paper=bpy.context.object;paper.name=PREFIX+'_ArtworkPaper';paper.data.name=paper.name
    paper.data.materials.append(white)
    folder=HERE/'label-art';folder.mkdir(exist_ok=True)
    variants=[
        ('Lemonade','ЛИМОНАД','ОСВЕЖАВАЩА НАПИТКА','0,5 Л', 'lemon'),
        ('Beer','ПИВО','СВЕТЛО','0,33 Л', 'barley'),
        ('Syrup','СИРОП','МАЛИНИ','0,5 Л', 'berry'),
    ]
    base=set(sc.objects)
    for key, title, subtitle, amount, icon in variants:
        for o in sc.objects:
            if o not in base:o.hide_render=True
        own=set(sc.objects)
        stroke(sc,key+'Outer',[(-.44,-.44),(.44,-.44),(.44,.44),(-.44,.44)],black,.004,True)
        stroke(sc,key+'Inner',[(-.416,-.412),(.416,-.412),(.416,.412),(-.416,.412)],black,.0015,True)
        text_line(sc,key+'Title',title,.16,.16,.77,black)
        text_line(sc,key+'Subtitle',subtitle,.33,.062,.74,black)
        text_line(sc,key+'Amount',amount,-.32,.095,.65,black)
        for y in (.04,-.24):
            stroke(sc,key+'Rule',[(-.35,y),(.35,y)],black,.002)
        if icon=='barley':
            for direction in (-1,1):
                stroke(sc,key+'Stalk',[(0,-.22),(direction*.055,-.10),(direction*.07,-.005)],black,.003)
                for level in range(4):
                    y=-.18+level*.042;x=direction*(.025+level*.012)
                    stroke(sc,key+'Grain',[(x,y),(x+direction*.045,y+.015),(x+direction*.008,y+.036),(x,y)],black,.003,True)
        else:
            for shift in (-.07,.07):
                pts=[(shift+.087*math.cos(i*math.tau/48),-.105+.075*math.sin(i*math.tau/48)) for i in range(48)]
                stroke(sc,key+'Fruit',pts,black,.003,True)
            stroke(sc,key+'Leaf',[(-.015,-.06),(-.07,.015),(.01,-.01),(.04,.01),(.07,-.04)],black,.003)
            if icon=='lemon':
                for a in range(6):
                    angle=a*math.tau/6
                    stroke(sc,key+'Segment',[(.07,-.105),(.07+.07*math.cos(angle),-.105+.06*math.sin(angle))],black,.0015)
        sc.render.filepath=str(folder/(key+'.png'))
        bpy.ops.render.render(write_still=True)
        img=bpy.data.images.load(sc.render.filepath,check_existing=True)
        img.name=PREFIX+'_Label_'+key;img.colorspace_settings.name='Non-Color'
        for o in set(sc.objects)-own:o['label_artwork']=key
    bpy.context.window.scene=old
    return {'scene':sc.name,'labels':len(variants),'font':'Blender built-in Bfont outlines'}


def vec(mat, op, a, b, title):
    n=N(mat,'ShaderNodeVectorMath',title);n.operation=op
    W(mat,a,n.inputs[0]);W(mat,b,n.inputs[1]);return n.outputs[0]


def detailed_surface(name, palette, kind, rough, metal=0, artwork=None):
    mat=bpy.data.materials[PREFIX+'_'+name];mat.node_tree.nodes.clear();mat.use_fake_user=True
    tex=N(mat,'ShaderNodeTexCoord','Object space wear')
    xyz=N(mat,'ShaderNodeSeparateXYZ','Object dimensions');W(mat,tex.outputs['Generated'],xyz.inputs[0])
    coords=tex.outputs['Generated']
    equal=vec(mat,'MULTIPLY',coords,(1,1,2.6) if kind=='glass' else (2.3,1.1,.7) if kind=='clay' else (1,1,1.5),'Physical grain proportions')
    patches=m.noise(mat,equal,3.0,'Uneven old material',2)
    grain=m.noise(mat,equal,90,'Small pores and dust',2)
    chips=m.noise(mat,equal,23,'Medium chips and stains',2.5)
    color=R(mat,patches,[(.20,palette[0]),(.54,palette[1]),(.82,palette[2])],'Material palette')
    # Sparse scratches, not a uniform layer of blurry light mottling.
    stretched=vec(mat,'MULTIPLY',equal,(1,1,15) if kind!='clay' else (1,1,3),'Worn scratch direction')
    scratch_noise=m.noise(mat,stretched,50,'Fine irregular scratches',1)
    scratch=C(mat,'MULTIPLY',C(mat,'GREATER_THAN',scratch_noise,.74),C(mat,'GREATER_THAN',chips,.5))
    color=M(mat,C(mat,'MULTIPLY',scratch,.48),color,m.rgba(palette[2]),'Sparse rubbed scratches')
    bottom=C(mat,'LESS_THAN',xyz.outputs['Z'],C(mat,'ADD',.035,C(mat,'MULTIPLY',chips,.16)))
    dirt=C(mat,'MULTIPLY',bottom,C(mat,'GREATER_THAN',grain,.36))
    roughval=C(mat,'ADD',rough-.11,C(mat,'MULTIPLY',chips,.20))
    metalval=metal
    height=C(mat,'MULTIPLY',grain,.12 if kind=='glass' else .36)
    if kind=='glass':
        neck=C(mat,'MULTIPLY',C(mat,'GREATER_THAN',xyz.outputs['Z'],.91),C(mat,'LESS_THAN',chips,.40))
        dirt=C(mat,'MAXIMUM',dirt,C(mat,'MULTIPLY',neck,.65))
        mineral=C(mat,'MULTIPLY',C(mat,'GREATER_THAN',chips,.67),C(mat,'GREATER_THAN',grain,.52))
        color=M(mat,C(mat,'MULTIPLY',mineral,.32),color,m.rgba('a8a88c'),'Patchy mineral bloom')
        height=C(mat,'ADD',height,C(mat,'MULTIPLY',scratch,.20))
    elif kind in ('metal','paint'):
        near_rim=C(mat,'MAXIMUM',C(mat,'LESS_THAN',xyz.outputs['Z'],.14),C(mat,'GREATER_THAN',xyz.outputs['Z'],.87))
        rust=C(mat,'GREATER_THAN',C(mat,'ADD',patches,C(mat,'MULTIPLY',near_rim,.15)),.62 if kind=='metal' else .57)
        rust_color=R(mat,grain,[(.25,'352719'),(.52,'77401e'),(.76,'b57838')],'Layered iron oxidation')
        color=M(mat,rust,color,rust_color,'Rust around seams and damaged paint')
        metalval=C(mat,'MULTIPLY',C(mat,'SUBTRACT',1,rust),metal)
        roughval=C(mat,'ADD',roughval,C(mat,'MULTIPLY',rust,.22))
        height=C(mat,'ADD',height,C(mat,'MULTIPLY',rust,.25))
        if kind=='paint':
            peel=C(mat,'LESS_THAN',chips,.34)
            color=M(mat,peel,color,m.rgba('6b746c'),'Chipped colored tin')
    else:
        pore=C(mat,'LESS_THAN',grain,.31)
        color=M(mat,C(mat,'MULTIPLY',pore,.58),color,m.rgba('442c20'),'Visible clay pores')
        speck=C(mat,'GREATER_THAN',grain,.77)
        color=M(mat,C(mat,'MULTIPLY',speck,.48),color,m.rgba('bba382'),'Exposed pale aggregate')
        # Hairline cracks have a sparse domain, so the whole brick is not crazed.
        vor=N(mat,'ShaderNodeTexVoronoi','Pressed clay fissures');vor.feature='DISTANCE_TO_EDGE'
        vor.inputs['Scale'].default_value=7;W(mat,equal,vor.inputs['Vector'])
        crack=C(mat,'MULTIPLY',C(mat,'LESS_THAN',vor.outputs['Distance'],.012),C(mat,'LESS_THAN',chips,.37))
        color=M(mat,C(mat,'MULTIPLY',crack,.6),color,m.rgba('493025'),'Interrupted clay cracks')
        height=C(mat,'SUBTRACT',height,C(mat,'MULTIPLY',crack,.42))
    color=M(mat,C(mat,'MULTIPLY',dirt,.72),color,m.rgba('524a31'),'Adhering soil at foot and seams')
    roughval=C(mat,'MAXIMUM',roughval,C(mat,'MULTIPLY',dirt,.90))
    if artwork:
        if kind=='clay':
            u=C(mat,'ADD',.5,C(mat,'DIVIDE',C(mat,'SUBTRACT',xyz.outputs['X'],.5),.61))
            v=C(mat,'ADD',.5,C(mat,'DIVIDE',C(mat,'SUBTRACT',xyz.outputs['Y'],.5),.49))
            face=C(mat,'GREATER_THAN',xyz.outputs['Z'],.80)
        else:
            u=C(mat,'ADD',.5,C(mat,'DIVIDE',C(mat,'SUBTRACT',xyz.outputs['X'],.5),.88))
            low,span=(.21,.37) if kind=='glass' else (.20,.62)
            v=C(mat,'DIVIDE',C(mat,'SUBTRACT',xyz.outputs['Z'],low),span)
            face=C(mat,'LESS_THAN',xyz.outputs['Y'],.34)
        mapping=N(mat,'ShaderNodeCombineXYZ','Label placement');W(mat,u,mapping.inputs['X']);W(mat,v,mapping.inputs['Y'])
        img=N(mat,'ShaderNodeTexImage','Original Bulgarian artwork');img.image=bpy.data.images[PREFIX+'_Label_'+artwork]
        img.extension='CLIP';W(mat,mapping.outputs[0],img.inputs['Vector'])
        edge=C(mat,'MINIMUM',C(mat,'MINIMUM',u,C(mat,'SUBTRACT',1,u)),C(mat,'MINIMUM',v,C(mat,'SUBTRACT',1,v)))
        domain=C(mat,'MULTIPLY',face,C(mat,'GREATER_THAN',edge,0))
        ink=C(mat,'SUBTRACT',1,img.outputs['Color'])
        # Lose portions of lettering and borders, while fragments still reward a close view.
        fade=C(mat,'MULTIPLY',C(mat,'GREATER_THAN',chips,.40),C(mat,'GREATER_THAN',grain,.31))
        ink=C(mat,'MULTIPLY',ink,fade)
        if kind=='clay':
            stamp=C(mat,'MULTIPLY',domain,ink)
            color=M(mat,C(mat,'MULTIPLY',stamp,.50),color,m.rgba('4b3324'),'Worn Bulgarian maker stamp')
            height=C(mat,'SUBTRACT',height,C(mat,'MULTIPLY',stamp,.40))
        else:
            torn=C(mat,'GREATER_THAN',edge,C(mat,'ADD',.009,C(mat,'MULTIPLY',chips,.055)))
            tears=C(mat,'GREATER_THAN',chips,.275)
            missing_corner=C(mat,'LESS_THAN',C(mat,'ADD',u,v),C(mat,'ADD',1.68,C(mat,'MULTIPLY',chips,.12)))
            bite=C(mat,'ADD',C(mat,'MULTIPLY',u,u),C(mat,'POWER',C(mat,'SUBTRACT',v,.44),2))
            missing_edge=C(mat,'GREATER_THAN',bite,.008)
            paper_mask=C(mat,'MULTIPLY',domain,C(mat,'MULTIPLY',torn,tears))
            paper_mask=C(mat,'MULTIPLY',paper_mask,C(mat,'MULTIPLY',missing_corner,missing_edge))
            paper=R(mat,patches,[(.23,'8d7851'),(.51,'c3b686'),(.78,'ded4a6')],'Aged paper')
            print_color='53604a' if artwork=='Lemonade' else '644127' if artwork=='Beer' else '81424b' if artwork=='Syrup' else '7c3324'
            paper=M(mat,C(mat,'MULTIPLY',ink,.77),paper,m.rgba(print_color),'Faded Bulgarian print')
            paper=M(mat,C(mat,'MULTIPLY',C(mat,'LESS_THAN',chips,.36),.35),paper,m.rgba('7e704c'),'Tide-stained paper')
            color=M(mat,paper_mask,color,paper,'Torn printed label')
            roughval=C(mat,'ADD',C(mat,'MULTIPLY',C(mat,'SUBTRACT',1,paper_mask),roughval),C(mat,'MULTIPLY',paper_mask,.84))
            metalval=C(mat,'MULTIPLY',C(mat,'SUBTRACT',1,paper_mask),metalval)
            height=C(mat,'ADD',height,C(mat,'MULTIPLY',paper_mask,.18))
    p=N(mat,'ShaderNodeBsdfPrincipled','Original Surface')
    W(mat,color,p.inputs['Base Color']);W(mat,roughval,p.inputs['Roughness']);W(mat,metalval,p.inputs['Metallic'])
    if kind=='glass':
        p.inputs['Coat Weight'].default_value=.32;p.inputs['Coat Roughness'].default_value=.25
    bump=N(mat,'ShaderNodeBump','Pitting scratches and paper relief')
    bump.inputs['Strength'].default_value=.42
    bump.inputs['Distance'].default_value=.0009 if kind=='glass' else .0018
    W(mat,height,bump.inputs['Height']);W(mat,bump.outputs[0],p.inputs['Normal'])
    emit=N(mat,'ShaderNodeEmission','Color Bake');W(mat,color,emit.inputs[0])
    masks=N(mat,'ShaderNodeCombineColor','Metal AO Roughness')
    W(mat,metalval,masks.inputs[0]);masks.inputs[1].default_value=1;W(mat,roughval,masks.inputs[2])
    me=N(mat,'ShaderNodeEmission','Masks Bake');W(mat,masks.outputs[0],me.inputs[0])
    out=N(mat,'ShaderNodeOutputMaterial','Output');W(mat,p.outputs[0],out.inputs['Surface'])
    for i,n in enumerate(mat.node_tree.nodes):n.location=((i%9)*200,-(i//9)*220)
    mat.diffuse_color=m.rgba(palette[1])


def prepare():
    label_art();bpy.context.window.scene=m.scene()
    detailed_surface('GreenGlass',['182e23','38543a','667b50'],'glass',.32,artwork='Lemonade')
    detailed_surface('AmberGlass',['422719','7b4c24','ac7639'],'glass',.34,artwork='Beer')
    detailed_surface('BlueGlass',['1d393b','3c6663','77918a'],'glass',.33,artwork='Syrup')
    for obj in m.models():
        obj.data.materials.clear()
        for name in obj['source_materials']:obj.data.materials.append(bpy.data.materials[name])
        for f,index in zip(obj.data.polygons,obj['source_material_indices']):f.material_index=index
    # Sufficient texel density for broken Cyrillic strokes at close inspection.
    m.ATLAS_SIZE=2048
    for image in bpy.data.images:
        if image.name.startswith(PREFIX) and any(image.name==PREFIX+'_'+g+'_'+c for g in ('Bottles',) for c in ('BaseColor','Normal','Masks')):
            image.scale(2048,2048)
    return {'revision':'richer wear and Bulgarian print','atlas_size':2048,'geometry':'three enlarged bottle variants'}
