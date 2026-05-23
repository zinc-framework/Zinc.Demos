using Zinc.Internal.Sokol;
using static System.MathF;

namespace Zinc.Sandbox.Demos;

// Port of sokol_gp's sample-framebuffer.c (https://github.com/edubart/sokol_gp).
// Renders a spinning magenta fan into a 128x128 offscreen render target, then tiles that
// rendered texture across the window, rotating each tile (alternating draw_filled_rect and
// draw_textured_rect to show both texture-binding paths).
//
// The offscreen pass is a *nested* GP queue: GP.begin/end push/pop sokol_gp's state stack and
// base the nested command range at the current cursor, so GP.flush() inside the offscreen
// sg pass dispatches only the fan's commands. It all happens in Update(), which the engine runs
// after its outer GP.begin() but before the swapchain pass — so the offscreen pass completes
// first, and the screen tiles flush normally in the engine's main pass.
//
// Zinc's sokol_gfx uses the view-based attachment API: images -> sg_view (color/resolve/depth)
// -> sg_attachments. The offscreen target must match the color/depth format + sample count that
// sokol_gp baked into its pipelines (queried via GP.query_desc()), or the pass/pipeline formats
// won't validate.
[DemoScene("SGP_Example_Framebuffer")]
public class SGP_Example_Framebuffer : Scene
{
    private const int FB = 128;

    private sg_image fbColor;
    private sg_image fbResolve;   // single-sampled resolve target (only when MSAA is on)
    private sg_image fbDepth;     // depth attachment (only when sokol_gp uses a depth format)
    private sg_view colorView;
    private sg_view resolveView;
    private sg_view depthView;
    private sg_attachments fbAttachments;
    private sg_sampler linearSampler;
    private sg_image sampleImg;   // the image bound for sampling on screen
    private bool hasResolve;
    private bool hasDepth;

    public override unsafe void Create()
    {
        // Match the formats sokol_gp baked into its pipelines so the offscreen pass is compatible.
        sgp_desc d = GP.query_desc();
        sg_pixel_format colorFmt = d.color_format;
        sg_pixel_format depthFmt = d.depth_format;
        int sampleCount = d.sample_count;
        hasResolve = sampleCount > 1;
        hasDepth = depthFmt != sg_pixel_format.SG_PIXELFORMAT_NONE;

        // Color attachment image (multi-sampled when the swapchain is).
        sg_image_desc colorDesc = default;
        colorDesc.usage.color_attachment = 1;
        colorDesc.width = FB;
        colorDesc.height = FB;
        colorDesc.pixel_format = colorFmt;
        colorDesc.sample_count = sampleCount;
        fbColor = Gfx.make_image(&colorDesc);

        // Resolve image — single-sampled copy we can sample from, needed only under MSAA.
        if (hasResolve)
        {
            sg_image_desc resolveDesc = default;
            resolveDesc.usage.resolve_attachment = 1;
            resolveDesc.width = FB;
            resolveDesc.height = FB;
            resolveDesc.pixel_format = colorFmt;
            resolveDesc.sample_count = 1;
            fbResolve = Gfx.make_image(&resolveDesc);
        }

        // Depth attachment — sokol_gp's pipelines carry the environment depth format.
        if (hasDepth)
        {
            sg_image_desc depthDesc = default;
            depthDesc.usage.depth_stencil_attachment = 1;
            depthDesc.width = FB;
            depthDesc.height = FB;
            depthDesc.pixel_format = depthFmt;
            depthDesc.sample_count = sampleCount;
            fbDepth = Gfx.make_image(&depthDesc);
        }

        // Views over the images (the new sokol attachment API binds views, not images).
        sg_view_desc cvd = default;
        cvd.color_attachment.image = fbColor;
        colorView = Gfx.make_view(&cvd);

        if (hasResolve)
        {
            sg_view_desc rvd = default;
            rvd.resolve_attachment.image = fbResolve;
            resolveView = Gfx.make_view(&rvd);
        }
        if (hasDepth)
        {
            sg_view_desc dvd = default;
            dvd.depth_stencil_attachment.image = fbDepth;
            depthView = Gfx.make_view(&dvd);
        }

        fbAttachments = default;
        fbAttachments.colors.e0 = colorView;
        if (hasResolve) fbAttachments.resolves.e0 = resolveView;
        if (hasDepth) fbAttachments.depth_stencil = depthView;

        sg_sampler_desc smpDesc = default;
        smpDesc.min_filter = sg_filter.SG_FILTER_LINEAR;
        smpDesc.mag_filter = sg_filter.SG_FILTER_LINEAR;
        smpDesc.wrap_u = sg_wrap.SG_WRAP_CLAMP_TO_EDGE;
        smpDesc.wrap_v = sg_wrap.SG_WRAP_CLAMP_TO_EDGE;
        linearSampler = Gfx.make_sampler(&smpDesc);

        // Under MSAA we sample the resolve target; otherwise the color image is directly sampleable.
        sampleImg = hasResolve ? fbResolve : fbColor;
    }

    public override unsafe void Update(double dt)
    {
        int width = Engine.Width, height = Engine.Height;

        // dark background
        GP.set_color(0.05f, 0.05f, 0.05f, 1.0f);
        GP.clear();
        GP.reset_color();

        float time = (float)Engine.Time;
        GP.set_blend_mode(sgp_blend_mode.SGP_BLENDMODE_BLEND);

        // 1) render the spinning fan into the offscreen target
        DrawFbo();

        // 2) tile that rendered texture across the window, rotating each tile
        int i = 0;
        for (int y = 0; y < height; y += 192)
        {
            for (int x = 0; x < width; x += 192)
            {
                GP.push_transform();
                GP.rotate_at(time, x + 64, y + 64);
                GP.set_image(0, sampleImg);
                GP.set_sampler(0, linearSampler);
                if (i % 2 == 0)
                {
                    GP.draw_filled_rect(x, y, FB, FB);
                }
                else
                {
                    sgp_rect dest = new() { x = x, y = y, w = FB, h = FB };
                    sgp_rect src = new() { x = 0, y = 0, w = FB, h = FB };
                    GP.draw_textured_rect(0, dest, src);
                }
                GP.reset_image(0);
                GP.reset_sampler(0);
                GP.pop_transform();
                i++;
            }
        }
        GP.reset_blend_mode();
    }

    // Nested GP queue dispatched into the offscreen sg pass.
    private unsafe void DrawFbo()
    {
        GP.begin(FB, FB);
        GP.project(0, FB, FB, 0);
        DrawFan();

        sg_pass pass = default;
        pass.action.colors.e0.load_action = sg_load_action.SG_LOADACTION_CLEAR;
        pass.action.colors.e0.clear_value = new sg_color { r = 1f, g = 1f, b = 1f, a = 0.2f };
        pass.attachments = fbAttachments;
        Gfx.begin_pass(&pass);
        GP.flush();
        GP.end();
        Gfx.end_pass();
    }

    private unsafe void DrawFan()
    {
        sgp_irect viewport = GP.query_state()->viewport;
        int width = viewport.w, height = viewport.h;
        float hw = width * 0.5f, hh = height * 0.5f;
        float w = height * 0.3f;

        sgp_vec2* points = stackalloc sgp_vec2[4096];
        int count = 0;
        float step = (2.0f * PI) / 6.0f;
        for (float theta = 0.0f; theta <= 2.0f * PI + step * 0.5f; theta += step)
        {
            points[count++] = new sgp_vec2 { x = hw * 1.33f + w * Cos(theta), y = hh * 1.33f - w * Sin(theta) };
            if (count % 3 == 1)
                points[count++] = new sgp_vec2 { x = hw, y = hh };
        }
        GP.set_color(1.0f, 0.0f, 1.0f, 1.0f);
        GP.draw_filled_triangles_strip(points, (uint)count);
        GP.reset_color();
    }

    public override unsafe void Cleanup()
    {
        Gfx.destroy_view(colorView);
        Gfx.destroy_image(fbColor);
        if (hasResolve) { Gfx.destroy_view(resolveView); Gfx.destroy_image(fbResolve); }
        if (hasDepth) { Gfx.destroy_view(depthView); Gfx.destroy_image(fbDepth); }
        Gfx.destroy_sampler(linearSampler);
    }
}
