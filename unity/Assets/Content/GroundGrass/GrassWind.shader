Shader "Something Down There/Sunny Grass"
{
    Properties
    {
        _BaseMap("Blender blade atlas", 2D) = "white" {}
        _WindAmplitude("Tip sway (m)", Range(0, .12)) = .07
        _WindSpeed("Wind speed", Range(0, 3)) = 1.1
    }
    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Geometry" }
        Cull Off
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);
        CBUFFER_START(UnityPerMaterial)
        float4 _BaseMap_ST;
        float _WindAmplitude, _WindSpeed;
        CBUFFER_END
        struct Attributes {
            float4 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float2 uv : TEXCOORD0;
            float2 bladeMotion : TEXCOORD1;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };
        struct Varyings {
            float4 positionCS : SV_POSITION;
            float3 positionWS : TEXCOORD0;
            half3 normalWS : TEXCOORD1;
            float2 uv : TEXCOORD2;
            half fog : TEXCOORD3;
            UNITY_VERTEX_OUTPUT_STEREO
        };
        Varyings Vert(Attributes input) {
            UNITY_SETUP_INSTANCE_ID(input);
            Varyings output = (Varyings)0;
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
            float3 world = TransformObjectToWorld(input.positionOS.xyz);
            float3 root = TransformObjectToWorld(float3(0,0,0));
            float time = _Time.y * _WindSpeed;
            // Two travelling waves form a shared breeze, slowly strengthened by
            // a broader gust. Authored blade phases keep tips from moving in unison.
            float wave = sin(dot(root.xz, float2(.55,.31)) - time * 1.25);
            float crossWave = sin(dot(root.xz, float2(-.28,.46)) - time * .78 + .8);
            float gust = .65 + .35 * sin(dot(root.xz, float2(.16,.11)) - time * .42);
            float2 direction = float2(.91,.41);
            float2 crosswind = float2(-.41,.91);
            float2 breeze = direction * ((.36 + .52 * wave + .22 * crossWave) * gust)
                + crosswind * (.13 * crossWave);
            float flutter = .14 * sin(time * 2.1 + input.bladeMotion.x * TWO_PI
                + dot(root.xz, float2(.38,.2)));
            float t = saturate(input.uv.y);
            float2 offset = _WindAmplitude * (breeze * t * t + crosswind * flutter * t * t * t);
            float scale = length(mul((float3x3)GetObjectToWorldMatrix(), float3(0,1,0)));
            float height = max(input.bladeMotion.y * scale, .1);
            world.xz += offset;
            world.y -= .5 * dot(offset,offset) / height;
            output.positionWS = world;
            output.positionCS = TransformWorldToHClip(world);
            float3 normal = TransformObjectToWorldNormal(input.normalOS);
            float2 derivative = _WindAmplitude * (2 * t * breeze + 3 * t * t * crosswind * flutter) / height;
            // Inverse transpose of the bend's local derivative, including the
            // slight height loss of a bent blade, prevents static lighting stripes.
            float verticalDerivative = 1 - dot(offset,derivative) / height;
            normal.y = (normal.y - dot(normal.xz,derivative)) / max(verticalDerivative,.5);
            output.normalWS = normalize(normal);
            output.uv = input.uv;
            output.fog = ComputeFogFactor(output.positionCS.z);
            return output;
        }
        ENDHLSL
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma target 3.5
            #pragma multi_compile_instancing
            #pragma instancing_options assumeuniformscaling
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile_fragment _ _SHADOWS_SOFT _SHADOWS_SOFT_LOW _SHADOWS_SOFT_MEDIUM _SHADOWS_SOFT_HIGH
            #pragma multi_compile_fog
            half4 Frag(Varyings input, FRONT_FACE_TYPE facing : FRONT_FACE_SEMANTIC) : SV_Target {
                half3 normal = normalize(input.normalWS);
                Light sun = GetMainLight(TransformWorldToShadowCoord(input.positionWS));
                half3 albedo = SAMPLE_TEXTURE2D(_BaseMap,sampler_BaseMap,input.uv).rgb;
                // Thin leaves share light across both sides. Broad fill and the
                // baked palette keep the cartoon shape readable without hard folds.
                half diffuse = .5 + .5 * abs(dot(normal,sun.direction));
                half3 ambientNormal = normalize(half3(normal.x * .35,.9,normal.z * .35));
                half3 light = SampleSH(ambientNormal) + sun.color * diffuse * sun.shadowAttenuation;
                return half4(MixFog(albedo * light,input.fog),1);
            }
            ENDHLSL
        }
        Pass
        {
            Name "DepthOnly"
            Tags { "LightMode"="DepthOnly" }
            ZWrite On ColorMask R
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Depth
            #pragma target 3.5
            #pragma multi_compile_instancing
            #pragma instancing_options assumeuniformscaling
            half4 Depth(Varyings input) : SV_Target { return input.positionCS.z; }
            ENDHLSL
        }
        Pass
        {
            Name "DepthNormals"
            Tags { "LightMode"="DepthNormals" }
            ZWrite On
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Normals
            #pragma target 3.5
            #pragma multi_compile_instancing
            #pragma instancing_options assumeuniformscaling
            half4 Normals(Varyings input, FRONT_FACE_TYPE facing : FRONT_FACE_SEMANTIC) : SV_Target {
                return half4(normalize(input.normalWS) * IS_FRONT_VFACE(facing,1,-1),0);
            }
            ENDHLSL
        }
    }
}
