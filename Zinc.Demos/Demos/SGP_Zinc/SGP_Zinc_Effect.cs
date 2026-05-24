using System.Numerics;

namespace Zinc.Sandbox.Demos;

// Idiomatic Zinc version of the Effect demo (mirrors SGP_Raw/SGP_Example_Effect, which is already
// idiomatic). A custom shader (res/shaders/effect.glsl) with 2 textures + 2 samplers + a uniform block,
// all bound per-object through the material system: .Shader, .Material.SetImage/SetSampler, and
// .Material.SetFragmentUniforms with the generated typed struct. No raw GP texture/uniform binding.
[DemoScene("SGP_Zinc_Effect")]
public class SGP_Zinc_Effect : Scene
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
            // cover the window while preserving the base image's aspect ratio
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
