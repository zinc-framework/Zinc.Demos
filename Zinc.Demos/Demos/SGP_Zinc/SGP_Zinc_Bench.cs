namespace Zinc.Sandbox.Demos;

// Idiomatic Zinc port of SGP_Example_Bench.
// The raw demo hand-draws thousands of filled/textured rects with nested GP loops and manual batch
// management. In Zinc you just create entities — Shapes (filled) and Sprites (textured) — and the engine
// batches and draws them. This spawns a grid of each; press to feel how many entities the renderer eats.
[DemoScene("SGP_Zinc_Bench")]
public class SGP_Zinc_Bench : Scene
{
    private const int cols = 80;
    private const int rows = 40;
    private SpriteData sprite;
    Color a = new Color(1f, 0.2f, 0.2f, 1f);
    Color b = new Color(0.2f, 1f, 0.2f, 1f);
    Color c = new Color(0.2f, 0.2f, 1f, 1f);
 
    public override void Preload()
    {
        Res.Assets.conscript.Load();
        sprite = Res.Assets.conscript.Texture.Slice(new Rect(0, 0, 64, 64));
    }

    public override void Create()
    {
        float cellW = Engine.Width / (float)cols;
        float cellH = (Engine.Height * 0.5f) / rows; // top half filled, bottom half textured

        // top half: colored Shapes
        Color active = new Color(1f, 1f, 1f, 1f);
        for (int y = 0; y < rows; y++)
            for (int x = 0; x < cols; x++)
            {
                active = CyclicColor(x);
                new Shape(cellW * 0.85f, cellH * 0.85f, color: active)
                {
                    X = (x + 0.5f) * cellW,
                    Y = (y + 0.5f) * cellH,
                };
            }

        // bottom half: textured Sprites (the bundled conscript character)
        float bottom = Engine.Height * 0.5f;
        float sx = (cellW * 0.95f) / sprite.Rect.width;
        float sy = (cellH * 0.95f) / sprite.Rect.height;
        for (int y = 0; y < rows; y++)
            for (int x = 0; x < cols; x++)
                new Sprite(sprite)
                {
                    X = (x + 0.5f) * cellW,
                    Y = bottom + (y + 0.5f) * cellH,
                    ScaleX = sx,
                    ScaleY = sy,
                };

        Engine.DebugTextStr = $"{cols * rows * 2} entities ({cols * rows} filled + {cols * rows} textured)";
    }

    Color CyclicColor(int x) => (x % 3) switch
    {
        0 => a,
        1 => b,
        _ => c,
    };
}
