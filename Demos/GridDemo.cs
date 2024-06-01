using Zinc.Core;

namespace Zinc.Sandbox.Demos;

[DemoScene("13 Grid")]
public class GridDemo : Scene
{
    Color shape1c = Palettes.ENDESGA[9];
    Color shape2c = Palettes.ENDESGA[8];
    List<Shape> shapes1 = new();
    List<Shape> shapes2 = new();
    Grid g = new Grid(new (new(Engine.Width / 2f, Engine.Height / 2f), new(0.5f, 0.5f), 10, 10, new(0.5f, 0.5f), 30, 30));
    public override void Create()
    {
        foreach (var p in g.Points)
        {
            shapes1.Add(new Shape(shape1c,5,5) { X = (int)p.X, Y = (int)p.Y, ColliderActive = false, PivotX = 2.5f, PivotY = 2.5f});
            shapes2.Add(new Shape(shape2c,5,5) { X = (int)p.X, Y = (int)p.Y, ColliderActive = false, PivotX = 2.5f, PivotY = 2.5f});
        }
    }

    public override void Update(double dt)
    {
        g.TransformGrid((float)Engine.Time,1f,1f);
        g.ApplyPositionsToEntites(shapes1);
        g.TransformGrid(-(float)Engine.Time,MathF.Sin((float)Engine.Time) + 2,MathF.Sin((float)Engine.Time) + 2);
        g.ApplyPositionsToEntites(shapes2);
    }
}