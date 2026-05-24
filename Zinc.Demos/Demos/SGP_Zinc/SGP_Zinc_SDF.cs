using System.Numerics;

namespace Zinc.Sandbox.Demos;

// Idiomatic Zinc version of the SDF demo (mirrors SGP_Raw/SGP_Example_SDF, which is already idiomatic).
// A fullscreen Shape whose .Shader is the SDF pipeline compiled from res/shaders/sdf.glsl; uniforms are
// set by filling the generated std140 struct (Res.Shaders.sdf.Fs) and handing it to
// .Material.SetFragmentUniforms. No raw GP.set_pipeline / GP.set_uniform — the material system handles it.
[DemoScene("SGP_Zinc_SDF")]
public class SGP_Zinc_SDF : Scene
{
    public override void Create()
    {
        // color white so the shader's `* iColor` (vertex color) doesn't tint the result
        var rect = new Shape(Engine.Width, Engine.Height, color: new Color(1f, 1f, 1f, 1f));
        rect.Shader = Res.Assets.sdf;
        rect.Renderer_Pivot = Vector2.Zero; // anchor top-left so the rect covers (0,0)-(W,H)

        rect.Update = (self, dt) =>
        {
            var s = (Shape)self;
            s.Renderer_Width = Engine.Width;   // keep it fullscreen across resizes
            s.Renderer_Height = Engine.Height;
            s.Material.SetFragmentUniforms(new Res.Shaders.sdf.Fs
            {
                iResolution = new Vector2(Engine.Width, Engine.Height),
                iTime = (float)Engine.Time,
            });
        };
    }
}
