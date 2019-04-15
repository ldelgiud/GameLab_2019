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
float3 DiffuseLightDirection = float3(1, 0, 0);

// The color of the diffuse light
float4 DiffuseColor = float4(1, 1, 1, 1);

// The intensity of the diffuse light
float DiffuseIntensity = 1.0;

//--------------------------- TOON SHADER PROPERTIES ------------------------------
// The color to draw the lines in.  Black is a good default.
float4 LineColor = float4(1, 1, 1, 1);

// The thickness of the lines.  This may need to change, depending on the scale of
// the objects you are drawing.
float LineThickness = 0.3;

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


//---------------------------------PASS2----------------------------------

VertexToPixel VertexColorShader(AppToVertex input)
{
	VertexToPixel output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	float4 normal = mul(input.Normal, WorldInverseTranspose);
	float lightIntensity = dot(normal, DiffuseLightDirection);

	output.Color = saturate(DiffuseColor * DiffuseIntensity * lightIntensity); // assuming that diffuseColor has been set up in the mesh drawing

	//output.Color = input.Color; // need fbx file to contain vertex color information

	output.TextureCoordinate = input.TextureCoordinate;

	return output;
}

float4 PixelColorShader(VertexToPixel input) : COLOR0
{
	float4 textureColor = tex2D(textureSampler, input.TextureCoordinate);
	textureColor.a = 1;


	//float4 col = saturate(input.Color);// + AmbientColor * AmbientIntensity);
	//col.a = 1;


	return textureColor;
}

// The entire technique for doing toon shading
technique Outline
{
	// The first pass will go through and draw the back-facing triangles with the outline shader,
	// which will draw a slightly larger version of the model with the outline color.  Later, the
	// model will get drawn normally, and draw over the top most of this, leaving only an outline.
	pass Pass1
	{
		VertexShader = compile VS_SHADERMODEL OutlineVertexShader();
		PixelShader = compile PS_SHADERMODEL OutlinePixelShader();
		CullMode = CW;
	}

	pass Pass2
	{
		VertexShader = compile VS_SHADERMODEL VertexColorShader();
		PixelShader = compile PS_SHADERMODEL PixelColorShader();
		CullMode = CCW;
	}

}