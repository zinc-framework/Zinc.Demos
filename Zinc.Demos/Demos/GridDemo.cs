using Zinc.Core;

namespace Zinc.Sandbox.Demos;

[DemoScene("13 Grid")]
public class GridDemo : Scene
{
    Color shape1c = Palettes.ENDESGA[9];
    Color shape2c = Palettes.ENDESGA[8];
    Grid g,h;
    int dim = 8;
    public override void Create()
    {
        g = new Grid(cellWidth:dim, cellHeight:dim,update: (self, dt) =>{
            self.Rotation += (float)dt;
        });

        for (int i = 0; i < dim * dim; i++)
        {
            g.AddChild(new Shape(16,16,shape1c){Name = $"shape{i}"});
        }

        h = new Grid(cellWidth:dim, cellHeight:dim,update: (self, dt) =>{
            self.Rotation -= (float)dt;
        });

        for (int i = 0; i < dim * dim; i++)
        {
            h.AddChild(new Shape(16,16,shape2c){Name = $"shape{i}"});
        }
    }

    public override void Update(double dt)
    {
        //ping pong math function:
        // float pingPong = MathF.Abs(MathF.Sin((float)Engine.Time));
        float pingPong = MathF.Sin((float)Engine.Time) + 2f;
        // g.CellHeight = (int)(pingPong * 16);
        // g.CellWidth = (int)(pingPong * 16);
        // h.CellHeight = (int)(pingPong * 16);
        // h.CellWidth = (int)(pingPong * 16);

        g.ScaleX = pingPong;
        g.ScaleY = pingPong;
        h.ScaleX = pingPong;
        h.ScaleY = pingPong;
        g.CellWidth = (pingPong * 16);
        g.CellHeight = (pingPong * 16);
        h.CellWidth = (pingPong * 16);
        h.CellHeight = (pingPong * 16);


        ImGUI.Window("gridPos",() =>
        {
            float cw = g.CellWidth;
            float ch = g.CellHeight;
            ImGUI.SliderFloat("cell_width", ref cw, 1, 128,"");
            ImGUI.SliderFloat("cell_height", ref ch, 1, 128,"");
            g.CellWidth = cw;
            g.CellHeight = ch;
        });

    }
}