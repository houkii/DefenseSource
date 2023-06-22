Shader "Custom/IndicatorShader" {
    Properties{
        _Radius("Radius", Range(0, 1)) = 0.5
        _Color("Color", Color) = (1,1,1,1)
        _CircleColor("CircleColor", Color) = (1,1,1,1)
        _MainTex("Texture", 2D) = "white" {}
        _InnerRadiusSize("InnerRadius", Range(0, 1)) = 0.1
    }

        SubShader{
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
                    float4 worldPos : TEXCOORD1;
                };

                float _Radius;
                float4 _Color;
                float4 _CircleColor;
                float4 _Positions[10];
                float _Ranges[10];
                sampler2D _MainTex;
                float _InnerRadiusSize;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.vertex.xy + 0.5;
                    o.worldPos = mul(unity_ObjectToWorld, v.pos);
                    return o;
                }

                float circle(float2 uv, float2 center, float radius) {
                    float2 dir = uv - center;
                    float r = length(dir);
                    float innerRadiusDistance = radius * _InnerRadiusSize;

                    if (r <= radius && r >= radius * _InnerRadiusSize)
                    {
                        float distanceRatio = (r - innerRadiusDistance) / (radius - innerRadiusDistance);
                        //float thickness = _Thickness * (sin(.25 * _Time.y * 2.0 * UNITY_PI) + 1.0) / 2.0;
                        return smoothstep(0, 1, distanceRatio);
                    }
                    else
                        return 0;
                }

                fixed4 frag(v2f i) : SV_Target {
                    float c = circle(i.uv, float2(1,1), _Radius);

                    for (int j = 0; j < 10; j++) {
                        if (_Positions[j].w != 0) {
                            float2 center = _Positions[j].xy + float2(0.5, 0.5);
                            c = max(c, circle(i.uv, center, _Ranges[j]));
                        }
                    }

                    float4 circleColorWithAlpha = _CircleColor;
                    circleColorWithAlpha.rgb *= circleColorWithAlpha.a;

                    float4 colorWithAlpha = _Color;
                    colorWithAlpha.rgb *= (1.0 - circleColorWithAlpha.a);

                    float4 result = circleColorWithAlpha + colorWithAlpha;

                    return lerp(_Color, result, c);
                }
                ENDCG
            }
        }
            FallBack "Diffuse"
}