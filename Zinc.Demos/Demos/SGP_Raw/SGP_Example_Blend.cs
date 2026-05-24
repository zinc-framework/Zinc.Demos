using Zinc.Internal.Sokol;

namespace Zinc.Sandbox.Demos;

// Port of sokol_gp's sample-blend.c. Draws three overlapping RGB rectangles
// under each of GP's seven blend modes, over a checkerboard, to verify
// set_blend_mode behaves across the full set of modes.
[DemoScene("SGP_Example_Blend")]
public class SGP_Example_Blend : Scene
{
    public override void Update(double dt)
    {
        int width = Engine.Width, height = Engine.Height;
        DrawCheckboard(width, height);
        DrawRects(width / (float)height);

        // restore engine defaults for the remainder of the frame
        GP.reset_blend_mode();
        GP.reset_color();
        GP.reset_project();
    }

    private void DrawCheckboard(int width, int height)
    {
        GP.set_color(0.2f, 0.2f, 0.2f, 1.0f);
        GP.clear();
        GP.set_color(0.4f, 0.4f, 0.4f, 1.0f);
        for (int y = 0; y < height / 32 + 1; y++)
            for (int x = 0; x < width / 32 + 1; x++)
                if ((x + y) % 2 == 0)
                    GP.draw_filled_rect(x * 32, y * 32, 32, 32);
        GP.reset_color();
    }

    private void DrawRects(float ratio)
    {
        // work in a 60-unit-tall coordinate space, aspect corrected
        GP.project(0, 60 * ratio, 0, 60);
        GP.set_color(1.0f, 1.0f, 1.0f, 1.0f);

        DrawBlendCell(sgp_blend_mode.SGP_BLENDMODE_NONE, 0, 0);
        DrawBlendCell(sgp_blend_mode.SGP_BLENDMODE_BLEND, 20, 0);
        DrawBlendCell(sgp_blend_mode.SGP_BLENDMODE_BLEND_PREMULTIPLIED, 40, 0);
        DrawBlendCell(sgp_blend_mode.SGP_BLENDMODE_ADD, 20, 20);
        DrawBlendCell(sgp_blend_mode.SGP_BLENDMODE_ADD_PREMULTIPLIED, 40, 20);
        DrawBlendCell(sgp_blend_mode.SGP_BLENDMODE_MOD, 20, 40);
        DrawBlendCell(sgp_blend_mode.SGP_BLENDMODE_MUL, 40, 40);
    }

    private void DrawBlendCell(sgp_blend_mode mode, float tx, float ty)
    {
        GP.set_blend_mode(mode);
        GP.push_transform();
        GP.translate(tx, ty);
        Draw3Rects(1.0f, 0.5f);
        GP.pop_transform();
    }

    private void Draw3Rects(float brightness, float alpha)
    {
        GP.translate(2.5f, 2.5f);
        GP.set_color(brightness, 0.0f, 0.0f, alpha);
        GP.draw_filled_rect(0, 0, 10, 10);
        GP.set_color(0.0f, brightness, 0.0f, alpha);
        GP.draw_filled_rect(0, 5, 10, 10);
        GP.set_color(0.0f, 0.0f, brightness, alpha);
        GP.draw_filled_rect(5, 2.5f, 10, 10);
    }
}
