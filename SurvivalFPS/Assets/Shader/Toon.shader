// Rampテクスチャと呼ばれる色をサンプリングできる専用テクスチャを使う
// オブジェクト表面の暗い色はこの色明るい色はこの色のようにRampテクスチャから色を取得する


// 明るい暗いを計算するにはライトのベクトルとオブジェクトの法線ベクトルの内積を取る
// これによりライトと法線方向が一致する場合は明るく、ライトと法線方向が垂直に近づくにつれ値が小さくなり暗いと判定される


// Toonシェーダーはsurfだけではつくれないためライティングも自分で書く必要がある

Shader "Custom/Toon"{
	Properties{
		_Color("Color",Color) = (1, 1, 1, 1)
		_MainTex("Albedo(RGB)", 2D) = "white"{}
		_RampTex("Ramp", 2D) = "white"{}
		_Emmision("Emmision", Range(0.1, 10)) = 0.1
		}

		SubShader{
			Tags{"RenderType" = "Opaque"}
			LOD 200

			CGPROGRAM
			// ここでライト用のメソッドをUnityに伝える(ライト用のメソッドのLightingを抜いたもの)
			#pragma surface surf ToonRamp fullforwardShadow
			#pragma target 3.0

			sampler2D _MainTex;
			sampler2D _RampTex;
			float _Emmision;

			struct Input {
				float2 uv_MainTex;
				};

				fixed4 _Color;


				// ライト用のメソッドはLightingから始める必要がある
				fixed4 LightingToonRamp(SurfaceOutput s, fixed3 lightDir, fixed atten){
					half d = dot(s.Normal, lightDir) * 0.5 + 0.5; // ライトと法線の内積を取る
					fixed3 ramp = tex2D(_RampTex, fixed2(d, 0.5)).rgb;
					fixed3 col = s.Albedo * _LightColor0.rgb * ramp * _Emmision;
					col *= atten;
					return fixed4(col, s.Alpha);
					// fixed4 c;
					// c.rgb = s.Albedo * _LightColor0.rgb * ramp;
					// c.a = 0;
					//return c;
					}

				   // ライティングの工程を挟んだ場合はSurfaceOutputStandard型が使えない
				   //　そのためSurfaceOutput型に書き換える(SurfaceOutput型はEmmisionやSmoothnessは定義されていない)
					void surf(Input IN, inout SurfaceOutput o){
						fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
						o.Albedo = c.rgb;
						o.Alpha = c.a;
						}
						ENDCG
			}
			FallBack"Diffuse"
	}