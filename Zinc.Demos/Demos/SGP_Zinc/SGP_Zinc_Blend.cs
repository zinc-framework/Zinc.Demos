namespace Zinc.Sandbox.Demos;

// Idiomatic Zinc port of SGP_Example_Blend.
//
// The raw demo calls GP.set_blend_mode between immediate draws inside a 60-unit-tall, aspect-corrected
// projection. Here every rectangle is a Shape and its blend mode is just a property (Renderer_BlendMode).
// To match the reference pixel-for-pixel we reproduce its layout in screen space: the raw demo projects
// (0, 60*ratio, 0, 60) over the framebuffer, which is a uniform Height/60 px-per-unit scale, so we place
// the Shapes at unit*scale. Seven cells show the seven blend modes, each a triad of overlapping
// translucent R/G/B Shapes over a 0.2/0.4 gray checkerboard.
//
// z-order note: the renderer draws OrderByDescending(RenderOrder) — i.e. *higher RenderOrder = further
// back*. So the checkerboard gets a high RenderOrder (behind) and within a triad R > G > B so they paint
// R, then G, then B on top, matching the raw demo's draw order (which is what makes the blends agree).
[DemoScene("SGP_Zinc_Blend")]
public class SGP_Zinc_Blend : Scene
{
    // (mode, cell position in the raw demo's 60-unit space) — identical to sample-blend.c.
    private static readonly (BlendMode mode, float tx, float ty)[] Cells =
    {
        (BlendMode.None,                0,  0),
        (BlendMode.Blend,              20,  0),
        (BlendMode.BlendPremultiplied, 40,  0),
        (BlendMode.Add,                20, 20),
        (BlendMode.AddPremultiplied,   40, 20),
        (BlendMode.Mod,                20, 40),
        (BlendMode.Mul,                40, 40),
    };


    // RenderOrder layers (higher = further back, see note above).
    private const int CheckerOrder = 1000;
    private const int ROrder = 30, GOrder = 20, BOrder = 10;

    public override void Create()
    {
        // Background: clear to 0.2 gray with a 0.4 gray, 32px checkerboard — same as the raw demo's
        // GP.clear(0.2) + checkerboard. The clear handles the squares the checkerboard leaves empty.
        Engine.SetClearColor(new Color(0.2f, 0.2f, 0.2f, 1f));
        const int tile = 32;
        var checkerColor = new Color(0.4f, 0.4f, 0.4f, 1f);
        for (int y = 0; y < Engine.Height / tile + 1; y++)
            for (int x = 0; x < Engine.Width / tile + 1; x++)
                if ((x + y) % 2 == 0)
                    new Shape(tile, tile, color: checkerColor)
                    {
                        X = x * tile + tile / 2f,
                        Y = y * tile + tile / 2f,
                        RenderOrder = CheckerOrder,
                    };

        // Match the raw demo's projection: project(0, 60*ratio, 0, 60) is a uniform Height/60 scale.
        float scale = Engine.Height / 60f;
        foreach (var (mode, tx, ty) in Cells)
            DrawTriad(tx, ty, scale, mode);
    }

    // Three overlapping 10x10-unit translucent rects (R, G, B) at the raw demo's offsets, all using the
    // cell's blend mode. In-cell origin is +(2.5,2.5) units; rects sit at (0,0), (0,5), (5,2.5).
    private void DrawTriad(float tx, float ty, float scale, BlendMode mode)
    {
        float size = 10f * scale;
        const float a = 0.5f;
        // Shape pivot is centered, so X/Y is the rect center = (top-left + 5 units).
        MakeRect((tx + 7.5f) * scale, (ty + 7.5f) * scale, size, new Color(1f, 0f, 0f, a), mode, ROrder);
        MakeRect((tx + 7.5f) * scale, (ty + 12.5f) * scale, size, new Color(0f, 1f, 0f, a), mode, GOrder);
        MakeRect((tx + 12.5f) * scale, (ty + 10f) * scale, size, new Color(0f, 0f, 1f, a), mode, BOrder);
    }

    private void MakeRect(float cx, float cy, float size, Color color, BlendMode mode, int order) =>
        new Shape(size, size, color: color) { X = cx, Y = cy, Renderer_BlendMode = mode, RenderOrder = order };

    // Leave the clear color as the demos found it.
    public override void Cleanup() => Engine.SetClearColor(new Color(Palettes.ONE_BIT_MONITOR_GLOW[0]));
}
