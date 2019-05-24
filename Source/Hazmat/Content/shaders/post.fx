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


float Brightness = 0;
float Contrast = 0;
float Saturation = 0.4;
float Hue = 0.2;

bool redVignetteActive = false;

const float3 lumCoeff = float3(0.2125, 0.7154, 0.0721);

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



float3x3 QuaternionToMatrix(float4 quat)
{
    float3 cross = quat.yzx * quat.zxy;
    float3 square= quat.xyz * quat.xyz;
    float3 wimag = quat.w * quat.xyz;

    square = square.xyz + square.yzx;

    float3 diag = 0.5 - square;
    float3 a = (cross + wimag);
    float3 b = (cross - wimag);

    return float3x3(
    2.0 * float3(diag.x, b.z, a.y),
    2.0 * float3(a.z, diag.y, b.x),
    2.0 * float3(b.y, a.x, diag.z));
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 outputColor = tex2D(s0, input.texCoord);
    float3 hsv; 
    float3 intensity;           
    float3 root3 = float3(0.57735, 0.57735, 0.57735);
	float half_angle = 0.5 * radians(Hue); // Hue is radians of 0 tp 360 degree
	float4 rot_quat = float4( (root3 * sin(half_angle)), cos(half_angle));
	float3x3 rot_Matrix = QuaternionToMatrix(rot_quat);     
	outputColor.rgb = mul(rot_Matrix, outputColor.rgb);
	outputColor.rgb = (outputColor.rgb - 0.5) *(Contrast + 1.0) + 0.5;  
	outputColor.rgb = outputColor.rgb + Brightness;         
	intensity = float(dot(outputColor.rgb, lumCoeff));
	outputColor.rgb = lerp(intensity, outputColor.rgb, Saturation );            
	
	outputColor.a = 1 - alpha;
	return outputColor;
}

float4 VignettePS(VertexShaderOutput input) : COLOR0
{	
	float4 outputColor = tex2D(s0, input.texCoord);
    float3 hsv; 
    float3 intensity;           
    float3 root3 = float3(0.57735, 0.57735, 0.57735);
	float half_angle = 0.5 * radians(Hue); // Hue is radians of 0 tp 360 degree
	float4 rot_quat = float4( (root3 * sin(half_angle)), cos(half_angle));
	float3x3 rot_Matrix = QuaternionToMatrix(rot_quat);     
	outputColor.rgb = mul(rot_Matrix, outputColor.rgb);
	outputColor.rgb = (outputColor.rgb - 0.5) *(Contrast + 1.0) + 0.5;  
	outputColor.rgb = outputColor.rgb + Brightness;         
	intensity = float(dot(outputColor.rgb, lumCoeff));
	outputColor.rgb = lerp(intensity, outputColor.rgb, Saturation );            
	
	float dist = distance(input.texCoord, float2(0.5, 0.5)) * 0.7;
	float blackness = smoothstep(radiusX, radiusY, dist);
	outputColor.rgb *= blackness;
	if (redVignetteActive) outputColor.rgb = saturate(outputColor.rgb	+ ((1 - blackness) * float3(1, 0, 0)));
	outputColor.a = alpha;
	
	return outputColor;
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