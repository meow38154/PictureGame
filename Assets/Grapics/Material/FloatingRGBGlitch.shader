
Shader "Custom/FloatingRGBGlitch"
{
    Properties
    {
        _Color ("Tint", Color) = (1,1,1,1)
        _Density ("Line Density", Range(1,50)) = 15
        _Thickness ("Line Thickness", Range(0.001,0.15)) = 0.015
        _GlitchSpeed ("Glitch Speed", Range(0,30)) = 12
        _BlinkChance ("Blink Chance", Range(0,1)) = 0.4
        _FragmentChance ("Fragment Chance", Range(0,1)) = 0.35
        _GlobalBlink ("Global Blink Chance", Range(0,1)) = 0.15
        _Seed ("Random Seed", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "UniversalMaterialType" = "Unlit"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            Name "Glitch"
            Tags { "LightMode" = "Universal2D" }

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                half4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                half4 color : COLOR;
            };

            CBUFFER_START(UnityPerMaterial)
                half4 _Color;
                float _Density;
                float _Thickness;
                float _GlitchSpeed;
                float _BlinkChance;
                float _FragmentChance;
                float _GlobalBlink;
                float _Seed;
            CBUFFER_END

            float Hash21(float2 p)
            {
                float3 p3 = frac(float3(p.xyx) * 0.1031);
                p3 += dot(p3, p3.yzx + 33.33);
                return frac((p3.x + p3.y) * p3.z);
            }


            float RandomValue(float row, float frame, float salt)
            {
                float randomSeed = Hash21(
                    float2(
                        _Seed + floor(frame * 0.731),
                        floor(frame * 1.379) + salt * 17.17
                    )
                ) * 10000.0;

                return Hash21(
                    float2(
                        row * 17.17 + salt * 13.31 + randomSeed,
                        frame * 23.71 + salt * 7.97 + randomSeed
                    )
                );
            }


            Varyings vert(Attributes v)
            {
                Varyings o;

                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                o.color = v.color;

                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float frame = floor(
                    _Time.y * max(_GlitchSpeed, 0.001)
                );

                float row = floor(i.uv.y * _Density);
                float localY = frac(i.uv.y * _Density);

                float center = lerp(
                    0.15,
                    0.85,
                    RandomValue(row, frame, 1.0)
                );

                float thickness = _Thickness * _Density;

                float lineValue = 1.0 - smoothstep(
                    thickness,
                    thickness + 0.015,
                    abs(localY - center)
                );

                float start = RandomValue(row, frame, 2.0) * 0.8;

                float length = lerp(
                    0.03,
                    0.65,
                    RandomValue(row, frame, 3.0)
                );

                float segment =
                    step(start, i.uv.x) *
                    step(i.uv.x, start + length);

                float fragmentRandom = RandomValue(
                    row,
                    frame,
                    4.0
                );

                if (fragmentRandom < _FragmentChance)
                {
                    float fragmentCount = floor(
                        lerp(
                            2.0,
                            6.0,
                            RandomValue(row, frame, 5.0)
                        )
                    );

                    float fragmentIndex = floor(
                        i.uv.x * fragmentCount
                    );

                    float fragmentMask = step(
                        0.35,
                        RandomValue(
                            row + fragmentIndex * 31.0,
                            frame,
                            6.0
                        )
                    );

                    segment *= fragmentMask;
                }

                float active = step(
                    _BlinkChance,
                    RandomValue(row, frame, 7.0)
                );

                float globalActive = step(
                    _GlobalBlink,
                    RandomValue(0.0, frame, 8.0)
                );

                float colorRandom = RandomValue(
                    row,
                    frame,
                    9.0
                );

                float3 rgb;

                if (colorRandom < 0.3333)
                {
                    rgb = float3(1, 0, 0);
                }
                else if (colorRandom < 0.6666)
                {
                    rgb = float3(0, 1, 0);
                }
                else
                {
                    rgb = float3(0, 0, 1);
                }

                float alpha =
                    lineValue *
                    segment *
                    active *
                    globalActive *
                    _Color.a *
                    i.color.a;

                return half4(rgb, alpha);
            }

            ENDHLSL
        }
    }

    FallBack Off
}
