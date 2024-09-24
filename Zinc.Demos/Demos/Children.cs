using Volatile;
using Zinc.Core.ImGUI;

namespace Zinc.Sandbox.Demos;

[DemoScene("17 Children")]
public class ChildrenDemo : Scene
{
    List<Shape> shapes = new List<Shape>(4);
    float scaleX = 1;
    float scaleY = 1;
    public override void Create()
    {
        for (int i = 0; i < shapes.Capacity; i++)
        {
            shapes.Add(new Shape(width:32,height:32,parent: i > 0 ? shapes[i-1] : null, update:(self, dt) =>
            {
                // self.Rotation += Quick.RandFloat() * (float)dt;
                // self.Renderer_Rotation += Quick.RandFloat() * (float)dt;
            })
            {
                Name = $"Shape{i}",
                X = i == 0 ? Engine.Width / 2f : 48,
                Y = i == 0 ? Engine.Height / 2f : 48,
            });
        }
    }

    public override void Update(double dt)
    {
        ImGUIHelper.Wrappers.SetNextWindowSize(500,500);
        ImGUIHelper.Wrappers.Window($"rot", Internal.Sokol.ImGuiWindowFlags_.ImGuiWindowFlags_None, () =>{
            
            ImGUIHelper.Wrappers.SliderFloat($"scaleX", ref scaleX, 1, 32, "", Internal.Sokol.ImGuiSliderFlags_.ImGuiSliderFlags_None);
            ImGUIHelper.Wrappers.SliderFloat($"scaleY", ref scaleY, 1, 32, "", Internal.Sokol.ImGuiSliderFlags_.ImGuiSliderFlags_None);
            shapes[0].ScaleX = scaleX;
            shapes[0].ScaleY = scaleY;
            // shapes[0].ScaleY = dim;
            
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