using Volatile;
using Zinc.Core.ImGUI;

namespace Zinc.Sandbox.Demos;

[DemoScene("17 Children")]
public class ChildrenDemo : Scene
{
    List<Shape> shapes = new List<Shape>();
    public override void Create()
    {
        for (int i = 0; i < 4; i++)
        {
            shapes.Add(new Shape(parent: i > 0 ? shapes[i-1] : null)
            {
                Name = $"Shape{i}",
                X = i == 0 ? Engine.Width / 2f : 48,
                Y = i == 0 ? Engine.Height / 2f : 48,
            });
        }
    }

    public override void Update(double dt)
    {
        ImGUIHelper.Wrappers.Window($"rot", Internal.Sokol.ImGuiWindowFlags_.ImGuiWindowFlags_None, () =>{
            foreach (var shape in shapes)
            {
                float r = shape.Rotation;
                ImGUIHelper.Wrappers.SliderFloat($"{shape.Name} rot", ref r, 0, 6.28f, "", Internal.Sokol.ImGuiSliderFlags_.ImGuiSliderFlags_None);
                shape.Rotation = r;
            }
        });
    }
}


public record Prefab(IComponent[] Components)
{
    public void Spawn()
    {
        // new Entity(Components);
    }
};

public partial record SpritePrefab() : Prefab([
    new Position(),
    new SpriteRenderer()
]);

public static class Prefabs
{
    public static Prefab ShapePrefab = new Prefab([

        new Position()
    ]);
}