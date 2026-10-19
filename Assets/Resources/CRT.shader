Shader "Custom/CRT"
{
    HLSLINCLUDE
    #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
    TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
    TEXTURE2D_SAMPLER2D(_LUT, sampler_LUT);
    float _Blend;
    int _ScaleX;
    int _ScaleY;


    float4 Frag(VaryingsDefault i) : SV_Target
    {
        float2 uv = floor(float2(i.texcoord.x * _ScaleX, i.texcoord.y * _ScaleY));
        uv = float2(uv.x / _ScaleX, uv.y / _ScaleY);
        float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);



        float2 LUT_uv = float2(i.texcoord.x * _ScaleX % 2 / 2, i.texcoord.y * _ScaleY % 2 / 2);
        float4 LUT = SAMPLE_TEXTURE2D(_LUT, sampler_LUT, LUT_uv);






        //return color * (1 - _Blend) + LUT * _Blend;
        return lerp(color, color*LUT,_Blend);
    }

    ENDHLSL
    SubShader
    {
        Cull Off ZWrite Off ZTest Always
            Pass
        {
            HLSLPROGRAM
                #pragma vertex VertDefault
                #pragma fragment Frag
            ENDHLSL
        }
    }
}