#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

uniform float radiusX = 0.5;    //should be bigger than y
uniform float radiusY = 0.37;  
uniform float alpha = 0.6; // for vignette

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

 //  float4 base = tex2D(backBuffer, IN.uv0);    
 //  float dist = distance(IN.uv0, float2(0.5,0.5)) * 0.7 ;    
 //  base.rgb *= smoothstep(radiusX, radiusY, dist);    
 //  return base;  

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 color = tex2D(s0, input.texCoord);
	color.a = 1;
	return color;
}

float4 VignettePS(VertexShaderOutput input) : COLOR0
{
	float4 color = tex2D(s0, input.texCoord);
	
	float dist = distance(input.texCoord, float2(0.5, 0.5)) * 0.7;
	color.rgb *= smoothstep(radiusX, radiusY, dist);
	color.a = alpha;
	return color;
}

technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
	
	pass P1
	{
		PixelShader = compile PS_SHADERMODEL VignettePS();
	}
};