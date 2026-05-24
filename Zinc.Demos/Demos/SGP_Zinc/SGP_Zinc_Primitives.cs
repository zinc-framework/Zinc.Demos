using static System.MathF;

namespace Zinc.Sandbox.Demos;

// Idiomatic Zinc port of SGP_Example_Primitives.
// The raw demo draws rects, points, lines and triangles with immediate-mode GP calls across four
// viewports. Zinc is entity-oriented: rectangles are Shapes, and the transform tricks the raw demo shows
// (translate / rotate / scale) are just entity properties (X/Y, Rotation, ScaleX/Y) animated in an update
// lambda. Raw point/line/triangle primitives have no entity equivalent — use SGP_Raw if you need those.
[DemoScene("SGP_Zinc_Primitives")]
public class SGP_Zinc_Primitives : Scene
{
    public override void Create()
    {
        float cy = Engine.Height * 0.5f;
        float qx = Engine.Width * 0.25f;
        const float size = 80f;

        // left: bounces (translate via Y)
        new Shape(size, size, color: new Color(0.9f, 0.3f, 0.4f, 1f), update: (s, dt) =>
        {
            float t = (Sin((float)Engine.Time) + 1f) * 0.5f;
            s.Y = cy + (2f * size * t - size);
        }) { X = qx, Y = cy };

        // middle: spins (Rotation)
        new Shape(size, size, color: new Color(0.4f, 0.9f, 0.3f, 1f), update: (s, dt) =>
        {
            s.Rotation = (float)Engine.Time;
        }) { X = qx * 2f, Y = cy };

        // right: pulses (ScaleX/Y)
        new Shape(size, size, color: new Color(0.3f, 0.5f, 0.95f, 1f), update: (s, dt) =>
        {
            float t = (Sin((float)Engine.Time) + 1f) * 0.5f;
            s.ScaleX = t + 0.5f;
            s.ScaleY = t + 0.5f;
        }) { X = qx * 3f, Y = cy };
    }
}
