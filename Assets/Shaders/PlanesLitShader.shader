// Blinn-Phong illumination model shader adapted from Lab 4 solution PhongShader.Shader
// Adapted so that it handles directional light in WorldSpace over a surface/plane
Shader "Lit/PlanesLitShader"
{ 
	//Define variable properties of lighting model
	Properties
	{
		_fAtt ("Attentuation Factor", Range (0.0, 1.0)) = 0.75
		_Ka ("Ambient Intensity", Range (0.0, 1.0)) = 1.0
		_Kd ("Diffuse Intensity", Range (0.0, 1.0)) = 1.0
		_Ks ("Specular Intensity", Range (0.0, 1.0)) = 0.25
		_specN ("Specular Power", Float) = 100.0
		_Color ("Main Color", Color) = (.5,.5,.5,1)
	}
	SubShader
	{
		LOD 200

		Pass
		{
			Tags {"LightMode"="ForwardBase"}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"

	
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

			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
				vertOut o;

				// Convert Vertex position and corresponding normal into world coords.
				// Note that we have to multiply the normal by the transposed inverse of the world 
				// transformation matrix (for cases where we have non-uniform scaling; we also don't
				// care about the "fourth" dimension, because translations don't affect the normal) 
				float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
				float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));

				// Transform vertex in world coordinates to camera coordinates, and pass colour
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = _Color;

				// Pass out the world vertex position and world normal to be interpolated
				// in the fragment shader (and utilised)
				o.worldVertex = worldVertex;
				o.worldNormal = worldNormal;

				return o;
			}

			// Implementation of the fragment/pixel shader
			fixed4 frag(vertOut v) : SV_Target
			{
				// Our interpolated normal might not be of length 1
				float3 interpNormal = normalize(v.worldNormal);

				// Calculate ambient RGB intensities
				float3 amb = v.color.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * _Ka;

				// Calculate diffuse RBG reflections
				float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
				float LdotN = dot(lightDir, interpNormal);
				float3 dif = _fAtt * _LightColor0.rgb * _Kd * v.color.rgb * saturate(LdotN);

				// Calculate specular reflections using Blinn-Phong approximation
				float3 viewDir = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);
				float3 H = normalize(viewDir + lightDir);
				float3 NdotH = dot(interpNormal, H);
				float3 spe = _fAtt * _LightColor0.rgb * _Ks * pow(saturate(NdotH), _specN);

				// Combine Phong illumination model components
				float4 returnColor = float4(0, 0, 0, 0);
				returnColor.rgb = amb.rgb + dif.rgb + spe.rgb;
				returnColor.a = v.color.a;

				return returnColor;
			}
			ENDCG
		}
	}
}