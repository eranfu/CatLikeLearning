Shader "Custom/TexturedWithDetail"
{
    Properties
    {
        _Tint("Tint", Color) = (1, 1, 1, 1)
        _MainTex("Texture", 2D) = "white" {}
        _DetailTex("Detail Texture", 2D) = "gray" {}
    }

    SubShader
    {
        Pass
        {
            CGPROGRAM

            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram

            #include "UnityCG.cginc"

            struct Interpolators
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
                float2 uvDetail : TEXCOORD1;
            };

            float4 _Tint;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _DetailTex;
            float4 _DetailTex_ST;

            void MyVertexProgram(inout Interpolators io)
            {
                io.position = UnityObjectToClipPos(io.position);
                io.uv = TRANSFORM_TEX(io.uv, _MainTex);
                io.uvDetail = TRANSFORM_TEX(io.uvDetail, _DetailTex);
            }

            void MyFragmentProgram(in Interpolators i, out float4 color : SV_TARGET)
            {
                color = tex2D(_MainTex, i.uv) * _Tint;
                color *= tex2D(_DetailTex, i.uvDetail) * unity_ColorSpaceDouble;
            }

            ENDCG
        }
    }
}
