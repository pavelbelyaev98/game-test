Shader "Something Down There/Ground Triplanar"
{
    Properties
    {
        _SoilAlbedo("Soil colour", 2D) = "white" {}
        [Normal] _SoilNormal("Soil normal", 2D) = "bump" {}
        _SoilRoughness("Soil roughness (R), contact (G), stone coverage (B)", 2D) = "white" {}
        _TurfAlbedo("Turf colour", 2D) = "white" {}
        [Normal] _TurfNormal("Turf normal", 2D) = "bump" {}
        _TurfRoughness("Turf roughness (R), blade contact (G)", 2D) = "white" {}
        _TileMetres("Turf tile metres", Float) = 1
        _SoilTileMetres("Soil tile metres", Float) = 2
        _NormalStrength("Soil relief", Range(0, 2)) = 0.8
        _StoneNormalStrength("Embedded stone relief", Range(0, 2)) = 0.85
        _TurfNormalStrength("Turf relief", Range(0, 2)) = 0.45
        _SurfaceHeight("Original surface height", Float) = 0
        _TurfDepth("Turf transition depth", Range(0.01, 0.1)) = 0.035
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
            float _StoneNormalStrength;
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
        #include "../../Runtime/Terrain/ExcavationDaylight.hlsl"

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

        // Derivatives come from continuous world position, never the changing
        // projection sign. Sign boundaries must not select an unrelated coarse mip.
        #define GROUND_SAMPLE(tex, uv, dx, dy) SAMPLE_TEXTURE2D_GRAD(tex, sampler##tex, uv, dx, dy)

        half3 ProjectGroundNormal(half3 n, half3 weights, half3 axisSign,
            half3 nx, half3 ny, half3 nz)
        {
            half3 dx = half3(0, nx.y, nx.x * axisSign.x) / max(nx.z, 0.25);
            half3 dy = half3(ny.x * axisSign.y, 0, ny.y) / max(ny.z, 0.25);
            half3 dz = half3(-nz.x * axisSign.z, nz.y, 0) / max(nz.z, 0.25);
            half3 detail = dx * weights.x + dy * weights.y + dz * weights.z;
            return normalize(n + detail - n * dot(detail, n));
        }

        void GroundSurface(float3 position, half3 geometricNormal,
            out half3 colour, out half3 normal, out half roughness, out half occlusion)
        {
            half3 n = normalize(geometricNormal);
            half3 axis = abs(n);
            half bestAxis = max(axis.x, max(axis.y, axis.z));
            // Exclude stretched grazing projections from the soil blend.
            half3 weights = smoothstep(bestAxis - 0.16, bestAxis, axis);
            weights /= max(dot(weights, 1.0), 0.0001);
            half3 turfWeights = weights;
            float3 p = position / max(_SoilTileMetres, 0.05);
            float3 positionDx = ddx(position), positionDy = ddy(position);
            float3 pDx = positionDx / max(_SoilTileMetres, 0.05);
            float3 pDy = positionDy / max(_SoilTileMetres, 0.05);
            // One world-space origin across chunks and rim meshes. No mesh UVs
            // or tangents: fresh vertical cuts keep the same physical texel scale.
            half3 axisSign = half3(n.x < 0 ? -1 : 1, n.y < 0 ? -1 : 1, n.z < 0 ? -1 : 1);
            float2 uvX = float2(p.z * axisSign.x, p.y);
            float2 uvY = float2(p.x * axisSign.y, p.z);
            float2 uvZ = float2(-p.x * axisSign.z, p.y);
            float2 dxX = float2(pDx.z * axisSign.x, pDx.y), dyX = float2(pDy.z * axisSign.x, pDy.y);
            float2 dxY = float2(pDx.x * axisSign.y, pDx.z), dyY = float2(pDy.x * axisSign.y, pDy.z);
            float2 dxZ = float2(-pDx.x * axisSign.z, pDx.y), dyZ = float2(-pDy.x * axisSign.z, pDy.y);
            half3 cx = GROUND_SAMPLE(_SoilAlbedo, uvX, dxX, dyX).rgb;
            half3 cy = GROUND_SAMPLE(_SoilAlbedo, uvY, dxY, dyY).rgb;
            half3 cz = GROUND_SAMPLE(_SoilAlbedo, uvZ, dxZ, dyZ).rgb;
            half3 maskX = GROUND_SAMPLE(_SoilRoughness, uvX, dxX, dyX).rgb;
            half3 maskY = GROUND_SAMPLE(_SoilRoughness, uvY, dxY, dyY).rgb;
            half3 maskZ = GROUND_SAMPLE(_SoilRoughness, uvZ, dxZ, dyZ).rgb;
            // Coverage is a material boundary, not a transparent overlay. The
            // former multiplicative weighting still diluted whole stone faces
            // with another plane's dirt. Height-select one coherent material
            // through overlaps; reserve blending for its narrow filtered edge.
            float3 stoneCoverage = float3(maskX.b, maskY.b, maskZ.b);
            float3 eligible = step(bestAxis - 0.16, axis);
            float3 priority = axis + stoneCoverage * 0.6 - (1 - eligible) * 2;
            float highest = max(priority.x, max(priority.y, priority.z));
            float blendWidth = clamp(fwidth(highest), 0.008, 0.025);
            float3 mineralWeights = max(priority - highest + blendWidth, 0);
            mineralWeights /= max(dot(mineralWeights, 1.0), 0.0001);
            float3 covered = stoneCoverage * eligible;
            float mineral = smoothstep(0.15, 0.65, max(covered.x, max(covered.y, covered.z)));
            weights = lerp(weights, mineralWeights, mineral);
            colour = cx * weights.x + cy * weights.y + cz * weights.z;
            // Authored B coverage gives stones their own relief without
            // amplifying the accepted soil grain or adding texture lookups.
            half3 nx = UnpackNormalScale(GROUND_SAMPLE(_SoilNormal, uvX, dxX, dyX), lerp(_NormalStrength, _StoneNormalStrength, maskX.b));
            half3 ny = UnpackNormalScale(GROUND_SAMPLE(_SoilNormal, uvY, dxY, dyY), lerp(_NormalStrength, _StoneNormalStrength, maskY.b));
            half3 nz = UnpackNormalScale(GROUND_SAMPLE(_SoilNormal, uvZ, dxZ, dyZ), lerp(_NormalStrength, _StoneNormalStrength, maskZ.b));
            // Surface-gradient projection: a flat normal map reproduces the
            // density-gradient mesh normal exactly, including blended slopes.
            normal = ProjectGroundNormal(n, weights, axisSign, nx, ny, nz);
            half2 soilMask = maskX.rg * weights.x + maskY.rg * weights.y + maskZ.rg * weights.z;
            roughness = soilMask.r;
            occlusion = soilMask.g;

            // The turf cap continues a short way down fresh lips. The authored
            // leaf pattern supplies ragged tips; height excludes deeper walls,
            // while the facing term excludes ceilings rather than all slopes.
            float depth = max(0, _SurfaceHeight - position.y);
            // Reuse the same three world planes for turf. The old top-only UVs
            // stretched leaf shapes into stripes down even a shallow cut lip.
            // All explicit gradients are available before this depth branch.
            float edgeWidth = max(0.00075, (abs(positionDx.y) + abs(positionDy.y)) * 0.65);
            if (depth < _TurfDepth + 0.02)
            {
                float turfScale = max(_SoilTileMetres, 0.05) / max(_TileMetres, 0.05);
                half3 grassX = GROUND_SAMPLE(_TurfAlbedo, uvX * turfScale, dxX * turfScale, dyX * turfScale).rgb;
                half3 grassY = GROUND_SAMPLE(_TurfAlbedo, uvY * turfScale, dxY * turfScale, dyY * turfScale).rgb;
                half3 grassZ = GROUND_SAMPLE(_TurfAlbedo, uvZ * turfScale, dxZ * turfScale, dyZ * turfScale).rgb;
                half3 grass = grassX * turfWeights.x + grassY * turfWeights.y + grassZ * turfWeights.z;
                half leaf = saturate((grass.g - grass.r * 0.7) * 3.5);
                half drift = GROUND_SAMPLE(_SoilAlbedo, position.xz * 0.61, positionDx.xz * 0.61, positionDy.xz * 0.61).r;
                float fringeDepth = _TurfDepth * (0.45 + leaf * 0.55) + (drift - 0.35) * 0.012;
                // Filter just the visible boundary rather than a fixed 2 cm colour
                // fade. Pixel derivatives keep the narrow edge stable at distance.
                float edge = depth - fringeDepth;
                half turf = (1 - smoothstep(-edgeWidth, edgeWidth, edge))
                    * smoothstep(-0.2, -0.05, n.y);
                // Darker roots give the thin living cap thickness without drawing a
                // constant black outline around cuts. Flat lawn stays unchanged.
                grass *= lerp(1, 0.82, saturate(depth / max(fringeDepth, 0.01)) * (1 - saturate(n.y)));
                half3 gx = UnpackNormalScale(GROUND_SAMPLE(_TurfNormal, uvX * turfScale, dxX * turfScale, dyX * turfScale), _TurfNormalStrength);
                half3 gy = UnpackNormalScale(GROUND_SAMPLE(_TurfNormal, uvY * turfScale, dxY * turfScale, dyY * turfScale), _TurfNormalStrength);
                half3 gz = UnpackNormalScale(GROUND_SAMPLE(_TurfNormal, uvZ * turfScale, dxZ * turfScale, dyZ * turfScale), _TurfNormalStrength);
                normal = normalize(lerp(normal, ProjectGroundNormal(n, turfWeights, axisSign, gx, gy, gz), turf));
                colour = lerp(colour, grass, turf);
                half2 turfMask = GROUND_SAMPLE(_TurfRoughness, uvX * turfScale, dxX * turfScale, dyX * turfScale).rg * turfWeights.x
                    + GROUND_SAMPLE(_TurfRoughness, uvY * turfScale, dxY * turfScale, dyY * turfScale).rg * turfWeights.y
                    + GROUND_SAMPLE(_TurfRoughness, uvZ * turfScale, dxZ * turfScale, dyZ * turfScale).rg * turfWeights.z;
                roughness = lerp(roughness, turfMask.r, turf);
                occlusion = lerp(occlusion, lerp(0.65, 1, turfMask.g), turf);
            }
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
                surface.occlusion = occlusion * ExcavationAmbient(input.positionWS, normalize(input.normalWS));
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
