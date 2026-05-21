/*
    sokol-shdc annotated GLSL for a Zinc custom sgp pipeline.

    The vertex stage matches sgp's contract (attr 0 = coord, attr 1 = color;
    sgp pre-transforms vertices, so gl_Position is just coord.xy). The fragment
    stage draws an animated 2D signed-distance-field of smooth-unioned circles.

    Zinc.Magic turns this file into:
      - Res.Assets.sdf            (ShaderAsset handle, from the @program name)
      - Res.Shaders.sdf.Fs        (std140 uniform struct for the @fs block)
    and the build's shdc step produces the compiled sg_shader behind it.
*/
@module sdf

@vs vs
in vec4 coord;
in vec4 color;
out vec2 texUV;
out vec4 iColor;
void main() {
    gl_Position = vec4(coord.xy, 0.0, 1.0);
    texUV = coord.zw;
    iColor = color;
}
@end

@fs fs
// slot 1 == sgp's fragment uniform slot (set via sgp_set_uniform fs_data).
layout(binding=1) uniform sdf_fs_params {
    vec2 iResolution;
    float iTime;
};
in vec2 texUV;
in vec4 iColor;
out vec4 fragColor;

float sdCircle(vec2 p, float r) { return length(p) - r; }

float opSmoothUnion(float d1, float d2, float k) {
    float h = clamp(0.5 + 0.5 * (d2 - d1) / k, 0.0, 1.0);
    return mix(d2, d1, h) - k * h * (1.0 - h);
}

void main() {
    // window-space pixel -> centered, aspect-corrected coords
    vec2 uv = (gl_FragCoord.xy / iResolution) * 2.0 - 1.0;
    uv.x *= iResolution.x / iResolution.y;

    // five orbiting blobs smooth-unioned together, plus a pulsing core
    float d = 1e9;
    for (int i = 0; i < 5; i++) {
        float a = iTime * 0.7 + float(i) * 1.2566370614; // 2*pi/5
        vec2 c = 0.55 * vec2(cos(a), sin(a * 1.3));
        float di = sdCircle(uv - c, 0.18);
        d = (i == 0) ? di : opSmoothUnion(d, di, 0.25);
    }
    d = opSmoothUnion(d, sdCircle(uv, 0.12 + 0.05 * sin(iTime * 2.0)), 0.25);

    vec3 bg = vec3(0.04, 0.05, 0.09);
    vec3 inside = 0.5 + 0.5 * cos(iTime + d * 8.0 + vec3(0.0, 2.0, 4.0));
    vec3 col = mix(bg, inside, smoothstep(0.01, -0.01, d));
    // thin outer glow on the iso-line
    col += (0.015 / max(abs(d), 0.002)) * vec3(0.2, 0.5, 1.0) * step(0.0, d);

    fragColor = vec4(col, 1.0) * iColor;
}
@end

@program sdf vs fs
