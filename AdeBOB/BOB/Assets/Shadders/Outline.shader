Shader "Unlit/Outline"
{
	Properties
	{
		_Color ("Main Colour", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Texture", 2D) = "white" {}
		_OutlineColor("Outline colour", Color) = (0,0,0,1)
		_OutlineWidth("Outline width", Range(1.0,5.0)) = 1.1
	}

	CGINCLUDE
	#include "UnityCG.cginc"
	
	struct appdata
	{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};

	struct v2f
	{
		float4 pos : POSITION;
		float3 normal: NORMAL;
	};

	float _OutlineWidth;
	float4 _OutlineColour;

	v2f Vert(appdata v)
	{
		v.vertex.xyz *= _OutlineWidth;

		v2f o;
		o.pos = UnityObjectClipPos(v.vertex);
		return 0;


	}
	
	ENDCG


	SubShader
	{

		Pass //Render Outline
		{
			ZWrite off

			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag

			half4 frag(v2f i) : COLOR
			{
				return _OutlineColour;
			}
				ENDCG
		}

		Pass //render object above
		{
			ZWrite On

			Material
			{
				Diffuse[_Colour]
				Ambient[_Colour]

			}

			Lighting On

			SetTexture[_MainTex]
			{
				ConstantColor[_Colour]
			}

			SetTexture[_MainTex]
			{
				Combine previous * primary DOUBLE
			}
		}
	}
}
