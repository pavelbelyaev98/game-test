Shader "Something Down There/Ground Triplanar"
{
    Properties
    {
        _SoilAlbedo("Soil colour", 2D) = "white" {}
        [Normal] _SoilNormal("Soil normal", 2D) = "bump" {}
        _SoilRoughness("Soil roughness (R), contact occlusion (G)", 2D) = "white" {}
        _TurfAlbedo("Turf colour", 2D) = "white" {}
        [Normal] _TurfNormal("Turf normal", 2D) = "bump" {}
        _TurfRoughness("Turf roughness", 2D) = "white" {}
        _TileMetres("Turf tile metres", Float) = 1
        _SoilTileMetres("Soil tile metres", Float) = 2
        _NormalStrength("Soil relief", Range(0, 2)) = 0.8
        _TurfNormalStrength("Turf relief", Range(0, 2)) = 0.45
        _SurfaceHeight("Original surface height", Float) = 0
        _TurfDepth("Turf transition depth", Range(0.05, 0.5)) = 0.2
        _MacroVariation("Broad colour variation", Range(0, 0.4)) = 0.12
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" "Queue"="Geometry" }
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Packing.hlsl"
        CBUFFER_START(UnityPerMaterial)
            float _TileMetres;
            float _SoilTileMetres;
            float _NormalStrength;
            float _TurfNormalStrength;
            float _SurfaceHeight;
            float _TurfDepth;
            float _MacroVariation;
        CBUFFER_END
        TEXTURE2D(_SoilAlbedo); SAMPLER(sampler_SoilAlbedo);
        TEXTURE2D(_SoilNormal); SAMPLER(sampler_SoilNormal);
        TEXTURE2D(_SoilRoughness); SAMPLER(sampler_SoilRoughness);
        TEXTURE2D(_TurfAlbedo); SAMPLER(sampler_TurfAlbedo);
        TEXTURE2D(_TurfNormal); SAMPLER(sampler_TurfNormal);
        TEXTURE2D(_TurfRoughness); SAMPLER(sampler_TurfRoughness);

        struct GroundAttributes
        {
            float4 positionOS : POSITION;
            float3 normalOS : NORMAL;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };
        struct GroundVaryings
        {
            float4 positionCS : SV_POSITION;
            float3 positionWS : TEXCOORD0;
            half3 normalWS : TEXCOORD1;
            half fogFactor : TEXCOORD2;
            UNITY_VERTEX_INPUT_INSTANCE_ID
            UNITY_VERTEX_OUTPUT_STEREO
        };
        GroundVaryings GroundVertex(GroundAttributes input)
        {
            GroundVaryings output = (GroundVaryings)0;
            UNITY_SETUP_INSTANCE_ID(input);
            UNITY_TRANSFER_INSTANCE_ID(input, output);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
            output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
            output.positionCS = TransformWorldToHClip(output.positionWS);
            output.normalWS = TransformObjectToWorldNormal(input.normalOS);
            output.fogFactor = ComputeFogFactor(output.positionCS.z);
            return output;
        }

        void GroundSurface(float3 position, half3 geometricNormal,
            out half3 colour, out half3 normal, out half roughness, out half occlusion)
        {
            half3 n = normalize(geometricNormal);
            half3 weights = pow(abs(n), 8);
            weights /= max(dot(weights, 1.0), 0.0001);
            float3 p = position / max(_SoilTileMetres, 0.05);
            float2 turfUV = position.xz / max(_TileMetres, 0.05);
            // One world-space origin across chunks and rim meshes. No mesh UVs
            // or tangents: fresh vertical cuts keep the same physical texel scale.
            half3 axisSign = half3(n.x < 0 ? -1 : 1, n.y < 0 ? -1 : 1, n.z < 0 ? -1 : 1);
            float2 uvX = float2(p.z * axisSign.x, p.y);
            float2 uvY = float2(p.x * axisSign.y, p.z);
            float2 uvZ = float2(-p.x * axisSign.z, p.y);
            half3 cx = SAMPLE_TEXTURE2D(_SoilAlbedo, sampler_SoilAlbedo, uvX).rgb;
            half3 cy = SAMPLE_TEXTURE2D(_SoilAlbedo, sampler_SoilAlbedo, uvY).rgb;
            half3 cz = SAMPLE_TEXTURE2D(_SoilAlbedo, sampler_SoilAlbedo, uvZ).rgb;
            colour = cx * weights.x + cy * weights.y + cz * weights.z;
            half3 nx = UnpackNormalScale(SAMPLE_TEXTURE2D(_SoilNormal, sampler_SoilNormal, uvX), _NormalStrength);
            half3 ny = UnpackNormalScale(SAMPLE_TEXTURE2D(_SoilNormal, sampler_SoilNormal, uvY), _NormalStrength);
            half3 nz = UnpackNormalScale(SAMPLE_TEXTURE2D(_SoilNormal, sampler_SoilNormal, uvZ), _NormalStrength);
            // Surface-gradient projection: a flat normal map reproduces the
            // density-gradient mesh normal exactly, including blended slopes.
            half3 dx = half3(0, nx.y, nx.x * axisSign.x) / max(nx.z, 0.25);
            half3 dy = half3(ny.x * axisSign.y, 0, ny.y) / max(ny.z, 0.25);
            half3 dz = half3(-nz.x * axisSign.z, nz.y, 0) / max(nz.z, 0.25);
            half3 detail = dx * weights.x + dy * weights.y + dz * weights.z;
            detail -= n * dot(detail, n);
            normal = normalize(n + detail);
            half2 soilMask = SAMPLE_TEXTURE2D(_SoilRoughness, sampler_SoilRoughness, uvX).rg * weights.x
                + SAMPLE_TEXTURE2D(_SoilRoughness, sampler_SoilRoughness, uvY).rg * weights.y
                + SAMPLE_TEXTURE2D(_SoilRoughness, sampler_SoilRoughness, uvZ).rg * weights.z;
            roughness = soilMask.r;
            occlusion = soilMask.g;

            // Texture-driven edge variation avoids a perfectly straight green
            // stripe. Turf belongs only to original surface-facing ground.
            half edge = SAMPLE_TEXTURE2D(_SoilAlbedo, sampler_SoilAlbedo, position.xz * 0.61).r;
            half turf = smoothstep(_SurfaceHeight - _TurfDepth, _SurfaceHeight - 0.025,
                position.y + (edge - 0.4) * 0.1) * smoothstep(0.48, 0.87, n.y);
            half3 grass = SAMPLE_TEXTURE2D(_TurfAlbedo, sampler_TurfAlbedo, turfUV).rgb;
            half3 grassTS = UnpackNormalScale(SAMPLE_TEXTURE2D(_TurfNormal, sampler_TurfNormal, turfUV), _TurfNormalStrength);
            half3 grassDetail = half3(grassTS.x, 0, grassTS.y) / max(grassTS.z, 0.25);
            grassDetail -= n * dot(grassDetail, n);
            normal = normalize(lerp(normal, normalize(n + grassDetail), turf));
            colour = lerp(colour, grass, turf);
            roughness = lerp(roughness, SAMPLE_TEXTURE2D(_TurfRoughness, sampler_TurfRoughness, turfUV).r, turf);
            occlusion = lerp(occlusion, 1, turf);
            // Broad variation comes from the authored soil texture at a second
            // incommensurate scale; it remains anchored when chunks regenerate.
            half macro = SAMPLE_TEXTURE2D(_SoilAlbedo, sampler_SoilAlbedo,
                (position.xz + position.y * float2(0.37, 0.23)) * 0.073).r;
            colour *= 1 + (macro - 0.47) * _MacroVariation * 3;
        }
        ENDHLSL

        Pass
        {
            Name "GroundForward"
            Tags { "LightMode"="UniversalForwardOnly" }
            HLSLPROGRAM
            #pragma target 3.5
            #pragma vertex GroundVertex
            #pragma fragment GroundFragment
            #pragma multi_compile_instancing
            #pragma multi_compile_fog
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT _SHADOWS_SOFT_LOW _SHADOWS_SOFT_MEDIUM _SHADOWS_SOFT_HIGH
            #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
            #pragma multi_compile _ _CLUSTER_LIGHT_LOOP
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            half4 GroundFragment(GroundVaryings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                half3 albedo, normal;
                half roughness, occlusion;
                GroundSurface(input.positionWS, input.normalWS, albedo, normal, roughness, occlusion);
                InputData lighting = (InputData)0;
                lighting.positionWS = input.positionWS;
                lighting.positionCS = input.positionCS;
                lighting.normalWS = normal;
                lighting.viewDirectionWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
                lighting.shadowCoord = TransformWorldToShadowCoord(input.positionWS);
                lighting.fogCoord = input.fogFactor;
                lighting.vertexLighting = VertexLighting(input.positionWS, normal);
                lighting.bakedGI = SampleSH(normal);
                lighting.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(input.positionCS);
                lighting.shadowMask = half4(1, 1, 1, 1);
                SurfaceData surface = (SurfaceData)0;
                surface.albedo = albedo;
                surface.normalTS = half3(0, 0, 1);
                surface.smoothness = 1 - roughness;
                surface.occlusion = occlusion;
                surface.alpha = 1;
                half4 result = UniversalFragmentPBR(lighting, surface);
                result.rgb = MixFog(result.rgb, input.fogFactor);
                return result;
            }
            ENDHLSL
        }
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode"="ShadowCaster" }
            ZWrite On ZTest LEqual ColorMask 0
            HLSLPROGRAM
            #pragma target 3.5
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment
            #pragma multi_compile_instancing
            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
        }
        Pass
        {
            Name "DepthOnly"
            Tags { "LightMode"="DepthOnly" }
            ZWrite On ColorMask R
            HLSLPROGRAM
            #pragma target 3.5
            #pragma vertex GroundVertex
            #pragma fragment DepthFragment
            #pragma multi_compile_instancing
            half DepthFragment(GroundVaryings input) : SV_Target { return input.positionCS.z; }
            ENDHLSL
        }
        Pass
        {
            Name "DepthNormals"
            Tags { "LightMode"="DepthNormalsOnly" }
            ZWrite On
            HLSLPROGRAM
            #pragma target 3.5
            #pragma vertex GroundVertex
            #pragma fragment NormalsFragment
            #pragma multi_compile_instancing
            #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
            half4 NormalsFragment(GroundVaryings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                half3 albedo, normal;
                half roughness, occlusion;
                GroundSurface(input.positionWS, input.normalWS, albedo, normal, roughness, occlusion);
                #if defined(_GBUFFER_NORMALS_OCT)
                    float2 oct = PackNormalOctQuadEncode(normal);
                    return half4(PackFloat2To888(saturate(oct * 0.5 + 0.5)), 0);
                #else
                    return half4(normal, 0);
                #endif
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
