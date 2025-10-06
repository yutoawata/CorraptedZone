Shader "Unlit/VertexStreamExample"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transform" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
                // TEXCOORD1Ç≈colorSourceÇéÛÇØéÊÇÈÇÊÇ§Ç…Ç∑ÇÈ
                float3 colorSource : TEXCOORD1;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : Color;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;

                // colorSourceÇÃílÇégÇ¡ÇƒÇ¢Ç¢ä¥Ç∂Ç…êFÇïœÇ¶ÇÈ
                // https://light11.hatenadiary.com/entry/2019/10/26/231903
                float3 oscilateTimes = float3(1.0f, 1.0f, 1.0f);
                float3 phases = float3(0.25f, 0.43f, 0.82f);
                o.color.rgb *= 0.5 + 0.5 * cos(6.28 * (oscilateTimes * v.colorSource + phases.r));

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                return col;
            }
            ENDCG
        }
    }
}