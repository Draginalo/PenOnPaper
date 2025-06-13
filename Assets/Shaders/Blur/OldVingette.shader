Shader "PostProcessing/Vignette1"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Extent("Vignette extent", Float) = 0
        _Strength("Vignette strength", Float) = 1
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque" 
            "RenderPipeline"="UniversalPipeline"
        }

        HLSLINCLUDE

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        sampler2D _MainTex;

        CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_TexelSize;
            float _Extent;
            float _Strength;
        CBUFFER_END

        struct appdata
        {
            float4 in_pos : Position;
            float2 uv : TEXCOORD0;
        };

        struct v2f
        {
            float4 out_pos : SV_Position;
            float2 uv : TEXCOORD0;
        };

        v2f vert(appdata v)
        {
            v2f o;
            o.out_pos = TransformObjectToHClip(v.in_pos.xyz);
            o.uv = v.uv;
            return o;
        }

        ENDHLSL

        Pass
        {
            Name "Horizontal"

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag_horizontal

            float4 frag_horizontal(v2f i) : SV_Target
            {
                float2 newUV = i.uv * (1.0f - i.uv.yx);

                float vignette = newUV.x * newUV.y * _Strength;
                vignette = min(pow(vignette, _Extent), 1);

                float3 color = tex2D(_MainTex, i.uv).xyz;

                return float4(color * vignette, 1.0f);
            }

            ENDHLSL
        }
    }
}
