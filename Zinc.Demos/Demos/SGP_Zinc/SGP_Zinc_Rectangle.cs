using Zinc.Core;
using static System.MathF;

namespace Zinc.Sandbox.Demos;

// Idiomatic Zinc port of SGP_Example_Rectangle.
// Raw version: each frame sets up a projection and calls GP.rotate_at + GP.draw_filled_rect.
// Zinc version: create one Shape entity and animate its Rotation/Color in an update lambda — the engine
// draws it every frame for you. No projection, no per-frame draw calls.
[DemoScene("SGP_Zinc_Rectangle")]
public class SGP_Zinc_Rectangle : Scene
{
    public override void Create()
    {
        float size = Engine.Height * 0.4f;
        var color = new Color(1f, 0.2f, 0.2f, 1f);
        // var ct = new ColorTween(color, new Color(0.2f, 0.2f, 1f, 1f), Easing.EaseInOutSine)
        // {
        //     Duration = 2f,
        //     Loop = true,
        //     ValueUpdated = (c) => color = c
        // };
        var rect = new Shape(size, size, color:color);
        Quick.Center(rect); // a Shape's pivot is its center, so it spins in place
    }
}
