Shader "Custom/ToonDissolve_CustomData"
{
    Properties
    {
        _Color         ("Main Color", Color) = (1,1,1,1)
        _MainTex       ("MainTex", 2D) = "white" {}

        _DissolveTex   ("Dissolve Noise (R)", 2D) = "gray" {}

        _Steps         ("Toon Steps (min 2)", Float) = 3
        _NLPower       ("Lambert Power", Float) = 1
        _AttenPower    ("Atten Power", Float) = 1

        _LitTint       ("Lit Tint", Color) = (1,1,1,1)
        _ShadowTint    ("Shadow Tint", Color) = (0.2,0.2,0.25,1)
        _MinStep       ("Shadow Floor", Range(0,1)) = 0.1

        _Emission      ("Emission", Float) = 0
        _EmissionColor ("Emission Color", Color) = (1,1,1,1)

        // CustomData → しきい値へのマッピング
        _UseCustom     ("Use Custom1.x", Float) = 1
        _ThrOffset     ("Threshold Offset", Range(-1,1)) = 0.0
        _ThrScale      ("Threshold Scale", Range(0,3)) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest" }
        LOD 200

        CGPROGRAM
        #pragma surface surf ToonLit fullforwardshadows addshadow alpha:clip
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _DissolveTex;
        float4    _Color;

        float     _Steps, _NLPower, _AttenPower;
        float4    _LitTint, _ShadowTint;
        float     _MinStep;

        float     _Emission; float4 _EmissionColor;

        float     _UseCustom, _ThrOffset, _ThrScale;

        struct Input {
            float2 uv_MainTex;
            float4 color : COLOR;        // ← Particle Color（必要なら使用）
            float4 custom1 : TEXCOORD1;  // ← Custom Data (Custom1.xyzw)
            float3 viewDir;
            INTERNAL_DATA
        };

        // Toon ライティング（明部/暗部ティント）
        fixed4 LightingToonLit (SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
            half nl   = saturate(dot(s.Normal, lightDir));
            nl        = pow(nl, max(_NLPower, 1.0));
            half attp = pow(saturate(atten), max(_AttenPower, 1.0));
            half raw  = saturate(nl * attp);

            half steps = max(_Steps, 2.0);
            half q     = floor(raw * steps) / (steps - 1.0);
            half qLit  = lerp(_MinStep, 1.0, q);

            fixed3 litCol    = s.Albedo * _LightColor0.rgb * qLit * _LitTint.rgb;
            fixed3 shadowCol = s.Albedo * _ShadowTint.rgb;

            fixed3 col = lerp(shadowCol, litCol, q);
            return fixed4(col, s.Alpha);
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 baseC = tex2D(_MainTex, IN.uv_MainTex) * _Color;

            // --- Custom1.x → 閾値 0..1 にマッピング ---
            float thr = _ThrOffset; // デフォルトはオフセット値
            if (_UseCustom > 0.5)
                thr = saturate(_ThrOffset + IN.custom1.x * _ThrScale);

            // ノイズと比較して Cutout（影にも反映）
            float n = tex2D(_DissolveTex, IN.uv_MainTex).r;
            clip(n - thr);

            o.Albedo   = baseC.rgb;
            o.Alpha    = 1;
            o.Emission = _EmissionColor.rgb * _Emission;
        }
        ENDCG
    }

    Fallback Off
}
