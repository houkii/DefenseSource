Shader "Custom/FloorShader" {
    Properties{
        _Radius("Radius", Range(0, 1)) = 0.5
        _Color("Color", Color) = (1,1,1,1)
        _CircleColor("CircleColor", Color) = (1,1,1,1)
        _GlowAmount("GlowAmount", Range(0,1)) = 0.5
        _GlowSpeed("GlowSpeed", Range(0, 10)) = 5
        _Size("Size", Range(0,1)) = 0.1
    }

        SubShader{
        Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                    float4 pos : TEXCOORD1;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                float4 _Color;
                float4 _CircleColor;
                float4 _Positions[256]; 
                float _Radius;
                float _Ranges[256];
                float _GlowAmount;
                float _GlowSpeed;
                float _Size;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.vertex.xy + 0.5;
                    return o;
                }

                float CircleEnvelope(float2 position, float radius, float2 pos) {
                    float dist = distance(position, pos);

                    if (dist < radius)
                        return lerp(0,1,((radius - dist) / radius));
                    else 
                        return 0;
                }

                fixed4 frag(v2f i) : SV_Target{
                    float c = 0;
                    for (int j = 0; j < 256; j++) {
                        if (_Positions[j].w != 0) {
                            float2 center = _Positions[j].xy + float2(0.5, 0.5);
                            c = max(c, CircleEnvelope(center, _Ranges[j], i.uv));
                        }
                    }

                    if (c > _Size)
                        c = 0;
                    else if (c > 0)
                        c = lerp(0, 1, c / _Size);

                    float4 circleColorWithAlpha = _CircleColor;
                    float alpha = circleColorWithAlpha.a + (circleColorWithAlpha.a * _GlowAmount * sin(_Time.y * UNITY_PI * 2 * _GlowSpeed));
                    circleColorWithAlpha.rgb *= alpha;

                    float4 colorWithAlpha = _Color;
                    colorWithAlpha.rgb *= (1.0 - alpha);

                    float4 result = circleColorWithAlpha + colorWithAlpha;

                    return lerp(_Color, result, c);
                }
                ENDCG
            }
    }
        FallBack "Diffuse"
}