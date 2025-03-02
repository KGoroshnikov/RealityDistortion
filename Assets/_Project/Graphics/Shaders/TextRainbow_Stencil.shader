Shader "Shader Graphs/TextRainbow1"
{
    Properties
    {
        [NoScaleOffset]_MainTex("MainTex", 2D) = "white" {}
        _Speed("Speed", Float) = 1
        _Scale("Scale", Float) = 3
        [ToggleUI]_GragType("GragType", Float) = 0
        _ClipThresold("ClipThresold", Range(0, 1)) = 0.5
        [ToggleUI]_NoGradient("NoGradient", Float) = 0
        _MainColor("MainColor", Color) = (0, 0, 0, 0)
        _StencilMask("Stencil mask", Int) = 0
        [HideInInspector]_StencilComp("Stencil Comparison", Float) = 8
        [HideInInspector]_Stencil("Stencil ID", Float) = 0
        [HideInInspector]_StencilOp("Stencil Operation", Float) = 0
        [HideInInspector]_StencilWriteMask("Stencil Write Mask", Float) = 255
        [HideInInspector]_StencilReadMask("Stencil Read Mask", Float) = 255
        [HideInInspector]_ColorMask("ColorMask", Float) = 15
        [HideInInspector]_ClipRect("ClipRect", Vector) = (0, 0, 0, 0)
        [HideInInspector]_UIMaskSoftnessX("UIMaskSoftnessX", Float) = 1
        [HideInInspector]_UIMaskSoftnessY("UIMaskSoftnessY", Float) = 1
        [HideInInspector]_AlphaClip("_AlphaClip", Float) = 0.5
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "Queue"="Transparent"
            // DisableBatching: <None>
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalCanvasSubTarget"
            "IgnoreProjector"="True"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
        Pass
        {
            Name "Default"
            Tags
            {
                // LightMode: <None>
            }
        
            // Render State
            Cull Off
        Blend One OneMinusSrcAlpha
        ZTest Always
        ZWrite Off
        ColorMask [_ColorMask]
        Stencil {
            Ref[_StencilMask]
            Comp Equal
        }
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 2.0
        #pragma vertex vert
        #pragma fragment frag
        
            // Keywords
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP
        #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            // GraphKeywords: <None>
        
            #define CANVAS_SHADERGRAPH
        
            // Defines
           #define _SURFACE_TYPE_TRANSPARENT 1
           #define ATTRIBUTES_NEED_NORMAL
           #define ATTRIBUTES_NEED_TEXCOORD0
           #define ATTRIBUTES_NEED_TEXCOORD1
           #define ATTRIBUTES_NEED_COLOR
           #define ATTRIBUTES_NEED_VERTEXID
           #define ATTRIBUTES_NEED_INSTANCEID
           #define VARYINGS_NEED_POSITION_WS
           #define VARYINGS_NEED_NORMAL_WS
           #define VARYINGS_NEED_TEXCOORD0
           #define VARYINGS_NEED_TEXCOORD1
           #define VARYINGS_NEED_COLOR
        
        #define REQUIRE_DEPTH_TEXTURE
        #define REQUIRE_NORMAL_TEXTURE
        
           #define SHADERPASS SHADERPASS_CUSTOM_UI
        #define _ALPHATEST_ON 1
        
           #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/DebugMipmapStreamingMacros.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"
        #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"
        
            // --------------------------------------------------
            // Structs and Packing
        
        
            struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 color : COLOR;
             float4 uv0 : TEXCOORD0;
             float4 uv1 : TEXCOORD1;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(ATTRIBUTES_NEED_INSTANCEID)
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
             uint vertexID : VERTEXID_SEMANTIC;
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float3 TimeParameters;
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 texCoord0;
             float4 texCoord1;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
        };
        struct VertexDescriptionInputs
        {
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 texCoord1 : INTERP1;
             float4 color : INTERP2;
             float3 positionWS : INTERP3;
             float3 normalWS : INTERP4;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
        };
        
            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.texCoord1.xyzw = input.texCoord1;
            output.color.xyzw = input.color;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.texCoord1 = input.texCoord1.xyzw;
            output.color = input.color.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            return output;
        }
        
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            //UGUI has no keyword for when a renderer has "bloom", so its nessecary to hardcore it here, like all the base UI shaders.
            half4 _TextureSampleAdd;
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float _Speed;
        float _Scale;
        float4 _MainTex_TexelSize;
        float _GragType;
        float _ClipThresold;
        float _NoGradient;
        float4 _MainColor;
        float _Stencil;
        float _StencilOp;
        float _StencilWriteMask;
        float _StencilReadMask;
        float _ColorMask;
        float4 _ClipRect;
        float _UIMaskSoftnessX;
        float _UIMaskSoftnessY;
        UNITY_TEXTURE_STREAMING_DEBUG_VARS;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        static Gradient _Gradient = {0,4,2,{float4(1,0,0,0),float4(1,0.9623313,0.4,0.3117723),float4(0,0.2537313,1,0.6441138),float4(1,0.2311321,0.9166874,1),float4(0,0,0,0),float4(0,0,0,0),float4(0,0,0,0),float4(0,0,0,0)},{float2(1,0),float2(1,1),float2(0,0),float2(0,0),float2(0,0),float2(0,0),float2(0,0),float2(0,0)}};
        
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        static Gradient _Grad2 = {0,5,2,{float4(0.9113957,1,0.1933962,0),float4(1,0.06212844,0.04245281,0.2176547),float4(0.8430129,0.0235849,1,0.4764782),float4(0.1084906,0.9174281,1,0.7499962),float4(0.1943086,1,0,1),float4(0,0,0,0),float4(0,0,0,0),float4(0,0,0,0)},{float2(1,0),float2(1,1),float2(0,0),float2(0,0),float2(0,0),float2(0,0),float2(0,0),float2(0,0)}};
        
        
            // Graph Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
            // Graph Functions
            
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        float2 Unity_GradientNoise_Deterministic_Dir_float(float2 p)
        {
            float x; Hash_Tchou_2_1_float(p, x);
            return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }
        
        void Unity_GradientNoise_Deterministic_float (float2 UV, float3 Scale, out float Out)
        {
            float2 p = UV * Scale.xy;
            float2 ip = floor(p);
            float2 fp = frac(p);
            float d00 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip), fp);
            float d01 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
            float d10 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
            float d11 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
            fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
            Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }
        
        void Unity_SampleGradientV1_float(Gradient Gradient, float Time, out float4 Out)
        {
            // convert to OkLab if we need perceptual color space.
            float3 color = lerp(Gradient.colors[0].rgb, LinearToOklab(Gradient.colors[0].rgb), Gradient.type == 2);
        
            [unroll]
            for (int c = 1; c < Gradient.colorsLength; c++)
            {
                float colorPos = saturate((Time - Gradient.colors[c - 1].w) / (Gradient.colors[c].w - Gradient.colors[c - 1].w)) * step(c, Gradient.colorsLength - 1);
                float3 color2 = lerp(Gradient.colors[c].rgb, LinearToOklab(Gradient.colors[c].rgb), Gradient.type == 2);
                color = lerp(color, color2, lerp(colorPos, step(0.01, colorPos), Gradient.type % 2)); // grad.type == 1 is fixed, 0 and 2 are blends.
            }
            color = lerp(color, OklabToLinear(color), Gradient.type == 2);
        
        #ifdef UNITY_COLORSPACE_GAMMA
            color = LinearToSRGB(color);
        #endif
        
            float alpha = Gradient.alphas[0].x;
            [unroll]
            for (int a = 1; a < Gradient.alphasLength; a++)
            {
                float alphaPos = saturate((Time - Gradient.alphas[a - 1].y) / (Gradient.alphas[a].y - Gradient.alphas[a - 1].y)) * step(a, Gradient.alphasLength - 1);
                alpha = lerp(alpha, Gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), Gradient.type % 2));
            }
        
            Out = float4(color, alpha);
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
            // Graph Vertex
            // GraphVertex: <None>
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreSurface' */
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float3 Emission;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Property_6bbf9e59b6554899b654fae8e563d28d_Out_0_Boolean = _NoGradient;
            float4 _Property_45a85da19c8e4d3b9afdc13012224559_Out_0_Vector4 = _MainColor;
            float _Property_18578a90fd1248caa653b449df9d3080_Out_0_Boolean = _GragType;
            Gradient _Property_98be5391d0ad4bbc9623851a6955682f_Out_0_Gradient = _Grad2;
            float _Property_444627fa18684939adc2e57da5355783_Out_0_Float = _Speed;
            float _Multiply_2a36951103734842951b59b7f63ef17c_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, _Property_444627fa18684939adc2e57da5355783_Out_0_Float, _Multiply_2a36951103734842951b59b7f63ef17c_Out_2_Float);
            float2 _TilingAndOffset_c3c1da45296e4972a4c645b8ceab7d50_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (_Multiply_2a36951103734842951b59b7f63ef17c_Out_2_Float.xx), _TilingAndOffset_c3c1da45296e4972a4c645b8ceab7d50_Out_3_Vector2);
            float _Property_85bda99c97994e3d84a1c61417df27ee_Out_0_Float = _Scale;
            float _GradientNoise_355326d06c7e437486c2fc03730582c5_Out_2_Float;
            Unity_GradientNoise_Deterministic_float(_TilingAndOffset_c3c1da45296e4972a4c645b8ceab7d50_Out_3_Vector2, _Property_85bda99c97994e3d84a1c61417df27ee_Out_0_Float, _GradientNoise_355326d06c7e437486c2fc03730582c5_Out_2_Float);
            float4 _SampleGradient_b6a1e707b9cd4e10bea0d7845c64920f_Out_2_Vector4;
            Unity_SampleGradientV1_float(_Property_98be5391d0ad4bbc9623851a6955682f_Out_0_Gradient, _GradientNoise_355326d06c7e437486c2fc03730582c5_Out_2_Float, _SampleGradient_b6a1e707b9cd4e10bea0d7845c64920f_Out_2_Vector4);
            Gradient _Property_9dfad6b7f28f4a2295211731200784dd_Out_0_Gradient = _Gradient;
            float4 _SampleGradient_c4c3e049e1a24262972bff18bd25439f_Out_2_Vector4;
            Unity_SampleGradientV1_float(_Property_9dfad6b7f28f4a2295211731200784dd_Out_0_Gradient, _GradientNoise_355326d06c7e437486c2fc03730582c5_Out_2_Float, _SampleGradient_c4c3e049e1a24262972bff18bd25439f_Out_2_Vector4);
            float4 _Branch_594bbbb5d1a546b4a005dfce2506f58d_Out_3_Vector4;
            Unity_Branch_float4(_Property_18578a90fd1248caa653b449df9d3080_Out_0_Boolean, _SampleGradient_b6a1e707b9cd4e10bea0d7845c64920f_Out_2_Vector4, _SampleGradient_c4c3e049e1a24262972bff18bd25439f_Out_2_Vector4, _Branch_594bbbb5d1a546b4a005dfce2506f58d_Out_3_Vector4);
            float4 _Branch_a2c9b19bd30e4081b22d4ac73b9c81dd_Out_3_Vector4;
            Unity_Branch_float4(_Property_6bbf9e59b6554899b654fae8e563d28d_Out_0_Boolean, _Property_45a85da19c8e4d3b9afdc13012224559_Out_0_Vector4, _Branch_594bbbb5d1a546b4a005dfce2506f58d_Out_3_Vector4, _Branch_a2c9b19bd30e4081b22d4ac73b9c81dd_Out_3_Vector4);
            UnityTexture2D _Property_e4a7dfe21802411188ffdf6f8a2a561f_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _UV_b1c7f105606844fdbb59143b2f74e5dd_Out_0_Vector4 = IN.uv0;
            float4 _SampleTexture2D_96023cb586824c94a0143d1b4f0002cb_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_e4a7dfe21802411188ffdf6f8a2a561f_Out_0_Texture2D.tex, _Property_e4a7dfe21802411188ffdf6f8a2a561f_Out_0_Texture2D.samplerstate, _Property_e4a7dfe21802411188ffdf6f8a2a561f_Out_0_Texture2D.GetTransformedUV((_UV_b1c7f105606844fdbb59143b2f74e5dd_Out_0_Vector4.xy)) );
            float _SampleTexture2D_96023cb586824c94a0143d1b4f0002cb_R_4_Float = _SampleTexture2D_96023cb586824c94a0143d1b4f0002cb_RGBA_0_Vector4.r;
            float _SampleTexture2D_96023cb586824c94a0143d1b4f0002cb_G_5_Float = _SampleTexture2D_96023cb586824c94a0143d1b4f0002cb_RGBA_0_Vector4.g;
            float _SampleTexture2D_96023cb586824c94a0143d1b4f0002cb_B_6_Float = _SampleTexture2D_96023cb586824c94a0143d1b4f0002cb_RGBA_0_Vector4.b;
            float _SampleTexture2D_96023cb586824c94a0143d1b4f0002cb_A_7_Float = _SampleTexture2D_96023cb586824c94a0143d1b4f0002cb_RGBA_0_Vector4.a;
            float _Property_4468b3634845458888e7d766d477253d_Out_0_Float = _ClipThresold;
            surface.BaseColor = (_Branch_a2c9b19bd30e4081b22d4ac73b9c81dd_Out_3_Vector4.xyz);
            surface.Alpha = _SampleTexture2D_96023cb586824c94a0143d1b4f0002cb_A_7_Float;
            surface.Emission = float3(0, 0, 0);
            surface.AlphaClipThreshold = _Property_4468b3634845458888e7d766d477253d_Out_0_Float;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorCopyToSDI' */
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
            output.uv0 =                                        input.texCoord0;
        #if UNITY_ANY_INSTANCING_ENABLED
        #else // TODO: XR support for procedural instancing because in this case UNITY_ANY_INSTANCING_ENABLED is not defined and instanceID is incorrect.
        #endif
            output.TimeParameters =                             _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
            return output;
        }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/CanvasPass.hlsl"
        
            ENDHLSL
        }
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    FallBack "Hidden/Shader Graph/FallbackError"
}