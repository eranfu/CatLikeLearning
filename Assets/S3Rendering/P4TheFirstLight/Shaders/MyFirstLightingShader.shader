﻿Shader "Custom/MyFirstLightingShader" {
    Properties {
        _Tint("Tint", Color) = (1, 1, 1, 1)
        _MainTex("Albedo", 2D) = "white" {}
        _SpecularTint("Specular", Color) = (0.5, 0.5, 0.5)
        _Smoothness("Smoothness", Range(0, 1)) = 0.5
    }

    SubShader {
        Pass {
            Tags {
                "LightMode" = "ForwardBase"
            }

            CGPROGRAM

            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram

            #include "UnityStandardBRDF.cginc"

            struct VertexData {                
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct Interpolators {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float4 worldPos : TEXCOORD2;
            };

            float4 _Tint;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float3 _SpecularTint;
            float _Smoothness;

            void MyVertexProgram(VertexData v, out Interpolators i) {
                i.position = UnityObjectToClipPos(v.position);
                i.worldPos = mul(unity_ObjectToWorld, v.position);
                i.uv = TRANSFORM_TEX(v.uv, _MainTex);
                i.normal = UnityObjectToWorldNormal(v.normal);
            }

            void MyFragmentProgram(Interpolators i, out float4 color : SV_TARGET) {
                i.normal = normalize(i.normal);
                float3 lightDir = _WorldSpaceLightPos0.xyz;
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                float3 halfVector = normalize(lightDir + viewDir);
                
                float3 lightColor = _LightColor0.rgb;
                float3 albedo = tex2D(_MainTex, i.uv);
                float3 diffuse = albedo * lightColor * DotClamped(lightDir, i.normal);
                float3 specular = _SpecularTint * lightColor * pow(DotClamped(halfVector, i.normal), _Smoothness * 100);

                color = float4(specular, 1);
            }

            ENDCG
        }
    }
}