using Zinc.Internal.Sokol;
using static System.MathF;

namespace Zinc.Sandbox.Demos;

// Port of sokol_gp's sample-rectangle.c. A single rectangle spun about the
// origin in a normalized, aspect-corrected coordinate space set up with
// GP.project, with a color that animates over time.
[DemoScene("SGP_Example_Rectangle")]
public class SGP_Example_Rectangle : Scene
{
    public override void Update(double dt)
    {
        int width = Engine.Width, height = Engine.Height;
        float ratio = width / (float)height;

        GP.viewport(0, 0, width, height);
        // map the framebuffer to (-ratio..ratio, 1..-1) so (0,0) is centered
        GP.project(-ratio, ratio, 1.0f, -1.0f);
        GP.set_color(0.1f, 0.1f, 0.1f, 1.0f);
        GP.clear();

        float time = (float)Engine.Time;
        float r = Sin(time) * 0.5f + 0.5f;
        float g = Cos(time) * 0.5f + 0.5f;
        GP.set_color(r, g, 0.3f, 1.0f);
        GP.rotate_at(time, 0.0f, 0.0f);
        GP.draw_filled_rect(-0.5f, -0.5f, 1.0f, 1.0f);

        // restore engine defaults for the remainder of the frame
        GP.reset_color();
        GP.reset_transform();
        GP.reset_project();
    }
}
