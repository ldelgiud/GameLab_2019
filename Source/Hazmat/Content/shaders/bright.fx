﻿#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif


Texture2D SpriteTexture;

sampler2D s0 = sampler_state
{
	Texture = <SpriteTexture>;
};


struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 texCoord : TEXCOORD0;
};

uniform float u_blurSize;
uniform float u_intensity;
float time;


float4 MainPS(VertexShaderOutput input) : COLOR
{
	float2 uv = input.texCoord.xy;
	float4 inCol = tex2D(s0, input.texCoord);
	float4 outCol = float4(0,0,0,0);
	
	float blurSize = 0.0;
	blurSize += sin(time * 2) * u_blurSize;

	if (inCol.a == 0)
	{
		// take nine samples, with the distance blurSize between them
		outCol += tex2D(s0, float2(input.texCoord.x - 3.0*blurSize , input.texCoord.y)) * 0.05;
		outCol += tex2D(s0, float2(input.texCoord.x - 2.0*blurSize , input.texCoord.y)) * 0.09;
		outCol += tex2D(s0, float2(input.texCoord.x - 1.0*blurSize, input.texCoord.y)) * 0.12;
		//outCol += tex2D(s0, float2(input.texCoord.x - blurSize, input.texCoord.y)) * 0.15;
		//outCol += tex2D(s0, float2(input.texCoord.x, input.texCoord.y)) * 0.16;
		//outCol += tex2D(s0, float2(input.texCoord.x + blurSize, input.texCoord.y)) * 0.15;
		outCol += tex2D(s0, float2(input.texCoord.x + 1.0*blurSize, input.texCoord.y)) * 0.12;
		outCol += tex2D(s0, float2(input.texCoord.x + 2.0*blurSize, input.texCoord.y)) * 0.09;
		outCol += tex2D(s0, float2(input.texCoord.x + 3.0*blurSize, input.texCoord.y)) * 0.05;

		//// blur in y (vertical)
		// take nine samples, with the distance blurSize between them
		outCol += tex2D(s0, float2(input.texCoord.x, input.texCoord.y - 3.0*blurSize)) * 0.05;
		outCol += tex2D(s0, float2(input.texCoord.x, input.texCoord.y - 2.0*blurSize)) * 0.09;
		outCol += tex2D(s0, float2(input.texCoord.x, input.texCoord.y - 1.0*blurSize)) * 0.12;
		//outCol += tex2D(s0, float2(input.texCoord.x, input.texCoord.y - blurSize)) * 0.15;
		//outCol += tex2D(s0, float2(input.texCoord.x, input.texCoord.y)) * 0.16;
		//outCol += tex2D(s0, float2(input.texCoord.x, input.texCoord.y + blurSize)) * 0.15;
		outCol += tex2D(s0, float2(input.texCoord.x, input.texCoord.y + 1.0*blurSize)) * 0.12;
		outCol += tex2D(s0, float2(input.texCoord.x, input.texCoord.y + 2.0*blurSize)) * 0.09;
		outCol += tex2D(s0, float2(input.texCoord.x, input.texCoord.y + 3.0*blurSize)) * 0.05;

		// diagonal
		//outCol += tex2D(s0, float2(input.texCoord.x + 2.0*blurSize, input.texCoord.y + 2.0*blurSize)) * 0.05;
		outCol += tex2D(s0, float2(input.texCoord.x - 2.0*blurSize, input.texCoord.y + 2.0*blurSize)) * 0.05;
		outCol += tex2D(s0, float2(input.texCoord.x + 2.0*blurSize, input.texCoord.y - 2.0*blurSize)) * 0.05;
		//outCol += tex2D(s0, float2(input.texCoord.x - 2.0*blurSize, input.texCoord.y - 2.0*blurSize)) * 0.05;

		if (outCol.a >= 0.0)
		{
			//outCol.a = float4(1, 1, 1, 1);
			outCol.a = float4(0, 0, 0, 0);
		}

	}
	
	return outCol * u_intensity + inCol;
}

technique ChromaticAberation
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};