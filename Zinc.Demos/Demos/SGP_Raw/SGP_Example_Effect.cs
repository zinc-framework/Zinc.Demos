using System.Numerics;

namespace Zinc.Sandbox.Demos;

// Port of sokol_gp's sample-effect.c (https://github.com/edubart/sokol_gp).
// A 2D shader effect that samples a base image and warps it with animated fog driven by a second
// (perlin) texture. Exercises the material system's texture/sampler binding for custom shaders:
// res/shaders/effect.glsl declares 2 textures + 2 samplers + a fragment uniform block, all bound
// per-object via .Material. The .Shader / .Material accessors come from MaterialComponent's
// [EntityAccessible] members (Zinc.ECSGenerator); the typed Fs uniform struct + the effect handle
// come from Zinc.Magic's reflection of the .glsl.
[DemoScene("SGP_Example_Effect")]
public class SGP_Example_Effect : Scene
{
    public override void Create()
    {
        // generated asset handles (Zinc.Magic scans res/) — paths baked in, resolved correctly
        var baseTex = Res.Assets.lpc_winter_preview.Texture;
        var perlin = Res.Assets.perlin.Texture;
        baseTex.Load();   // populate Width/Height for the aspect ratio below
        perlin.Load();
        var linear = new Sampler(); // defaults: linear filter, repeat wrap
        float imageRatio = (float)baseTex.Width / baseTex.Height;

        // color white so the shader's iColor (per-vertex color) doesn't tint the result
        var rect = new Shape(Engine.Width, Engine.Height, color: new Color(1f, 1f, 1f, 1f));
        rect.Shader = Res.Assets.effect;
        rect.Renderer_Pivot = Vector2.Zero; // anchor top-left so the rect covers (0,0)-(W,H)
        rect.Material.SetImage(0, baseTex);   // iTexChannel0 — base image
        rect.Material.SetImage(1, perlin);    // iTexChannel1 — fog noise
        rect.Material.SetSampler(0, linear);
        rect.Material.SetSampler(1, linear);

        rect.Update = (self, dt) =>
        {
            var s = (Shape)self;
            // cover the window while preserving the base image's aspect ratio (like the sokol_gp sample)
            float windowRatio = (float)Engine.Width / Engine.Height;
            s.Renderer_Width = windowRatio >= imageRatio ? Engine.Width : imageRatio * Engine.Height;
            s.Renderer_Height = windowRatio >= imageRatio ? Engine.Width / imageRatio : Engine.Height;
            s.Material.SetFragmentUniforms(new Res.Shaders.effect.Fs
            {
                iVelocity = new Vector2(0.02f, 0.01f),
                iPressure = 0.3f,
                iTime = (float)Engine.Time,
                iWarpiness = 0.2f,
                iRatio = imageRatio,
                iZoom = 0.4f,
                iLevel = 1.0f,
            });
        };
    }
}
