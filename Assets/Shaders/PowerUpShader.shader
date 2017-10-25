Shader "Unlit/PowerUpShader"
{
	//Define variable properties of lighting model
	Properties
	{
		_fAtt ("Attentuation Factor", Range (0.0, 1.0)) = 0.75
		_Ka ("Ambient Intensity", Range (0.0, 1.0)) = 1.0
		_Kd ("Diffuse Intensity", Range (0.0, 1.0)) = 1.0
		_Ks ("Specular Intensity", Range (0.0, 1.0)) = 0.25
		_specN ("Specular Power", Float) = 100.0
		
	}
	SubShader
	{
		Pass
		{
			Tags {"LightMode"="ForwardBase"}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

	
			struct vertIn
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float4 color : COLOR;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
				float4 worldVertex : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
			};

			//Properties initialisation
			uniform float _fAtt;
			uniform float _Ka;
			uniform float _Kd;
			uniform float _Ks;
			uniform float _specN;
			uniform float4 _Color;

			// Implementation of the vertex shader to setup for fragment shader
			vertOut vert(vertIn v)
			{
				vertOut o;

				float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
				float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));


				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;

				o.worldVertex = worldVertex;
				o.worldNormal = worldNormal;

				return o;
			}

			// Implementation of the fragment/pixel shader for Blinn-Phong lighting model
			float4 frag(vertOut v) : SV_Target
			{
				float4 output;
				float3 interpNormal = normalize(v.worldNormal);

				// Calculate ambient RGB intensities
				float3 amb = v.color.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * _Ka;
				output = float4(amb, 1.0);
				// Calculate diffuse RGB reflections
				float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
				float3 diffuse = _LightColor0.rgb * _Color.rgb * max(0.0, dot(interpNormal, lightDir)) * _Kd;
				output.rgb += diffuse.rgb;
				// Calculate specular RGB reflections
				float3 viewDir = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);
				float3 H = normalize(viewDir + lightDir);
				float3 NdotH = dot(interpNormal, H);
				float3 spe = _fAtt * _LightColor0.rgb * pow(saturate(NdotH), _specN) * _Ks;
				output.rgb += spe.rgb;
				return output;
			}
			ENDCG
		}
	}
}
