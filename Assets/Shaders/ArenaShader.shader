// Shader for the arena, draws bright colored grid on darker surface and gets illuminated by emissive surfaces.
// Aim is to give off a neon/tron type of vibe, as the setting for our game.

Shader "Unlit/ArenaShader"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_EmissionMap ("Emission Map", 2D) = "white" {}
		_MainColor ("MainColor", Color) = (1, 1, 1, 1)
		_LineThickness ("Line Thickness", Float) = 0.01
		_LineSpacing ("Line Spacing", Float) = 10.0
		[HDR] _LineColorB ("Line ColorB", Color) = (0, 0, 0)
		[HDR] _LineColorR ("Line ColorR", Color) = (0, 0, 0)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 2000
		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityLightingCommon.cginc"
			
			struct vertIn
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 lightMapUV : TEXCOORD1;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float4 worldPos : TEXCOORD0;
				float4 color : COLOR;
				float2 lightMapUV : TEXCOORD1;
			};

			//Properties initialisation
			uniform sampler2D _MainTex;
			uniform sampler2D _EmissionMap;
			uniform float4 _MainColor;
			uniform float _LineThickness;
			uniform float _LineSpacing;
			uniform float4 _LineColorB;
			uniform float4 _LineColorR;

			//Simple vertex shader to setup position vertices and lightmap uv
			vertOut vert(vertIn v)
			{
				vertOut o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.color = _MainColor;
				o.lightMapUV = ((v.lightMapUV.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
				return o;
			}
			//Fragment shader that will draw out grid based on position and bake lightmap from emissive materials
			float4 frag (vertOut v) : SV_Target
			{
				float4 output;
				float3 base = v.color.rgb;
				fixed4 lightmap = UNITY_SAMPLE_TEX2D(unity_Lightmap, v.lightMapUV);
				if (v.worldPos.z <= 0)
				{
					//Shades grid lines
					if (frac(v.worldPos.x/_LineSpacing) < _LineThickness || frac(v.worldPos.z/_LineSpacing) < _LineThickness) 
					{
					
						output = tex2D(_MainTex, v.worldPos);
						float4 emission = tex2D(_EmissionMap, v.worldPos) * _LineColorB;
						output.rgb += emission.rgb;
						return output;
					}
					else 
					{
						//Applies baked lightmap data
						output = tex2D(_MainTex, v.worldPos) * lightmap * lightmap.a;
						output.rgb += base * UNITY_LIGHTMODEL_AMBIENT.rgb;
						return output;
					}
				}
				else
				{
					//Shades grid lines
					if (frac(v.worldPos.x/_LineSpacing) < _LineThickness || frac(v.worldPos.z/_LineSpacing) < _LineThickness) 
					{
					
						output = tex2D(_MainTex, v.worldPos);
						float4 emission = tex2D(_EmissionMap, v.worldPos) * _LineColorR;
						output.rgb += emission.rgb;
						return output;
					}
					else 
					{
						//Applies baked lightmap data
						output = tex2D(_MainTex, v.worldPos) * lightmap * lightmap.a;
						output.rgb += base * UNITY_LIGHTMODEL_AMBIENT.rgb;
						return output;
					}
				}
				

			}
			ENDCG
		}
	}
}