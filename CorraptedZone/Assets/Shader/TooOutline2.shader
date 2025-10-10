Shader "Custom/OutlineToon2"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width (World)", Float) = 0.04

        _Color         ("Main Color", Color) = (1,1,1,1)
        _MainTex       ("MainTex", 2D) = "white" {}

        _DisoleveTex("NoiseTex", 2D) = "white"{}
        _Threshold("Threshold", Range(0, 1)) = 0.0

        _Steps         ("Toon Steps (>=2)", Float) = 3
        _Emission      ("Emission", Float) = 0
        _EmissionColor ("Emission Color", Color) = (1,1,1,1)

        _NLPower("Lambert Power (>= 1 tighter)", Float) = 1
        _AttenPower("Attenuation Power ( >= 1 tighter)", Float) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        // ===== Pass 1: Outline（ワールド空間押し出し＋スケール補正） =====
        Pass
        {
            Cull Front
            ZWrite On

            CGPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata { float4 vertex:POSITION; float3 normal:NORMAL; };
            struct v2f     { float4 pos:SV_POSITION; };

            float4 _OutlineColor;
            float  _OutlineWidth;

            v2f vert (appdata v)
            {
                v2f o;
                float3 nWS   = normalize(UnityObjectToWorldNormal(v.normal));
                float3 posWS = mul(unity_ObjectToWorld, v.vertex).xyz;

                // オブジェクトの最大軸スケールで補正
                float3 sx = float3(unity_ObjectToWorld._m00, unity_ObjectToWorld._m01, unity_ObjectToWorld._m02);
                float3 sy = float3(unity_ObjectToWorld._m10, unity_ObjectToWorld._m11, unity_ObjectToWorld._m12);
                float3 sz = float3(unity_ObjectToWorld._m20, unity_ObjectToWorld._m21, unity_ObjectToWorld._m22);
                float  s  = max(max(length(sx), length(sy)), length(sz));

                posWS += nWS * (_OutlineWidth / max(s, 1e-6));
                o.pos  = UnityWorldToClipPos(float4(posWS,1));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target { return _OutlineColor; }
            ENDCG
        }

        // ===== 本体：Surface Shader（Toon + Emission） =====
        CGPROGRAM
        #pragma surface surf ToonLit fullforwardshadows addshadow
        #pragma target 3.0

        sampler2D _MainTex; 
        float4    _Color;

        float     _Steps;
        float     _Emission;
        float4    _EmissionColor;

        float     _NLPower;
        float     _AttenPower;


        struct Input { float2 uv_MainTex; };

        // Toon 用カスタムライティング
        fixed4 LightingToonLit (SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
            half nl = saturate(dot(s.Normal, lightDir));
            // 面の向きを絞る
            nl = pow(nl, max(_NLPower, 1.0)); // 値が上がるほどあたりが狭くなる

            // ライトの減衰をまとめて絞る
            half att = saturate(atten);
            att = pow(att, max(_AttenPower, 1.0)); // 値が上がるほどスポットライトのコーンが狭くなる

           half raw = saturate(nl * att);

            half steps = max(_Steps, 2.0);
            half q     = floor(raw * steps) / (steps - 1.0);  // 0,1/(N-1),...,1

            fixed3 col = s.Albedo * _LightColor0.rgb * q;
            return fixed4(col, s.Alpha);
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c  = tex2D(_MainTex, IN.uv_MainTex) * _Color;

            o.Albedo   = c.rgb;
            o.Alpha    = c.a;

            // 自発光（ライティングに加算される）
            o.Emission = c.rgb * _Emission;
        }
        ENDCG
    }
    FallBack Off
}
