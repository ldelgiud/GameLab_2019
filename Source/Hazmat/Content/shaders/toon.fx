#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

//--------------------------- BASIC PROPERTIES ------------------------------
// The world transformation
float4x4 World;

// The view transformation
float4x4 View;

// The projection transformation
float4x4 Projection;

// The transpose of the inverse of the world transformation,
// used for transforming the vertex's normal
float4x4 WorldInverseTranspose;

//--------------------------- DIFFUSE LIGHT PROPERTIES ------------------------------
// The direction of the diffuse light
float3 DiffuseLightDirection = float3(1, 1, 1);

// The color of the diffuse light
float4 DiffuseColor = float4(1, 1, 1, 1);

// The intensity of the diffuse light
float DiffuseIntensity = 1;

float4 AmbientColor = float4(1, 1, 1, 1);
float AmbientIntensity = 0;

bool ActivateBlink = false;

//--------------------------- TOON SHADER PROPERTIES ------------------------------
// The color to draw the lines in.  Black is a good default.
float4 LineColor = float4(0.0, 0.0, 0.0, 1);

// The thickness of the lines.  This may need to change, depending on the scale of
// the objects you are drawing.
float LineThickness = 0.3;

float4 GlowLineColor = float4(1, 1, 1, 1);
float GlowLineThickness = 0.3;
bool enableGlowing; // REMEMBER TO ALSO SET THE MODEL AS UPDATETIME true

float time;

//--------------------------- TEXTURE PROPERTIES ------------------------------
// The texture being used for the object
texture Texture;

// The texture sampler, which will get the texture color
sampler2D textureSampler = sampler_state
{
	Texture = (Texture);
	MinFilter = Linear;
	MagFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};


//--------------------------- DATA STRUCTURES ------------------------------
// The structure used to store information between the application and the
// vertex shader
struct AppToVertex
{
	float4 Position : POSITION0;            // The position of the vertex
	float3 Normal : NORMAL0;                // The vertex's normal
	float2 TextureCoordinate : TEXCOORD0;    // The texture coordinate of the vertex
};

// The structure used to store information between the vertex shader and the
// pixel shader
struct VertexToPixel
{
	float4 Position : POSITION0;
	float2 TextureCoordinate : TEXCOORD0;
	float3 Normal : TEXCOORD1;
	float4 Color : COLOR0;
};

//--------------------------- SHADERS ------------------------------

//---------------------------------PASS1----------------------------------
// The vertex shader that does the outlines
VertexToPixel GlowVertexShader(AppToVertex input)
{
	VertexToPixel output = (VertexToPixel)0;

	// Calculate where the vertex ought to be.  This line is equivalent
	// to the transformations in the CelVertexShader.
	float4 original = mul(mul(mul(input.Position, World), View), Projection);

	// Calculates the normal of the vertex like it ought to be.
	float4 normal = mul(mul(mul(input.Normal, World), View), Projection);

	// Take the correct "original" location and translate the vertex a little
	// bit in the direction of the normal to draw a slightly expanded object.
	// Later, we will draw over most of this with the right color, except the expanded
	// part, which will leave the outline that we want.
	output.Position = original + (mul(LineThickness + GlowLineThickness, normal));

	return output;
}

// The pixel shader for the outline.  It is pretty simple:  draw everything with the
// correct line color.
float4 GlowPixelShader(VertexToPixel input) : COLOR0
{
	if (enableGlowing)
	{
		float4 col = GlowLineColor;
		col.a = 0.5 + 1 * sin(time * 3.5);
		return col;
	}
	return float4(0, 0, 0, 0);
}

//---------------------------------PASS2----------------------------------

// The vertex shader that does the outlines
VertexToPixel OutlineVertexShader(AppToVertex input)
{
	VertexToPixel output = (VertexToPixel)0;

	// Calculate where the vertex ought to be.  This line is equivalent
	// to the transformations in the CelVertexShader.
	float4 original = mul(mul(mul(input.Position, World), View), Projection);

	// Calculates the normal of the vertex like it ought to be.
	float4 normal = mul(mul(mul(input.Normal, World), View), Projection);

	// Take the correct "original" location and translate the vertex a little
	// bit in the direction of the normal to draw a slightly expanded object.
	// Later, we will draw over most of this with the right color, except the expanded
	// part, which will leave the outline that we want.
	output.Position = original + (mul(LineThickness, normal));

	return output;
}

// The pixel shader for the outline.  It is pretty simple:  draw everything with the
// correct line color.
float4 OutlinePixelShader(VertexToPixel input) : COLOR0
{
	return LineColor;
}


//---------------------------------PASS3----------------------------------

VertexToPixel CelVertexShader(AppToVertex input)
{
	VertexToPixel output;

	// Transform the position
	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	// Transform the normal
	//output.Normal = normalize(mul(input.Normal, WorldInverseTranspose)); // changes based on position
	output.Normal = normalize(input.Normal);


	// Hack to read position (not allowed otherwise)
	//output.TextureCoordinate = output.Position;

	output.TextureCoordinate = input.TextureCoordinate;

	return output;


}

float4 CelPixelShader(VertexToPixel input) : COLOR0
{
	// Calculate diffuse light amount
	float intensity = dot(normalize(DiffuseLightDirection), input.Normal);
	if (intensity < 0)
		intensity = 0;

	// Achieve nice changing effect
	//float change = cos();

	// Calculate what would normally be the final color, including texturing and diffuse lighting
	float4 color = tex2D(textureSampler, input.TextureCoordinate) * DiffuseColor * DiffuseIntensity;
	color.a = 1;

	// Discretize the intensity, based on a few cutoff points
	if (intensity > 0.95)
		color = float4(1.0, 1, 1, 1.0) * color;
	else if (intensity > 0.5)
		color = float4(0.7, 0.7, 0.7, 1.0) * color;
	else if (intensity > 0.05)
		color = float4(0.35, 0.35, 0.35, 1.0) * color;
	else
		color = float4(0.1, 0.1, 0.1, 1.0) * color;

	color = saturate(color * 3); //pump intensity

	if (ActivateBlink) {
		return float4(1,1,1,1);
	}
	else
	{
		return saturate(color + AmbientColor * AmbientIntensity);
	}
}

// The entire technique for doing toon shading
technique Toon
{
	// The first pass will go through and draw the back-facing triangles with the outline shader,
	// which will draw a slightly larger version of the model with the outline color.  Later, the
	// model will get drawn normally, and draw over the top most of this, leaving only an outline.
	pass Pass1
	{
		VertexShader = compile VS_SHADERMODEL GlowVertexShader();
		PixelShader = compile PS_SHADERMODEL GlowPixelShader();
		CullMode = CW;
		//AlphaBlendEnable = true;
		//SrcBlend = SrcAlpha;
		//DestBlend = InvSrcAlpha;
	}

	pass Pass2
	{
		VertexShader = compile VS_SHADERMODEL OutlineVertexShader();
		PixelShader = compile PS_SHADERMODEL OutlinePixelShader();
		CullMode = CW;
		//AlphaBlendEnable = true;
		//SrcBlend = SrcAlpha;
		//DestBlend = InvSrcAlpha;
	}

	pass Pass3
	{
		VertexShader = compile VS_SHADERMODEL CelVertexShader();
		PixelShader = compile PS_SHADERMODEL CelPixelShader();
		CullMode = CCW;
	}

}







//
//#if OPENGL
//#define SV_POSITION POSITION
//#define VS_SHADERMODEL vs_3_0
//#define PS_SHADERMODEL ps_3_0
//#else
//#define VS_SHADERMODEL vs_4_0_level_9_1
//#define PS_SHADERMODEL ps_4_0_level_9_1
//#endif
//
////--------------------------- BASIC PROPERTIES ------------------------------
//// The world transformation
//float4x4 World;
//
//// The view transformation
//float4x4 View;
//
//// The projection transformation
//float4x4 Projection;
//
//// The transpose of the inverse of the world transformation,
//// used for transforming the vertex's normal
//float4x4 WorldInverseTranspose;
//
////--------------------------- DIFFUSE LIGHT PROPERTIES ------------------------------
//// The direction of the diffuse light
//float3 DiffuseLightDirection = float3(1, 1, 1);
//
//// The color of the diffuse light
//float4 DiffuseColor = float4(1, 1, 1, 1);
//
//// The intensity of the diffuse light
//float DiffuseIntensity = 1;
//
//float4 AmbientColor = float4(1, 1, 1, 1);
//
//float AmbientIntensity = 0.2;
//
////--------------------------- TOON SHADER PROPERTIES ------------------------------
//// The color to draw the lines in.  Black is a good default.
//float4 LineColor = float4(0.0, 0.0, 0.0, 1);
//
//// The thickness of the lines.  This may need to change, depending on the scale of
//// the objects you are drawing.
//float LineThickness = 0.3;
//
////--------------------------- TEXTURE PROPERTIES ------------------------------
//// The texture being used for the object
//texture Texture;
//
//// The texture sampler, which will get the texture color
//sampler2D textureSampler = sampler_state
//{
//	Texture = (Texture);
//	MinFilter = Linear;
//	MagFilter = Linear;
//	AddressU = Clamp;
//	AddressV = Clamp;
//};
//
//
////--------------------------- DATA STRUCTURES ------------------------------
//// The structure used to store information between the application and the
//// vertex shader
//struct AppToVertex
//{
//	float4 Position : POSITION0;            // The position of the vertex
//	float3 Normal : NORMAL0;                // The vertex's normal
//	float2 TextureCoordinate : TEXCOORD0;    // The texture coordinate of the vertex
//};
//
//// The structure used to store information between the vertex shader and the
//// pixel shader
//struct VertexToPixel
//{
//	float4 Position : POSITION0;
//	float2 TextureCoordinate : TEXCOORD0;
//	float3 Normal : TEXCOORD1;
//	float4 Color : COLOR0;
//};
//
////--------------------------- SHADERS ------------------------------
//// The vertex shader that does the outlines
//VertexToPixel OutlineVertexShader(AppToVertex input)
//{
//	VertexToPixel output = (VertexToPixel)0;
//
//	// Calculate where the vertex ought to be.  This line is equivalent
//	// to the transformations in the CelVertexShader.
//	float4 original = mul(mul(mul(input.Position, World), View), Projection);
//
//	// Calculates the normal of the vertex like it ought to be.
//	float4 normal = mul(mul(mul(input.Normal, World), View), Projection);
//
//	// Take the correct "original" location and translate the vertex a little
//	// bit in the direction of the normal to draw a slightly expanded object.
//	// Later, we will draw over most of this with the right color, except the expanded
//	// part, which will leave the outline that we want.
//	output.Position = original + (mul(LineThickness, normal));
//
//	return output;
//}
//
//// The pixel shader for the outline.  It is pretty simple:  draw everything with the
//// correct line color.
//float4 OutlinePixelShader(VertexToPixel input) : COLOR0
//{
//	return LineColor;
//}
//
//
////---------------------------------PASS2----------------------------------
//
//VertexToPixel CelVertexShader(AppToVertex input)
//{
//	VertexToPixel output;
//
//	// Transform the position
//	float4 worldPosition = mul(input.Position, World);
//	float4 viewPosition = mul(worldPosition, View);
//	output.Position = mul(viewPosition, Projection);
//
//	// Transform the normal
//	//output.Normal = normalize(mul(input.Normal, WorldInverseTranspose)); // changes based on position
//	output.Normal = normalize(input.Normal);
//
//
//	// Hack to read position (not allowed otherwise)
//	//output.TextureCoordinate = output.Position;
//
//	output.TextureCoordinate = input.TextureCoordinate;
//
//	return output;
//
//
//}
//
//float4 CelPixelShader(VertexToPixel input) : COLOR0
//{
//	// Calculate diffuse light amount
//	float intensity = dot(normalize(DiffuseLightDirection), input.Normal);
//	if (intensity < 0)
//		intensity = 0;
//
//	// Achieve nice changing effect
//	//float change = cos();
//
//	// Calculate what would normally be the final color, including texturing and diffuse lighting
//	float4 color = tex2D(textureSampler, input.TextureCoordinate);// * DiffuseColor * DiffuseIntensity;
//	color.a = 1;
//
//	// Discretize the intensity, based on a few cutoff points
//	if (intensity > 0.95)
//		color = float4(1.0, 1, 1, 1.0) * color;
//	else if (intensity > 0.5)
//		color = float4(0.7, 0.7, 0.7, 1.0) * color;
//	else if (intensity > 0.05)
//		color = float4(0.35, 0.35, 0.35, 1.0) * color;
//	else
//		color = float4(0.1, 0.1, 0.1, 1.0) * color;
//
//	color = saturate(color * 3); //pump intensity
//
//	return saturate(color);// + AmbientColor * AmbientIntensity);
//
//
//}
//
//// The entire technique for doing toon shading
//technique Toon
//{
//	// The first pass will go through and draw the back-facing triangles with the outline shader,
//	// which will draw a slightly larger version of the model with the outline color.  Later, the
//	// model will get drawn normally, and draw over the top most of this, leaving only an outline.
//	pass Pass1
//	{
//		VertexShader = compile VS_SHADERMODEL OutlineVertexShader();
//		PixelShader = compile PS_SHADERMODEL OutlinePixelShader();
//		CullMode = CW;
//		AlphaBlendEnable = true;
//		SrcBlend = SrcAlpha;
//		DestBlend = InvSrcAlpha;
//	}
//
//	pass Pass2
//	{
//		VertexShader = compile VS_SHADERMODEL CelVertexShader();
//		PixelShader = compile PS_SHADERMODEL CelPixelShader();
//		CullMode = CCW;
//	}
//
//}