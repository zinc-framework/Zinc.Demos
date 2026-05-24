using Zinc.Internal.Sokol;

namespace Zinc.Sandbox.Demos;

// Port of sokol_gp's sample-bench.c. Stress-draws thousands of filled and
// textured rects with interleaved textures/colors to exercise GP's batch
// optimizer. The original targets a square window; here the whole 3x3 grid is
// uniformly scaled to fit Zinc's window so every section stays visible.
// image1/image2 reuse the demo project's two bundled textures rather than the
// procedural gradients the C sample generates.
[DemoScene("SGP_Example_Bench")]
public class SGP_Example_Bench : Scene
{
    private const int count = 48;
    private const int rect_count = 4;

    private sg_image image1;
    private sg_image image2;

    public override void Preload()
    {
        Res.Assets.conscript.Load();
        Res.Assets.logo.Load();
        image1 = Res.Assets.conscript.Texture.Data;
        image2 = Res.Assets.logo.Texture.Data;
    }

    public override void Update(double dt)
    {
        GP.set_color(0.05f, 0.05f, 0.05f, 1.0f);
        GP.clear();
        GP.reset_color();

        int off = count * rect_count * 2;   // width/height of one section
        float gridSize = 3f * off;           // the 3x3 grid of sections
        float scale = (Engine.Height * 0.95f) / gridSize;

        GP.push_transform();
        // scale-to-fit, then center horizontally; all section translates below
        // compose under this transform
        GP.translate((Engine.Width - gridSize * scale) * 0.5f, Engine.Height * 0.025f);
        GP.scale(scale, scale);

        BenchRepeatedTextured();
        GP.translate(off, 0);
        BenchMultipleTextured();
        GP.translate(off, 0);
        BenchColoredTextured();
        GP.translate(-2 * off, off);
        BenchRepeatedFilled();
        GP.translate(off, 0);
        BenchMixed();
        GP.translate(off, 0);
        BenchColoredFilled();
        GP.translate(-2 * off, off);
        DrawCat();
        GP.translate(off, 0);
        DrawRect();
        GP.translate(off, 0);
        BenchSyncMixed();

        GP.pop_transform();
        GP.reset_color();
    }

    private void Rect(int x, int y) =>
        GP.draw_filled_rect(x * rect_count * 2, y * rect_count * 2, rect_count, rect_count);

    private void SetCyclicColor(int x)
    {
        if (x % 3 == 0) GP.set_color(1.0f, 0, 0, 1.0f);
        else if (x % 3 == 1) GP.set_color(0, 1.0f, 0, 1.0f);
        else GP.set_color(0, 0, 1.0f, 1.0f);
    }

    private void BenchRepeatedTextured()
    {
        GP.reset_color();
        GP.set_image(0, image1);
        for (int y = 0; y < count; ++y)
            for (int x = 0; x < count; ++x)
                Rect(x, y);
        GP.reset_image(0);
    }

    private void BenchMultipleTextured()
    {
        GP.reset_color();
        for (int y = 0; y < count; ++y)
            for (int x = 0; x < count; ++x)
            {
                GP.set_image(0, x % 2 == 0 ? image1 : image2);
                Rect(x, y);
            }
        GP.reset_image(0);
    }

    private void BenchColoredTextured()
    {
        GP.reset_color();
        GP.set_image(0, image1);
        for (int y = 0; y < count; ++y)
            for (int x = 0; x < count; ++x)
            {
                SetCyclicColor(x);
                Rect(x, y);
            }
        GP.reset_image(0);
    }

    private void BenchRepeatedFilled()
    {
        GP.reset_color();
        for (int y = 0; y < count; ++y)
            for (int x = 0; x < count; ++x)
                Rect(x, y);
    }

    private void BenchColoredFilled()
    {
        GP.reset_color();
        for (int y = 0; y < count; ++y)
            for (int x = 0; x < count; ++x)
            {
                SetCyclicColor(x);
                Rect(x, y);
            }
    }

    private void BenchMixed()
    {
        for (int diagonal = 0; diagonal < 2 * count - 1; ++diagonal)
        {
            int advance = Math.Max(diagonal - count + 1, 0);
            for (int y = diagonal - advance, x = advance; y >= 0 && x < count; --y, ++x)
            {
                SetCyclicColor(x);
                if ((x + y) % 2 == 0)
                {
                    Rect(x, y);
                }
                else
                {
                    GP.set_image(0, image1);
                    Rect(x, y);
                    GP.reset_image(0);
                }
            }
        }
    }

    private void BenchSyncMixed()
    {
        GP.set_image(0, image1);
        GP.reset_color();
        for (int y = 0; y < count; ++y)
            for (int x = 0; x < count; ++x)
            {
                GP.set_color((x + y) % 2 == 0 ? 1.0f : 0, (x + y) % 2 == 0 ? 0 : 1.0f, 0, 1.0f);
                Rect(x, y);
            }
        GP.reset_image(0);
    }

    private void DrawCat()
    {
        GP.reset_color();
        GP.set_image(0, image1);
        GP.draw_filled_rect(0, 0, rect_count * count * 2, rect_count * count * 2);
        GP.reset_image(0);
    }

    private void DrawRect()
    {
        GP.reset_color();
        GP.draw_filled_rect(0, 0, rect_count * count * 2, rect_count * count * 2);
    }
}
