using static System.MathF;
using Zinc.Internal.Sokol;

namespace Zinc.Sandbox.Demos;

// Idiomatic Zinc port of SGP_Example_Framebuffer.
// The raw version hand-rolls sokol images/views/attachments + a nested GP pass, then tiles the result
// with GP.draw_textured_rect. Here a RenderTarget hides all the offscreen-pass plumbing, and the tiles
// are ordinary Sprite entities that sample the target's Texture and spin via their Rotation property.
// (The fan drawn into the target is procedural, so it stays immediate-mode inside target.Render.)
[DemoScene("SGP_Zinc_Framebuffer")]
public class SGP_Zinc_Framebuffer : Scene
{
    private const int FB = 128;
    private RenderTarget target;

    public override void Create()
    {
        target = new RenderTarget(FB, FB);
        var data = new SpriteData(target.Texture, target.Texture.GetFullRect());

        // tile the target's texture across the window with rotating Sprite entities
        for (int y = 64; y < Engine.Height; y += 192)
            for (int x = 64; x < Engine.Width; x += 192)
                new Sprite(data, update: (s, dt) => s.Rotation = (float)Engine.Time)
                {
                    X = x,
                    Y = y,
                };
    }

    public override void Update(double dt)
    {
        // render the spinning fan into the offscreen target each frame
        target.Render(DrawFan, clear: new Color(1f, 1f, 1f, 0.2f));
    }

    private unsafe void DrawFan()
    {
        // This target is sampled as a texture, so flip Y in our own draw (GPU texture sampling is
        // bottom-up) — same as the raw sokol_gp framebuffer sample. RenderTarget itself imposes no
        // projection; the default would be upright (for CPU readback), which we override here.
        GP.project(0, FB, FB, 0);
        float hw = FB * 0.5f, hh = FB * 0.5f, w = FB * 0.3f;
        sgp_vec2* points = stackalloc sgp_vec2[4096];
        int count = 0;
        float step = (2f * PI) / 6f;
        for (float theta = 0f; theta <= 2f * PI + step * 0.5f; theta += step)
        {
            points[count++] = new sgp_vec2 { x = hw * 1.33f + w * Cos(theta), y = hh * 1.33f - w * Sin(theta) };
            if (count % 3 == 1) points[count++] = new sgp_vec2 { x = hw, y = hh };
        }
        GP.set_color(1f, 0f, 1f, 1f);
        GP.draw_filled_triangles_strip(points, (uint)count);
        GP.reset_color();
    }

    public override void Cleanup() => target.Dispose();
}
