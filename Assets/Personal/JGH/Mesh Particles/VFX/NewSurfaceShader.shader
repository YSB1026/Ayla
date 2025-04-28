Shader "TNTC/Disintegration2D" {
    Properties {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        // 디졸브 관련 속성
        _DissolveTexture ("Dissolve Texture", 2D) = "white" {}
        _DissolveColor ("Dissolve Border Color", Color) = (1,1,1,1)
        _DissolveBorder ("Dissolve Border Width", Range(0,0.5)) = 0.05
        
        // 분해 효과 관련 속성
        _Weight ("Disintegration Amount", Range(0,1)) = 0
        _Direction ("Direction", Vector) = (0,1,0,0)
        [HDR]_DisintegrationColor ("Particle Color", Color) = (1,1,1,1)
        _Glow ("Glow Intensity", float) = 1
        
        // 입자 관련 속성
        _ParticleTexture ("Particle Texture", 2D) = "white" {}
        _ParticleSize ("Particle Size", Range(0.01, 0.5)) = 0.1
        _ParticleAmount ("Particle Amount", Range(1, 50)) = 20
        
        // 흐름 관련 속성
        _FlowMap ("Flow Map (RG)", 2D) = "black" {}
        _FlowStrength ("Flow Strength", Range(0, 5)) = 1
        
        // 정렬을 위한 속성
        [PerRendererData] _ZWrite ("ZWrite", Float) = 0
        [PerRendererData] _Cutoff ("Alpha Cutoff", Range(0, 1)) = 0.5
        [MaterialToggle] PixelSnap ("Pixel Snap", Float) = 0
    }

    SubShader {
        Tags { 
            "Queue" = "Transparent" 
            "IgnoreProjector" = "True" 
            "RenderType" = "Transparent" 
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Cull Off
        Lighting Off
        ZWrite [_ZWrite]
        Blend One OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"
            
            // 속성 변수 선언
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            
            sampler2D _DissolveTexture;
            float4 _DissolveTexture_ST;
            float4 _DissolveColor;
            float _DissolveBorder;
            
            float _Weight;
            float4 _Direction;
            float4 _DisintegrationColor;
            float _Glow;
            
            sampler2D _ParticleTexture;
            float _ParticleSize;
            int _ParticleAmount;
            
            sampler2D _FlowMap;
            float4 _FlowMap_ST;
            float _FlowStrength;
            
            // 정점 입력 구조체
            struct appdata {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            // 프래그먼트 출력 구조체
            struct v2f {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
                float4 worldPos : TEXCOORD1;
            };
            
            // 난수 생성 함수
            float random(float2 uv) {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453123);
            }
            
            // 범위 재조정 함수
            float remap(float value, float from1, float to1, float from2, float to2) {
                return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
            }
            
            // 정점 셰이더
            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                
                #ifdef PIXELSNAP_ON
                o.vertex = UnityPixelSnap(o.vertex);
                #endif
                
                return o;
            }
            
            // 프래그먼트 셰이더
            fixed4 frag(v2f i) : SV_Target {
                // 메인 텍스처 샘플링
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                
                // 디졸브 텍스처 샘플링
                float dissolveValue = tex2D(_DissolveTexture, i.uv).r;
                
                // 디졸브 효과 적용
                float dissolveAmount = 2 * _Weight;
                
                // 디졸브 경계선 효과
                float borderMask = step(dissolveValue - dissolveAmount, _DissolveBorder) - 
                                  step(dissolveValue, dissolveAmount);
                
                // 원본 스프라이트에 경계선 효과 적용
                col.rgb += _DissolveColor.rgb * _Glow * borderMask;
                
                // 디졸브 알파 클리핑
                clip(dissolveValue - dissolveAmount);
                
                // 입자 효과 계산 (프래그먼트 셰이더에서만 일부 계산)
                if (_Weight > 0) {
                    // 디졸브 경계 근처에서 입자 스폰
                    float borderProximity = saturate(1 - abs(dissolveValue - dissolveAmount - _DissolveBorder * 0.5) / (_DissolveBorder * 2));
                    
                    // 흐름 맵을 활용한 입자 이동 방향 설정
                    float2 flowUV = TRANSFORM_TEX(i.worldPos.xy, _FlowMap);
                    float2 flowVector = (tex2D(_FlowMap, flowUV).rg * 2 - 1) * _FlowStrength;
                    
                    // 입자 효과를 위한 시간 변수
                    float time = _Time.y;
                    
                    // 입자 효과 누적
                    float particleEffect = 0;
                    
                    // 주변 픽셀에서 파티클 생성 (프래그먼트 단계에서 시뮬레이션)
                    for (int p = 0; p < _ParticleAmount; p++) {
                        // 파티클별 랜덤 시드
                        float2 seed = float2(p * 0.1, p * 0.2);
                        
                        // 파티클 시작 위치 계산 (디졸브 경계 주변)
                        float2 particleOrigin = i.uv + (random(seed) * 2 - 1) * _ParticleSize;
                        
                        // 파티클 이동 속도와 방향
                        float speed = random(seed + 0.5) * 0.5 + 0.5;
                        float2 particleDir = normalize(_Direction.xy + flowVector) * speed;
                        
                        // 시간에 따른 파티클 위치 업데이트
                        float2 particlePos = particleOrigin + particleDir * time * _Weight;
                        
                        // 현재 프래그먼트와 파티클 위치 사이의 거리
                        float dist = length(i.uv - particlePos);
                        
                        // 거리에 따른 파티클 강도 계산
                        float particleIntensity = saturate((_ParticleSize - dist) / _ParticleSize);
                        particleIntensity *= random(seed + 0.1) * borderProximity;
                        
                        // 파티클 효과 누적
                        particleEffect = max(particleEffect, particleIntensity);
                    }
                    
                    // 최종 색상에 입자 효과 블렌딩
                    col.rgb = lerp(col.rgb, _DisintegrationColor.rgb, particleEffect * _Weight);
                }
                
                // 알파 프리멀티플라이 적용
                col.rgb *= col.a;
                
                return col;
            }
            ENDCG
        }
    }
    
    Fallback "Sprites/Default"
}