using Zinc.Internal.Sokol;
using static System.MathF;

namespace Zinc.Sandbox.Demos;

// Port of sokol_gp's sample-primitives.c (https://github.com/edubart/sokol_gp).
// Exercises most of the GP immediate-mode API in four viewports: filled rects
// with transforms, a point grid, line strips + line segments, and filled/raw
// triangles with per-vertex color. All drawing happens in Update(), which the
// engine runs inside its per-frame GP.begin()/GP.flush() window.
[DemoScene("SGP_Example_Primitives")]
public class SGP_Example_Primitives : Scene
{
    public override unsafe void Update(double dt)
    {
        int width = Engine.Width, height = Engine.Height;
        int hw = width / 2;
        int hh = height / 2;

        // background
        GP.set_color(0.05f, 0.05f, 0.05f, 1.0f);
        GP.clear();
        GP.reset_color();

        // top left: rects, once unclipped and once scissored
        GP.viewport(0, 0, hw, hh);
        GP.set_color(0.1f, 0.1f, 0.1f, 1.0f);
        GP.clear();
        GP.reset_color();
        GP.push_transform();
        GP.translate(0.0f, -hh / 4.0f);
        DrawRects();
        GP.pop_transform();
        GP.push_transform();
        GP.translate(0.0f, hh / 4.0f);
        GP.scissor(0, 0, hw, (int)(3.0f * hh / 4.0f));
        DrawRects();
        GP.reset_scissor();
        GP.pop_transform();

        // top right: triangles
        GP.viewport(hw, 0, hw, hh);
        DrawTriangles();

        // bottom left: points
        GP.viewport(0, hh, hw, hh);
        DrawPoints();

        // bottom right: lines
        GP.viewport(hw, hh, hw, hh);
        GP.set_color(0.1f, 0.1f, 0.1f, 1.0f);
        GP.clear();
        GP.reset_color();
        DrawLines();

        // restore the full-frame viewport for the rest of the engine's frame
        GP.reset_viewport();
    }

    private unsafe void DrawRects()
    {
        sgp_irect viewport = GP.query_state()->viewport;
        int width = viewport.w, height = viewport.h;
        int size = 64;
        int hsize = size / 2;
        float time = (float)Engine.Time;
        float t = (1.0f + Sin(time)) / 2.0f;

        // left: bounces with a translate
        GP.push_transform();
        GP.translate(width * 0.25f - hsize, height * 0.5f - hsize);
        GP.translate(0.0f, 2 * size * t - size);
        GP.set_color(t, 0.3f, 1.0f - t, 1.0f);
        GP.draw_filled_rect(0, 0, size, size);
        GP.pop_transform();

        // middle: spins via rotate_at
        GP.push_transform();
        GP.translate(width * 0.5f - hsize, height * 0.5f - hsize);
        GP.rotate_at(time, hsize, hsize);
        GP.set_color(t, 1.0f - t, 0.3f, 1.0f);
        GP.draw_filled_rect(0, 0, size, size);
        GP.pop_transform();

        // right: pulses via scale_at
        GP.push_transform();
        GP.translate(width * 0.75f - hsize, height * 0.5f - hsize);
        GP.scale_at(t + 0.25f, t + 0.5f, hsize, hsize);
        GP.set_color(0.3f, t, 1.0f - t, 1.0f);
        GP.draw_filled_rect(0, 0, size, size);
        GP.pop_transform();
    }

    private unsafe void DrawPoints()
    {
        GP.set_color(1.0f, 1.0f, 1.0f, 1.0f);
        sgp_irect viewport = GP.query_state()->viewport;
        int width = viewport.w, height = viewport.h;
        sgp_vec2* points = stackalloc sgp_vec2[4096];
        int count = 0;
        for (int y = 64; y < height - 64 && count < 4096; y += 8)
            for (int x = 64; x < width - 64 && count < 4096; x += 8)
                points[count++] = new sgp_vec2 { x = x, y = y };
        GP.draw_points(points, (uint)count);
    }

    private unsafe void DrawLines()
    {
        GP.set_color(1.0f, 1.0f, 1.0f, 1.0f);
        sgp_irect viewport = GP.query_state()->viewport;
        sgp_vec2 c = new() { x = viewport.w / 2.0f, y = viewport.h / 2.0f };

        // spiral
        sgp_vec2* points = stackalloc sgp_vec2[4096];
        int count = 0;
        points[count++] = c;
        for (float theta = 0.0f; theta <= PI * 8.0f; theta += PI / 16.0f)
        {
            float r = 10.0f * theta;
            points[count++] = new sgp_vec2 { x = c.x + r * Cos(theta), y = c.y + r * Sin(theta) };
        }
        GP.draw_lines_strip(points, (uint)count);

        // x
        GP.push_transform();
        GP.translate(viewport.w / 2, viewport.h / 2);
        int x_size = 32;
        sgp_line* xline = stackalloc sgp_line[2];
        xline[0] = new sgp_line { a = new sgp_vec2 { x = -x_size, y = -x_size }, b = new sgp_vec2 { x = x_size, y = x_size } };
        xline[1] = new sgp_line { a = new sgp_vec2 { x = x_size, y = -x_size }, b = new sgp_vec2 { x = -x_size, y = x_size } };
        GP.draw_lines(xline, 2);
        GP.pop_transform();
    }

    private unsafe void DrawTriangles()
    {
        float time = (float)Engine.Time;
        sgp_irect viewport = GP.query_state()->viewport;
        int width = viewport.w, height = viewport.h;

        float hw = width * 0.5f;
        float hh = height * 0.5f;
        float w = height * 0.2f;
        float ax = hw - w, ay = hh + w;
        float bx = hw, by = hh - w;
        float cx = hw + w, cy = hh + w;

        // single triangle
        GP.set_color(1.0f, 0.0f, 1.0f, 1.0f);
        GP.push_transform();
        GP.translate(-w * 1.5f, 0.0f);
        GP.draw_filled_triangle(ax, ay, bx, by, cx, cy);

        // hexagon as a triangle strip
        GP.translate(w * 3.0f, -hh * 0.5f);
        GP.set_color(0.0f, 1.0f, 1.0f, 1.0f);
        {
            float step = (2.0f * PI) / 6.0f;
            int count = 0;
            sgp_vec2* points = stackalloc sgp_vec2[4096];
            for (float theta = 0.0f; theta <= 2.0f * PI + step * 0.5f; theta += step)
            {
                points[count++] = new sgp_vec2 { x = hw + w * Cos(theta), y = hh - w * Sin(theta) };
                if (count % 3 == 1)
                    points[count++] = new sgp_vec2 { x = hw, y = hh };
            }
            GP.draw_filled_triangles_strip(points, (uint)count);
        }

        // color wheel via raw per-vertex colored triangle strip
        GP.translate(0.0f, hh);
        GP.set_color(1.0f, 1.0f, 1.0f, 1.0f);
        {
            float step = (2.0f * PI) / 64.0f;
            int count = 0;
            sgp_vertex* verts = stackalloc sgp_vertex[4096];
            for (float theta = 0.0f; theta <= 2.0f * PI + step * 0.5f; theta += step)
            {
                verts[count].position = new sgp_vec2 { x = hw + w * Cos(theta), y = hh - w * Sin(theta) };
                verts[count].color = new sgp_color_ub4
                {
                    r = (byte)((Sin(theta + time * 1) + 1.0f) * 0.5f * 255.0f),
                    g = (byte)((Sin(theta + time * 2) + 1.0f) * 0.5f * 255.0f),
                    b = (byte)((Sin(theta + time * 4) + 1.0f) * 0.5f * 255.0f),
                    a = 255,
                };
                count++;
                if (count % 3 == 1)
                {
                    verts[count].position = new sgp_vec2 { x = hw, y = hh };
                    verts[count].color = new sgp_color_ub4 { r = 255, g = 255, b = 255, a = 255 };
                    count++;
                }
            }
            GP.draw(sg_primitive_type.SG_PRIMITIVETYPE_TRIANGLE_STRIP, verts, (uint)count);
        }
        GP.pop_transform();
    }
}
