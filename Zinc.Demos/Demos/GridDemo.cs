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
    int dim = 8;
    public override void Create()
    {
        g = new Grid(cellWidth:dim, cellHeight:dim,update: (self, dt) =>{
            self.Rotation += (float)dt;
        })
        {
            X = Engine.Width / 2f,
            Y = Engine.Height / 2f,
        };

        Console.WriteLine(g.GetChildren().Count);

        for (int i = 0; i < dim * dim; i++)
        {
            g.AddChild(new Shape(16,16){Name = $"shape{i}"});
        }
    }

    public override void Update(double dt)
    {
        ImGUIHelper.Wrappers.Window("gridPos",Internal.Sokol.ImGuiWindowFlags_.ImGuiWindowFlags_None,() =>
        {
            int cw = g.CellWidth;
            int ch = g.CellHeight;
            ImGUIHelper.Wrappers.SliderInt("cell_width", ref cw, 1, 128,"",Internal.Sokol.ImGuiSliderFlags_.ImGuiSliderFlags_None);
            ImGUIHelper.Wrappers.SliderInt("cell_height", ref ch, 1, 128,"",Internal.Sokol.ImGuiSliderFlags_.ImGuiSliderFlags_None);
            g.CellWidth = cw;
            g.CellHeight = ch;
        });

    }
}