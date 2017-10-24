// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/ArenaShader"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_MainColor ("MainColor", Color) = (1, 1, 1, 1)
		_GridThickness ("Grid Thickness", Float) = 0.01
		_GridSpacing ("Grid Spacing", Float) = 10.0
		_GridColour ("Grid Colour", Color) = (0.5, 1.0, 1.0, 1.0)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }

		Pass
		{
			CGPROGRAM
			#pragma shader_feature _EMISSION

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile LIGHTMAP_ON LIGHTMAP_OFF
			
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#include "Lighting.cginc"
			
			struct vertIn
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord1 : TEXCOORD1;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float4 worldPos : TEXCOORD0;
				float4 color : COLOR;
				float2 lightMapUV : TEXCOORD1;
			};

			sampler2D _MainTex;
			uniform float4 _MainColor;
			uniform float _GridThickness;
			uniform float _GridSpacing;
			uniform float4 _GridColour;

			
			vertOut vert(vertIn v)
			{
				vertOut o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = _MainColor;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.lightMapUV = ((v.texcoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
				return o;
			}
			
			float4 frag (vertOut v) : SV_Target
			{
				float4 output;
				float3 base = v.color.rgb;
				fixed4 lightmap = UNITY_SAMPLE_TEX2D(unity_Lightmap, v.lightMapUV);
				
				//float4 albedo = tex2D(_MainTex, v.worldPos);
				//fixed3 lightmap = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, v.lightMapUV));
				//output = float4(albedo.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb, albedo.a);
				if (frac(v.worldPos.x/_GridSpacing) < _GridThickness || frac(v.worldPos.z/_GridSpacing) < _GridThickness) 
				{
					return _GridColour;
				}
				else 
				{
					//float4 emission = tex2D(_EmissionMap, v.worldPos) * _EmissionColor;
					//output.rgb *= lightmap.rgb * 0.5f;
					output = tex2D(_MainTex, v.worldPos) * lightmap * lightmap.a;
					output.rgb += base * UNITY_LIGHTMODEL_AMBIENT.rgb;
					return output;
				}

			}
			
			ENDCG
		}
	}
	FallBack "Diffuse"
}
