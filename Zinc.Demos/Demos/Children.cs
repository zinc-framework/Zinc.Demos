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
        Engine.showStats = false;
        for (int i = 0; i < shapes.Capacity; i++)
        {
            if(i == 0)
            {
                shapes.Add(new Shape(width:32,height:32)
                {
                    Name = $"Shape{i}",
                    X = Engine.Width / 2f,
                    Y = Engine.Height / 2f,
                    Collider_Active = true,
                    Collider_OnMouseEnter = (self, mods) =>
                    {
                        ((Shape)self).Renderer_Color = Palettes.GetRandomColor();
                    },
                    Collider_OnStart = (self, other) =>
                    {
                        ((Shape)self).Renderer_Color = Palettes.GetRandomColor();
                    },
                });
            }
            else
            {
                shapes.Add(new Shape(width:32,height:32,parent: shapes[i-1])
                {
                    Name = $"Shape{i}",
                    X = 48,
                    Y = 48,
                    Collider_Active = true,
                    Collider_OnMouseEnter = (self, mods) =>
                    {
                        ((Shape)self).Renderer_Color = Palettes.GetRandomColor();
                    },
                });
            }
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