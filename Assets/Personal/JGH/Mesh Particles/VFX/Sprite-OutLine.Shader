Shader "Sprites/Outline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0

        // Outline 설정을 Material에서 직접 수정할 수 있도록 변경
        _Outline("Outline Enabled", Float) = 0
        _OutlineColor("Outline Color", Color) = (1,1,1,1)
        _OutlineSize("Outline Size", Range(1,10)) = 1
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"

            float _Outline;
            fixed4 _OutlineColor;
            float _OutlineSize; // int -> float로 바꿔야 Range 슬라이더 사용 가능
            float4 _MainTex_TexelSize;

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;

                if (_Outline > 0 && c.a != 0) {
                    float totalAlpha = 1.0;

                    [unroll(16)]
                    for (int i = 1; i <= _OutlineSize; i++) {
                        fixed4 pixelUp = tex2D(_MainTex, IN.texcoord + fixed2(0, i * _MainTex_TexelSize.y));
                        fixed4 pixelDown = tex2D(_MainTex, IN.texcoord - fixed2(0,i *  _MainTex_TexelSize.y));
                        fixed4 pixelRight = tex2D(_MainTex, IN.texcoord + fixed2(i * _MainTex_TexelSize.x, 0));
                        fixed4 pixelLeft = tex2D(_MainTex, IN.texcoord - fixed2(i * _MainTex_TexelSize.x, 0));

                        totalAlpha *= pixelUp.a * pixelDown.a * pixelRight.a * pixelLeft.a;
                    }

                    if (totalAlpha == 0) {
                        c = _OutlineColor;
                        c.a = 1.0;
                    }
                }

                c.rgb *= c.a;

                return c;
            }
        ENDCG
        }
    }
}
