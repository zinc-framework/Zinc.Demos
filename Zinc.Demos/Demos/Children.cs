using Zinc.Core;

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
            shapes.Add(new Shape(width:32,height:32,parent: i > 0 ? shapes[i-1] : null)
            {
                Name = $"Shape{i}",
                X = i == 0 ? Engine.Width / 2f : 48,
                Y = i == 0 ? Engine.Height / 2f : 48,
            });
        }
    }

    public override void Update(double dt)
    {
        ImGUI.Window($"rot",() =>{
            
            ImGUI.SliderFloat($"scaleX", ref scaleX, 1, 32, "");
            ImGUI.SliderFloat($"scaleY", ref scaleY, 1, 32, "");
            shapes[0].ScaleX = scaleX;
            shapes[0].ScaleY = scaleY;
            // shapes[0].ScaleY = dim;
            
            foreach (var shape in shapes)
            {
                float r = shape.Rotation;
                ImGUI.SliderFloat($"{shape.Name} rot", ref r, 0, 6.28f, "");
                shape.Rotation = r;

                // float xoff = 0;
                // ImGUIHelper.Wrappers.SliderFloat($"{shape.Name} xoff", ref xoff, 0, 6.28f, "", Internal.Sokol.ImGuiSliderFlags_.ImGuiSliderFlags_None);
                // shape.X = (shape.Name == "Shape0" ? Engine.Width / 2f : 48) + xoff;
            }
        });
    }
}