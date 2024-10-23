using static Zinc.Core.ImGUI;

namespace Zinc.Sandbox.Demos;

[DemoScene("08 Shape")]
public class ShapeDemo : Scene
{
    public override void Create()
    {
        new Shape(update: (self, dt) =>
        {
            Window("rot",() =>
            {
                float rotation = self.Rotation;
                float scaleX = self.ScaleX;
                float scaleY = self.ScaleY;
                SliderFloat("rotation", ref rotation, 0, 3.14f,"");
                SliderFloat("scaleX", ref scaleX, 1, 3f,"");
                SliderFloat("scaleT", ref scaleY, 1, 3f,"");
                self.Rotation = rotation;
                self.ScaleX = scaleX;
                self.ScaleY = scaleY;
            });
        });
    }
}
