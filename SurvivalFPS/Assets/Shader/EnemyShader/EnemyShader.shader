Shader "Unlit/EnemyShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Scale("Scale", Float) = 1.0
        _Speed("Speed", Float) = 1.0
        _Amplitude("Amplitude", Float) = 0.5
        _NoiseTex("Noise Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Scale;       // ノイズの細かさ（テクスチャUVを何倍に拡大するか）
            float _Speed;       // 流れる速さ（時間に対するオフセット）
            float _Amplitude;   // 揺らぎの大きさ（頂点変形や明暗差に乗算）
            sampler2D _NoiseTex;

            v2f vert (appdata v)
            {
                float noise = snoise(float3(v.vertex.xyz * _Scale, _Time.y * _Speed));
                v.vertex.xyz += v.normal * noise * _Amplitude;

                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                uv+= float2(_Time.y * _FlowX,_Time.x * _FlowY);
                float noiseTex = tex2D(_NoiseTex,uv).r;

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
