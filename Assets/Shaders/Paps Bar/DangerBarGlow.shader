Shader "UI/DangerBarGlow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FillAmount ("Fill Amount", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _FillAmount;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float fill = saturate(_FillAmount); // Clamp between 0 and 1

                // Color: from light red to full red
                float3 baseColor = lerp(float3(1.0, 0.4, 0.4), float3(1.0, 0.0, 0.0), fill);

                // Glow effect: stronger alpha with fill
                float alpha = lerp(0.3, 1.0, fill);

                return fixed4(baseColor, alpha);
            }
            ENDCG
        }
    }
}
