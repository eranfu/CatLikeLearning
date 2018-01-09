Shader "Custom/MyFirstShader"
{
    Properties
    {
        _Tint("Tint", Color) = (1, 1, 1, 1)
        _MainTex("Texture", 2D) = "white" {}
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
            };

            float4 _Tint;
            sampler2D _MainTex;
            float4 _MainTex_ST;

            void MyVertexProgram(inout Interpolators io)
            {
                io.position = UnityObjectToClipPos(io.position);
                io.uv = TRANSFORM_TEX(io.uv, _MainTex);
            }

            float4 MyFragmentProgram(Interpolators i) : SV_TARGET
            {
                return tex2D(_MainTex, i.uv) * _Tint;
            }

            ENDCG
        }
    }
}
