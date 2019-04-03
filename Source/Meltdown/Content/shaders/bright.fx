#if OPENGL
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


float4 MainPS(VertexShaderOutput input) : COLOR
{
	float2 uv = input.texCoord.xy;
	float4 inCol = tex2D(s0, input.texCoord);
	float4 outCol = float4(0,0,0,0);

	float alpha = -4.0 * inCol.a;
	alpha += tex2D(s0, input.texCoord.xy + float2(0.1, 0.0)).a;
	alpha += tex2D(s0, input.texCoord.xy + float2(-0.1, 0.0)).a;
	alpha += tex2D(s0, input.texCoord.xy + float2(0.0, -1.0)).a;
	alpha += tex2D(s0, input.texCoord.xy + float2(0.0, 1.0)).a;

//	for (uint i = 0; i < 31; i++) {
//		for (uint j = 0; j < 31; j++) {
//			float offset = 15;
//			float valX = (i - offset);
//			float valY = (j - offset);
//
//			outCol += tex2D(s0, 
//				  	float2(
//						valX + input.texCoord.x, 
//						valY + input.texCoord.y 
//					      )
//				  	);
//		}	
//	}	   
//
//	outCol.a = 1;

	return (inCol.rbg, alpha);
}

technique ChromaticAberation
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};