Shader "Custom/GlowingShader" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _BaseColor("Base Color", Color) = (1,1,1,1)
        _GlowColor("Glow Color", Color) = (1,1,1,1)
        _GlowAmount("Glow Amount", Range(0,1)) = 0.5
        _GlowSpeed("Glow Speed", Range(0,10)) = 1
        _MinGlow("Min Glow Amount", Range(0,1)) = 0
    }

        SubShader{
            Tags {"RenderType" = "Opaque"}
            LOD 100

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float _GlowAmount;
                float _GlowSpeed;
                float _MinGlow;
                float4 _GlowColor;
                float4 _BaseColor;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    float4 texColor = tex2D(_MainTex, i.uv);
                    float4 glowColor = _GlowColor * _GlowAmount;
                    float glowValue = _MinGlow + abs(sin(_Time.y * _GlowSpeed)) * (1 - _MinGlow);
                    float4 finalColor = (1 - glowValue) * _BaseColor + glowColor * glowValue;
                    finalColor.a = 1;
                    return finalColor;
                }
                ENDCG
            }
        }
            FallBack "Diffuse"
}