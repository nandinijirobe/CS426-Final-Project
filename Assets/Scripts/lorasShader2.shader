Shader "ShaderTest/URP_GlowOutlineOnly"
{
    Properties
    {
        _BaseMap("Base Texture", 2D) = "white" {}
        _GlowColor("Glow Color", Color) = (1, 0.84, 0, 1)
        _GlowSize("Glow Size", Range(0, 0.1)) = 0.05
        _GlowIntensity("Glow Intensity", Range(0, 5)) = 2
        _PulseSpeed("Pulse Speed", Range(0, 10)) = 2
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent+1" }
        LOD 200
        Cull Front
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        // Glow Outline Pass
        Pass
        {
            Name "GlowOutline"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            float4 _GlowColor;
            float _GlowSize;
            float _GlowIntensity;
            float _PulseSpeed;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float3 normalWS = TransformObjectToWorldNormal(IN.normalOS);
                float3 positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                positionWS += normalWS * _GlowSize;
                OUT.positionHCS = TransformWorldToHClip(positionWS);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float pulse = (sin(_Time.y * _PulseSpeed) + 1.0) * 0.5;
                return _GlowColor * (_GlowIntensity * pulse);
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/InternalErrorShader"
}
