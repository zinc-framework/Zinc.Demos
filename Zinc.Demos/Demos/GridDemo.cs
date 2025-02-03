using Zinc.Core;

namespace Zinc.Sandbox.Demos;

[DemoScene("13 Grid")]
public class GridDemo : Scene
{
    Color shape1c = Palettes.ENDESGA[9];
    Color shape2c = Palettes.ENDESGA[8];
    Grid g,h;
    int dim = 64;
    public override void Create()
    {
        g = new Grid(cellWidth:dim, cellHeight:dim,update: (self, dt) =>{
            self.Rotation += (float)dt;
        });
        Quick.Center(g);

        for (int i = 0; i < dim * dim; i++)
        {
            g.AddChild(new Shape(dim/2,dim/2,shape1c){Name = $"shape{i}"});
        }

        h = new Grid(cellWidth:dim, cellHeight:dim,update: (self, dt) =>{
            self.Rotation -= (float)dt;
        });
        Quick.Center(h);

        for (int i = 0; i < dim * dim; i++)
        {
            h.AddChild(new Shape(dim/2,dim/2,shape2c){Name = $"shape{i}"});
        }
    }

    public override void Update(double dt)
    {
        float pingPong = MathF.Sin((float)Engine.Time) + 2f;

        g.CellWidth = pingPong * dim;
        g.CellHeight = pingPong * dim;
        g.ScaleX = pingPong;
        g.ScaleY = pingPong;

        h.CellWidth = pingPong* dim;
        h.CellHeight = pingPong* dim;
        h.ScaleX = pingPong;
        h.ScaleY = pingPong;
    }
}