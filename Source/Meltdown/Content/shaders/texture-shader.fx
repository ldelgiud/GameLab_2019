#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float4x4 World;
float4x4 View;
float4x4 Projection;

float4x4 WorldInverseTranspose;

float4 AmbientColor = float4(1, 1, 1, 1);
float AmbientIntensity = 0.5;

float3 DiffuseLightDirection = float3(0.25, 1, 0.75);
float4 DiffuseColor = float4(1, 1, 1, 1);
float DiffuseIntensity = 1.0;

// Texture
texture Texture; // our current models don't have any textures -> only colors from materials (should be set as vertex colors but it is also not the case :/)
sampler2D textureSampler = sampler_state {
	Texture = (Texture);
	MagFilter = Linear;
	MinFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Normal : NORMAL0;
	float2 TextureCoordinate : TEXCOORD0;
	//float4 Color : COLOR0; //needs to be stored in vertex color info
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
	float2 TextureCoordinate : TEXCOORD1;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	float4 normal = mul(input.Normal, WorldInverseTranspose);
	float lightIntensity = dot(normal, DiffuseLightDirection);

	output.Color = saturate(DiffuseColor * DiffuseIntensity * lightIntensity);

	//output.Color = input.Color; // need fbx file to contain vertex color information

	output.TextureCoordinate = input.TextureCoordinate;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 textureColor = tex2D(textureSampler, input.TextureCoordinate);
	textureColor.a = 1;


	//float4 col = saturate(textureColor + input.Color + AmbientColor * AmbientIntensity);
	//col.a = 1;


	return textureColor;
}

technique Ambient
{
	pass Pass1
	{
		VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
}


// #if OPENGL
// 	#define SV_POSITION POSITION
// 	#define VS_SHADERMODEL vs_3_0
// 	#define PS_SHADERMODEL ps_3_0
// #else
// 	#define VS_SHADERMODEL vs_4_0_level_9_1
// 	#define PS_SHADERMODEL ps_4_0_level_9_1
// #endif

// float4x4 World;
// float4x4 View;
// float4x4 Projection;

// float4x4 WorldInverseTranspose;

// float4 AmbientColor = float4(1, 1, 1, 1);
// float AmbientIntensity = 0.5;

// float3 DiffuseLightDirection = float3(0.25, 1, 0.75);
// float4 DiffuseColor = float4(1, 1, 1, 1);
// float DiffuseIntensity = 1.0;

// // Texture
// texture ModelTexture; // our current models don't have any textures -> only colors from materials (should be set as vertex colors but it is also not the case :/)
// sampler2D textureSampler = sampler_state {
// 	Texture = (ModelTexture);
// 	MagFilter = Linear;
// 	MinFilter = Linear;
// 	AddressU = Clamp;
// 	AddressV = Clamp;
// };

// struct VertexShaderInput
// {
// 	float4 Position : POSITION0;
// 	float4 Normal : NORMAL0;
// 	float2 TextureCoordinate : TEXCOORD0;
// 	//float4 Color : COLOR0; //needs to be stored in vertex color info
// };

// struct VertexShaderOutput
// {
// 	float4 Position : POSITION0;
// 	float4 Color : COLOR0;
// 	float2 TextureCoordinate : TEXCOORD1;
// };

// VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
// {
// 	VertexShaderOutput output;

// 	float4 worldPosition = mul(input.Position, World);
// 	float4 viewPosition = mul(worldPosition, View);
// 	output.Position = mul(viewPosition, Projection);

// 	float4 normal = mul(input.Normal, WorldInverseTranspose);
// 	float lightIntensity = dot(normal, DiffuseLightDirection);

// 	output.Color = saturate(DiffuseColor * DiffuseIntensity * lightIntensity);

// 	//output.Color = input.Color; // need fbx file to contain vertex color information

// 	output.TextureCoordinate = input.TextureCoordinate;

// 	return output;
// }

// float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
// {
// 	//float4 textureColor = tex2D(textureSampler, input.TextureCoordinate);
// 	//textureColor.a = 1;


// 	float4 col = saturate(input.Color + AmbientColor * AmbientIntensity);
// 	col.a = 1;


// 	return col;
// }

// technique Ambient
// {
// 	pass Pass1
// 	{
// 		VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
// 		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
// 	}
// }