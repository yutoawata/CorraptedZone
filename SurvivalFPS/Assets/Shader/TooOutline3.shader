Shader "Custom/OutlineToon3"
{
    Properties
    {
        _Color         ("Main Color", Color) = (1,1,1,1)
        _MainTex       ("MainTex", 2D) = "white" {}

        _DissolveTex   ("Dissolve (R)", 2D) = "gray" {}
        _Threshold     ("Threshold", Range(0,1)) = 0.5

        _Steps         ("Toon Steps (min 2)", Float) = 3
        _NLPower       ("Lambert Power (>=1 tighter)", Float) = 1
        _AttenPower    ("Attenuation Power (>=1 tighter)", Float) = 1

        _LitTint       ("Lit Tint", Color) = (1,1,1,1)
        _ShadowTint    ("Shadow Tint", Color) = (0.2,0.2,0.25,1)
        _MinStep       ("Shadow Floor (0..1)", Range(0,1)) = 0.1

        _Emission      ("Emission", Float) = 0
        _EmissionColor ("Emission Color", Color) = (1,1,1,1)

        _RimColor("RimColor", Color) = (1, 1,1,1)
    }

    SubShader
    {
        // 影を出すため Cutout を使用
        Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest" }
        LOD 200

        CGPROGRAM
        #pragma surface surf ToonLit fullforwardshadows addshadow alpha:clip vertex:vert
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _DissolveTex;
        float4    _Color;

        float     _Steps;
        float     _NLPower;
        float     _AttenPower;

        float4    _LitTint;
        float4    _ShadowTint;
        float     _MinStep;

        float     _Emission;
        float4    _EmissionColor;

        half      _Threshold;

        float4 _RimColor;


        struct Input {
    float2 uv_MainTex;
    float3 viewDir;
    float  Threshold;
    INTERNAL_DATA
    };

// 頂点関数で TEXCOORD1.x（= Custom1.x）を詰め替え
    void vert (inout appdata_full v, out Input o) {
    UNITY_INITIALIZE_OUTPUT(Input, o);
    o.Threshold = v.texcoord1.x;
    }

        fixed4 LightingToonLit (SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
            half nl   = saturate(dot(s.Normal, lightDir));
            nl        = pow(nl, max(_NLPower, 1.0));
            half attp = pow(saturate(atten), max(_AttenPower, 1.0));
            half raw  = saturate(nl * attp);

            half steps = max(_Steps, 2.0);
            half q     = floor(raw * steps) / (steps - 1.0);   // 0..1 段階
            half qLit  = lerp(_MinStep, 1.0, q);               // 暗部の最低明度

            fixed3 litCol    = s.Albedo * _LightColor0.rgb * qLit * _LitTint.rgb;
            fixed3 shadowCol = s.Albedo * _ShadowTint.rgb;

            fixed3 col = lerp(shadowCol, litCol, q);           // 影→明部へ補間
            return fixed4(col, s.Alpha);
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 baseC = tex2D(_MainTex, IN.uv_MainTex) * _Color;

            // 切り抜き（影にも反映）
            float2 nUV = IN.uv_MainTex + _Time.y * 0.2; // 0.2 = 速度
            float n    = tex2D(_DissolveTex, nUV).r;
            clip(n - IN.Threshold);

            o.Albedo   = baseC.rgb;
            o.Alpha    = 1; // Cutoutなので1固定
            float rim = 1 - saturate(dot(IN.viewDir, o.Normal));
            o.Emission = _Emission * baseC.rgb;
        }
        ENDCG
    }

    Fallback Off
}
