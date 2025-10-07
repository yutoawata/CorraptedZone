Shader "Custom/ToonExplosion_BIRP"
{
    Properties
    {
        _MainTex        ("Noise / Shape (R)", 2D) = "gray" {}
        _Color          ("Tint", Color) = (1,1,1,1)

        // Toon
        _Steps          ("Toon Steps (>=2)", Float) = 4
        _EdgeSharpness  ("Edge Sharpness", Range(0.5, 8)) = 2

        // Ring shape (time animated)
        _Radius         ("Ring Radius", Range(0,1.5)) = 0.2
        _Thickness      ("Ring Thickness", Range(0.01,1)) = 0.25
        _Dissolve       ("Jaggy Dissolve (0..1)", Range(0,1)) = 0.0
        _DissolveAmp    ("Dissolve Strength", Range(0,1)) = 0.35

        // Outline (thin bright rim)
        _OutlineWidthPx ("Outline Width (px)", Range(0,6)) = 2
        _OutlineColor   ("Outline Color", Color) = (1,1,1,1)

        // Fade / Emission
        _Emission       ("Emission", Float) = 2.0
        _Fade           ("Global Fade", Range(0,1)) = 1.0

        // Render options
        _SoftParticles  ("Soft Particles", Float) = 1
        _InvFade        ("SoftParticles InvFade", Range(0.01, 10)) = 2
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        Blend One OneMinusSrcAlpha    // デフォ: アルファ合成（煙も可）
        ZWrite Off
        Cull Off
        Lighting Off
        Fog { Mode Off }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_particles
            #pragma multi_compile_fog
            #pragma target 3.0

            #include "UnityCG.cginc"

            sampler2D _MainTex; float4 _MainTex_ST;
            fixed4 _Color;
            float _Steps, _EdgeSharpness;
            float _Radius, _Thickness;
            float _Dissolve, _DissolveAmp;
            float _Emission, _Fade;

            float _SoftParticles, _InvFade;
            float _OutlineWidthPx; fixed4 _OutlineColor;

            // ソフトパーティクル
            UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                fixed4 color  : COLOR;    // Particle Color (頂点カラー)
            };

            struct v2f
            {
                float4 pos   : SV_POSITION;
                float2 uv    : TEXCOORD0;
                fixed4 col   : COLOR;
                UNITY_FOG_COORDS(1)
                float4 projPos : TEXCOORD2; // for soft particles
                float2 screenUV: TEXCOORD3; // outline px width
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv  = TRANSFORM_TEX(v.uv, _MainTex);
                o.col = v.color;
                UNITY_TRANSFER_FOG(o, o.pos);

                // ソフトパーティクル用
                o.projPos = ComputeScreenPos(o.pos);
                // 画面ピクセル幅計算用（SV_POSITION→NDC→pixel座標の近似）
                o.screenUV = o.pos.xy / o.pos.w;

                return o;
            }

            // 画面空間ピクセル幅 → UV上の閾値に変換（おおまかに）
            float outline_px_to_uv(float px, float4 pos_clip, float2 uv)
            {
                // quad前提で等方と仮定（簡易）
                // 実運用では _ScreenParams や頂点距離で補正
                float pixelToNDC = px * 2.0 / _ScreenParams.x;
                // UV空間でだいたい同等の幅に
                return pixelToNDC; 
            }

            // ソフトパーティクル
            float SoftParticleFade(v2f i)
            {
                if (_SoftParticles < 0.5) return 1.0;

                float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
                float partZ  = LinearEyeDepth(i.projPos.z / i.projPos.w);
                float diff   = (sceneZ - partZ);             // 手前ほど小さい
                return saturate(diff * _InvFade);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 中心を (0.5,0.5) として半径を取る
                float2 uv = i.uv;
                float2 p  = uv - 0.5;
                float r   = length(p);

                // 基本リング： [R, R+T] 内を本体、それ以外カット
                float inner = _Radius;
                float outer = _Radius + _Thickness;

                // ノイズで縁を崩す
                float n = tex2D(_MainTex, uv * 2.0).r; // 低周波ノイズ推奨
                float jag = (n - 0.5) * 2.0 * _DissolveAmp * _Dissolve; // -amp..+amp
                inner += jag * 0.5;
                outer += jag;

                // 0..1 マスク（スムーズ化してから段階化）
                float ringMask = smoothstep(outer, inner, r); // innerで1, outerで0 になる
                ringMask = saturate(ringMask);

                // トゥーン段階化
                float steps = max(2.0, _Steps);
                float band  = floor(pow(saturate(ringMask), _EdgeSharpness) * steps) / (steps - 1.0);

                // アウトライン（リングの外縁近傍を細く足す）
                float pxUv = outline_px_to_uv(_OutlineWidthPx, i.pos, i.uv);
                float rim  = smoothstep(outer + pxUv, outer, r) * smoothstep(inner, inner - pxUv, r);
                fixed4 outlineCol = _OutlineColor * rim;

                // 本体色（頂点カラー×色×段階）
                fixed4 col = fixed4(1,1,1,1) * band;
                col *= _Color;
                col *= i.col;              // Particle Color
                col.a *= _Fade;

                // 発光っぽく
                col.rgb *= _Emission;

                // アウトラインを加算（明るい縁）
                col.rgb += outlineCol.rgb * outlineCol.a;

                // ソフトパーティクル
                col.a *= SoftParticleFade(i);

                // 透明下地のときの視認性向上（少しだけ自己加算）
                col.rgb = max(0, col.rgb);

                return col;
            }
            ENDCG
        }
    }

    Fallback Off
}
