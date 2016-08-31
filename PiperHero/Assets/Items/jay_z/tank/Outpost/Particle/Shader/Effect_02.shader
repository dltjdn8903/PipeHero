Shader "Demo_02" 
{
	Properties 
	{
		_Color ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
	}
	
	Category 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		
		Blend OneMinusDstColor One   
		
		AlphaTest Greater .01
		ColorMask RGB
		
		Cull Back
		Lighting Off 
		ZWrite Off 
		Fog { Mode Off }
		
		BindChannels 
		{
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}
		
		// ---- 雙紋理卡
		SubShader 
		{
			Pass 
			{
				SetTexture [_MainTex] 
				{
					constantColor [_Color]
					combine constant * primary
				}
				SetTexture [_MainTex] 
				{
					combine texture * previous DOUBLE
				}
			}
		}
		
		// ---- 單一的紋理卡（不色調）
		SubShader
		{
			Pass 
			{
				SetTexture [_MainTex] 
				{
					combine texture * primary
				}
			}
		}
	}
}
