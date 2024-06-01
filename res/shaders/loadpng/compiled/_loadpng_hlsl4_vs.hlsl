cbuffer vs_params : register(b0)
{
    row_major float4x4 _21_mvp : packoffset(c0);
};


static float4 gl_Position;
static float4 pos;
static float2 uv;
static float2 texcoord0;

struct SPIRV_Cross_Input
{
    float4 pos : TEXCOORD0;
    float2 texcoord0 : TEXCOORD1;
};

struct SPIRV_Cross_Output
{
    float2 uv : TEXCOORD0;
    float4 gl_Position : SV_Position;
};

#line 15 "./shaders/loadpng.glsl"
void vert_main()
{
#line 15 "./shaders/loadpng.glsl"
    gl_Position = mul(pos, _21_mvp);
#line 16 "./shaders/loadpng.glsl"
    uv = texcoord0;
}

SPIRV_Cross_Output main(SPIRV_Cross_Input stage_input)
{
    pos = stage_input.pos;
    texcoord0 = stage_input.texcoord0;
    vert_main();
    SPIRV_Cross_Output stage_output;
    stage_output.gl_Position = gl_Position;
    stage_output.uv = uv;
    return stage_output;
}
