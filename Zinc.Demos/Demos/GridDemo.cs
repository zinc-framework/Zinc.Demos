using Zinc.Core;
using Zinc.Core.ImGUI;

namespace Zinc.Sandbox.Demos;

[DemoScene("13 Grid")]
public class GridDemo : Scene
{
    // Color shape1c = Palettes.ENDESGA[9];
    // Color shape2c = Palettes.ENDESGA[8];
    // List<Shape> shapes1 = new();
    // List<Shape> shapes2 = new();
    Grid g;
    public override void Create()
    {
        g = new Grid(update: (self, dt) =>{
            // self.Rotation += (float)dt;
        })
        {
            X = Engine.Width / 2f,
            Y = Engine.Height / 2f,
        };

        for (int i = 0; i < 10; i++)
        {
            // g.AddChild(new Shape(4,4));
            g.AddChild(new Shape(4,4,parent:g));
        }
    }

    public override void Update(double dt)
    {
        ImGUIHelper.Wrappers.Window("gridPos",Internal.Sokol.ImGuiWindowFlags_.ImGuiWindowFlags_None,() =>
        {
            float X = g.X;
            float Y = g.Y;
            float R = g.Rotation;
            ImGUIHelper.Wrappers.SliderFloat("rotation", ref R, 0, 6.28f,"",Internal.Sokol.ImGuiSliderFlags_.ImGuiSliderFlags_None);
            ImGUIHelper.Wrappers.SliderFloat("X", ref X, 1, 500f,"",Internal.Sokol.ImGuiSliderFlags_.ImGuiSliderFlags_None);
            ImGUIHelper.Wrappers.SliderFloat("Y", ref Y, 1, 500f,"",Internal.Sokol.ImGuiSliderFlags_.ImGuiSliderFlags_None);
            g.Rotation = R;
            g.X = X;
            g.Y = Y;

            int cw = g.CellWidth;
            int ch = g.CellHeight;
            ImGUIHelper.Wrappers.SliderInt("cell_width", ref cw, 1, 15,"",Internal.Sokol.ImGuiSliderFlags_.ImGuiSliderFlags_None);
            ImGUIHelper.Wrappers.SliderInt("cell_height", ref ch, 1, 15,"",Internal.Sokol.ImGuiSliderFlags_.ImGuiSliderFlags_None);
            g.CellWidth = cw;
            g.CellHeight = ch;

            Console.WriteLine($"g.X: {g.X}, g.Y: {g.Y}, g.Rotation: {g.Rotation}");
        });
    }
}