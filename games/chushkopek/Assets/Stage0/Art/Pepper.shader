Shader "Stage0/Pepper"
{
    Properties
    {
        _Roast ("Roast", Range(0,1)) = 0
        _Burn ("Burnt quality", Range(0,1)) = 0
        _Ready ("Skin loosened", Range(0,1)) = 0
        _Flesh ("Flesh layer", Range(0,1)) = 0
        _Hover ("Selected skin", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull Off
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert
        #pragma target 3.0
        struct Input { float3 localPos; };
        float _Roast, _Burn, _Ready, _Flesh, _Hover;
        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input,o);
            float spots=sin(v.vertex.x*115+v.vertex.y*61)*sin(v.vertex.z*97-v.vertex.y*83);
            v.vertex.xyz += v.normal * smoothstep(.3,.85,spots) * _Roast * (1-_Flesh) * .003;
            o.localPos = v.vertex.xyz;
        }
        float hash(float3 p) { return frac(sin(dot(p,float3(127.1,311.7,74.7)))*43758.5453); }
        float noise(float3 p)
        {
            float3 i=floor(p), f=frac(p); f=f*f*(3-2*f);
            return lerp(lerp(lerp(hash(i),hash(i+float3(1,0,0)),f.x),lerp(hash(i+float3(0,1,0)),hash(i+float3(1,1,0)),f.x),f.y),
                lerp(lerp(hash(i+float3(0,0,1)),hash(i+float3(1,0,1)),f.x),lerp(hash(i+float3(0,1,1)),hash(i+float3(1,1,1)),f.x),f.y),f.z);
        }
        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float n=noise(IN.localPos*47)+noise(IN.localPos*105)*.22;
            float charred=smoothstep(.84-_Roast*.49-_Burn*.3,.94-_Roast*.49-_Burn*.3,n)*_Roast;
            float3 raw=lerp(float3(.42,.018,.009),float3(.72,.045,.013),noise(IN.localPos*18));
            float3 skin=lerp(raw,float3(.027,.018,.012),charred);
            float3 flesh=lerp(float3(.85,.17,.028),float3(.27,.075,.019),_Burn*.7);
            o.Albedo=lerp(skin,flesh,_Flesh);
            o.Albedo=lerp(o.Albedo,o.Albedo+float3(.075,.065,.025),_Hover*.55);
            o.Metallic=0;
            o.Smoothness=lerp(lerp(.52,.16,charred),.68,_Flesh);
            o.Emission=float3(.018,.014,.002)*_Hover;
            o.Alpha=1;
        }
        ENDCG
    }
    Fallback "Standard"
}
