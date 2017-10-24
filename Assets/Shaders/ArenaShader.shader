// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/ArenaShader"
{
	Properties
	{
		_MainColor ("_MainColor", Color) = (1, 1, 1, 1)
		_GridThickness ("Grid Thickness", Float) = 0.01
		_GridSpacing ("Grid Spacing", Float) = 10.0
		_GridColour ("Grid Colour", Color) = (0.5, 1.0, 1.0, 1.0)
		_BaseColour ("Base Colour", Color) = (0.0, 0.0, 0.0, 0.0)
		[HDR] _EmissionColor("Emission Color", Color) = (0,0,0)
		_EmissionMap ("Emission Map", 2D) = "black" {}
		
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			Tags { "LightMode" = "ForwardBase" }
			
			CGPROGRAM
			#pragma target 3.0
			#include "UnityStandardCoreForward.cginc"
			#pragma shader_feature _EMISSION

			#pragma vertex vertBase
			#pragma fragment fragBase
			

			ENDCG
		}
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct vertIn
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float4 worldPos : TEXCOORD0;
				float4 color : COLOR;
			};

			uniform float4 _MainColor;
			
			
			uniform float _GridThickness;
			uniform float _GridSpacing;
			uniform float4 _GridColour;
			uniform float4 _BaseColour;
			uniform float4 _EmissionColor;
			uniform sampler2D _EmissionMap;

			vertOut vert(vertIn v)
			{
				vertOut o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}
			
			fixed4 frag (vertOut v) : SV_Target
			{
				if (frac(v.worldPos.x/_GridSpacing) < _GridThickness || frac(v.worldPos.z/_GridSpacing) < _GridThickness) 
				{
					return _GridColour;
				}
				else 
				{
					return _BaseColour;
				}
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
