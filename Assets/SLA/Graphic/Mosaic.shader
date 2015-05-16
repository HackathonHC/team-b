Shader "SLA/PostEffect/Mosaic"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}

	SubShader
	{
		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }

			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest 
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			float _PixelCountX;
			float _PixelCountY;

			fixed4 frag (v2f_img i) : SV_Target
			{
				float2 uv;
				uv.x = floor((i.uv.x - 0.5) * _PixelCountX) / _PixelCountX + 0.5;
				uv.y = floor((i.uv.y - 0.5) * _PixelCountY) / _PixelCountY + 0.5;
				
				return tex2D(_MainTex, uv);
			}

			ENDCG
		}
	}

	Fallback off

}