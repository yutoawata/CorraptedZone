Shader "Custom/OutlineToon"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width (World)", Float) = 0.04


        _Color   ("Main Color", Color) = (1,1,1,1)
        _MainTex ("MainTex", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 200

        Pass
        {
            Cull Front
            ZWrite On

            CGPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f {
                float4 pos : SV_POSITION;
            };

            float4 _OutlineColor;
            float  _OutlineWidth;

            v2f vert (appdata v)
            {
                v2f o;

                // ワールド空間で法線方向に押し出す
                float3 nWS   = normalize(UnityObjectToWorldNormal(v.normal));
                float3 posWS = mul(unity_ObjectToWorld, v.vertex).xyz;

                // オブジェクトスケール補正（最大軸スケールで割る）
                float3 sx = float3(unity_ObjectToWorld._m00, unity_ObjectToWorld._m01, unity_ObjectToWorld._m02);
                float3 sy = float3(unity_ObjectToWorld._m10, unity_ObjectToWorld._m11, unity_ObjectToWorld._m12);
                float3 sz = float3(unity_ObjectToWorld._m20, unity_ObjectToWorld._m21, unity_ObjectToWorld._m22);
                float  s  = max(max(length(sx), length(sy)), length(sz));

                posWS += nWS * (_OutlineWidth / max(s, 1e-6));
                o.pos = UnityWorldToClipPos(float4(posWS, 1));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _OutlineColor; // アウトライン色
            }
            ENDCG
        }

        // 簡易的なToon
        Pass
        {
            Cull Back
            ZWrite On

            CGPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv     : TEXCOORD0;
            };

            struct v2f {
                float4 pos      : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float2 uv       : TEXCOORD1;
                float3 posWS    : TEXCOORD2;
            };

            sampler2D _MainTex; float4 _MainTex_ST; float _Emission;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos      = UnityObjectToClipPos(v.vertex);
                o.normalWS = UnityObjectToWorldNormal(v.normal);
                o.uv       = TRANSFORM_TEX(v.uv, _MainTex);
                o.posWS    = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 N = normalize(i.normalWS);

                float3 L = (_WorldSpaceLightPos0.w == 0.0)
                         ? normalize(_WorldSpaceLightPos0.xyz)
                         : normalize(_WorldSpaceLightPos0.xyz - i.posWS);

                float nl = saturate(dot(N, L));

                float t = (nl <= 0.1) ? 0.1 : (nl <= 0.5) ? 0.5 : (nl <= 0.7) ? 0.7 : 1.0;

                float4 baseCol = tex2D(_MainTex, i.uv) * _Color;
                return float4(baseCol.rgb * t, baseCol.a);
            }
            ENDCG
        }
    }

    FallBack Off
}
