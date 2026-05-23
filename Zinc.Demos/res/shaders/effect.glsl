@module effect

@vs vs
in vec4 coord;          // xy = position, zw = texcoord  (attr 0)
in vec4 color;          // per-vertex color              (attr 1)
out vec2 texUV;
out vec4 iColor;
void main() {
    gl_Position = vec4(coord.xy, 0.0, 1.0);   // sgp pre-transforms verts on the CPU; no MVP uniform
    texUV = coord.zw;
    iColor = color;
}
@end

@fs fs
// Two sampled textures (sgp image channels 0 and 1) + their samplers, plus the fragment
// uniform block. binding=N maps to sgp's set_image(N)/set_sampler(N); the uniform block is
// binding=1 == sgp's FRAGMENT uniform slot.
layout(binding=0) uniform texture2D iTexChannel0;
layout(binding=1) uniform texture2D iTexChannel1;
layout(binding=0) uniform sampler iSmpChannel0;
layout(binding=1) uniform sampler iSmpChannel1;
layout(binding=1) uniform fs_uniforms {
    vec2 iVelocity;
    float iPressure;
    float iTime;
    float iWarpiness;
    float iRatio;
    float iZoom;
    float iLevel;
};
in vec2 texUV;
in vec4 iColor;
out vec4 fragColor;

float noise(vec2 p) {
    return texture(sampler2D(iTexChannel1, iSmpChannel1), p).r;
}

void main() {
    vec3 tex_col = texture(sampler2D(iTexChannel0, iSmpChannel0), texUV).rgb;
    vec2 fog_uv = (texUV * vec2(iRatio, 1.0)) * iZoom;
    float f = noise(fog_uv - iVelocity*iTime);
    f = noise(fog_uv + f*iWarpiness);
    vec3 col = mix(tex_col, vec3(f) * iColor.rgb, iPressure);
    fragColor = vec4(col, 1.0);
}
@end

@program effect vs fs
