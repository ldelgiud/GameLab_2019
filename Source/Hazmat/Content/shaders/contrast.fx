#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif


float Brightness = 0;
float Contrast = 0;
float Saturation = 0.4;
float Hue = 0.2;

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




// Converts the rgb value to hsv, where H's range is -1 to 5
float3 rgb_to_hsv(float3 RGB)
{
    float r = RGB.x;
    float g = RGB.y;
    float b = RGB.z;

    float minChannel = min(r, min(g, b));
    float maxChannel = max(r, max(g, b));

    float h = 0;
    float s = 0;
    float v = maxChannel;

    float delta = maxChannel - minChannel;

    if (delta != 0)
    {
        s = delta / v;

        if (r == v) h = (g - b) / delta;
        else if (g == v) h = 2 + (b - r) / delta;
        else if (b == v) h = 4 + (r - g) / delta;
    }

    return float3(h, s, v);
}

float3 hsv_to_rgb(float3 HSV)
{
    float3 RGB = HSV.z;

    float h = HSV.x;
    float s = HSV.y;
    float v = HSV.z;

    float i = floor(h);
    float f = h - i;

    float p = (1.0 - s);
    float q = (1.0 - s * f);
    float t = (1.0 - s * (1 - f));

    if (i == 0) { RGB = float3(1, t, p); }
    else if (i == 1) { RGB = float3(q, 1, p); }
    else if (i == 2) { RGB = float3(p, 1, t); }
    else if (i == 3) { RGB = float3(p, q, 1); }
    else if (i == 4) { RGB = float3(t, p, 1); }
    else /* i == -1 */ { RGB = float3(1, p, q); }

    RGB *= v;

    return RGB;
}


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


const float3 lumCoeff = float3(0.2125, 0.7154, 0.0721);
float4 MainPS(VertexShaderOutput input) : COLOR
{

	//float4 col = tex2D(s0, input.texCoord);
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

    return outputColor;
	

    //float3 hsv = rgb_to_hsv(col.xyz);

    //hsv.x += Hue;
    // Put the hue back to the -1 to 5 range
    //if (hsv.x > 5) { hsv.x -= 6.0; }
    //hsv = hsv_to_rgb(hsv);
    //float4 newColor = float4(hsv,col.w);

    //float4 colorWithBrightnessAndContrast = newColor;

    //colorWithBrightnessAndContrast.rgb /= colorWithBrightnessAndContrast.a;
    //colorWithBrightnessAndContrast.rgb = colorWithBrightnessAndContrast.rgb + Brightness;
    //colorWithBrightnessAndContrast.rgb = ((colorWithBrightnessAndContrast.rgb - 0.5f) * max(Contrast + 1.0, 0)) + 0.5f;  
    //colorWithBrightnessAndContrast.rgb *= colorWithBrightnessAndContrast.a;

    //float greyscale = dot(colorWithBrightnessAndContrast.rgb, float3(0.3, 0.59, 0.11)); 
    //colorWithBrightnessAndContrast.rgb = lerp(greyscale, colorWithBrightnessAndContrast.rgb, col.a * (Saturation + 1.0));       
    //return colorWithBrightnessAndContrast;





	//float4 color = tex2D(s0, input.texCoord);
	//color.rgb /= color.a;

	// Apply contrast.
    //color.rgb = ((color.rgb - 0.5f) * max(Contrast, 0)) + 0.5f;

	// Apply brightness.
	//color.rgb += Brightness;

	// Return final pixel color.
	//color.rgb *= color.a;

	//return color;
}

technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
