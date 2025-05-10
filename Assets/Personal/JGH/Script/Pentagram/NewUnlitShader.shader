Shader "Custom/PentagramRevealGlow"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Progress ("Reveal Progress", Range(0,1)) = 0
        _GlowPower ("Glow Power", Float) = 3.0
        _TwinkleSpeed ("Twinkle Speed", Float) = 3.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Progress;
            float _GlowPower;
            float _TwinkleSpeed;

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
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
{
    fixed4 col = tex2D(_MainTex, i.uv);

    // Reveal 계산
    float reveal = smoothstep(0.0, 1.0, i.uv.y - (1.0 - _Progress));

    // 중앙 glow 계산
    float2 center = float2(0.5, 0.5);
    float dist = distance(i.uv, center);
    float glowFactor = 1.0 - dist;
    glowFactor *= _GlowPower;

    // 반짝임
    float twinkle = sin(_Time.y * _TwinkleSpeed + dist * 20.0) * 0.5 + 0.5;
    glowFactor *= twinkle;

    // 항상 발광하되, 알파만 Reveal에 따라 적용
    fixed3 glowColor = col.rgb * glowFactor;
    return fixed4(glowColor, col.a * reveal);
}

            ENDCG
        }
    }
}
