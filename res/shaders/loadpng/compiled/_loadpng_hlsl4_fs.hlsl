Texture2D<float4> tex : register(t0);
SamplerState _tex_sampler : register(s0);

static float4 frag_color;
static float2 uv;

struct SPIRV_Cross_Input
{
    float2 uv : TEXCOORD0;
};

struct SPIRV_Cross_Output
{
    float4 frag_color : SV_Target0;
};

#line 12 "./shaders/loadpng.glsl"
void frag_main()
{
#line 12 "./shaders/loadpng.glsl"
    frag_color = tex.Sample(_tex_sampler, uv);
}

SPIRV_Cross_Output main(SPIRV_Cross_Input stage_input)
{
    uv = stage_input.uv;
    frag_main();
    SPIRV_Cross_Output stage_output;
    stage_output.frag_color = frag_color;
    return stage_output;
}
