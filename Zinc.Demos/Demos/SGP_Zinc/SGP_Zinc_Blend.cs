namespace Zinc.Sandbox.Demos;

// Idiomatic Zinc port of SGP_Example_Blend.
// The raw demo calls GP.set_blend_mode between draws. Here each rectangle is a Shape and its blend mode is
// just a property: Renderer_BlendMode. Seven cells show the blend modes, each with three overlapping
// translucent R/G/B Shapes, over a checkerboard of Shapes. (Renderer_BlendMode is the affordance added so
// you don't drop to raw GP for blending — it's on Sprites too.)
[DemoScene("SGP_Zinc_Blend")]
public class SGP_Zinc_Blend : Scene
{
    private static readonly (BlendMode mode, string label)[] Modes =
    {
        (BlendMode.None, "None"),
        (BlendMode.Blend, "Blend"),
        (BlendMode.BlendPremultiplied, "BlendPremul"),
        (BlendMode.Add, "Add"),
        (BlendMode.AddPremultiplied, "AddPremul"),
        (BlendMode.Mod, "Mod"),
        (BlendMode.Mul, "Mul"),
    };

    public override void Create()
    {
        // Checkerboard background. Created first, so it gets lower render order and draws behind the cells.
        const int tile = 32;
        for (int y = 0; y * tile < Engine.Height; y++)
            for (int x = 0; x * tile < Engine.Width; x++)
                if ((x + y) % 2 == 0)
                    new Shape(tile, tile, color: new Color(0.4f, 0.4f, 0.4f, 1f))
                    {
                        X = x * tile + tile / 2f,
                        Y = y * tile + tile / 2f,
                    };

        // One cell per blend mode, laid out in a 4-wide grid.
        float cell = MathF.Min(Engine.Width / 4f, Engine.Height / 3f);
        for (int i = 0; i < Modes.Length; i++)
        {
            float ox = (i % 4) * cell + cell * 0.5f;
            float oy = (i / 4) * cell + cell * 0.6f;
            DrawTriad(ox, oy, cell * 0.30f, Modes[i].mode);
        }
    }

    // Three overlapping translucent rects (R, G, B), all using the cell's blend mode.
    private void DrawTriad(float cx, float cy, float size, BlendMode mode)
    {
        const float a = 0.5f;
        float off = size * 0.4f;
        MakeRect(cx - off, cy - off, size, new Color(1f, 0f, 0f, a), mode);
        MakeRect(cx + off, cy - off, size, new Color(0f, 1f, 0f, a), mode);
        MakeRect(cx, cy + off, size, new Color(0f, 0f, 1f, a), mode);
    }

    private void MakeRect(float x, float y, float size, Color color, BlendMode mode) =>
        new Shape(size, size, color: color) { X = x, Y = y, Renderer_BlendMode = mode };
}
