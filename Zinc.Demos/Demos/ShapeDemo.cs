using static Zinc.Core.ImGUI.ImGUIHelper;

namespace Zinc.Sandbox.Demos;

[DemoScene("08 Shape")]
public class ShapeDemo : Scene
{
    public override void Create()
    {
        new Shape(update: (self, dt) =>
        {
            Wrappers.Window("rot",() =>
            {
                float rotation = self.Rotation;
                float scaleX = self.ScaleX;
                float scaleY = self.ScaleY;
                Wrappers.SliderFloat("rotation", ref rotation, 0, 3.14f,"",Internal.Sokol.ImGuiSliderFlags_.ImGuiSliderFlags_None);
                Wrappers.SliderFloat("scaleX", ref scaleX, 1, 3f,"",Internal.Sokol.ImGuiSliderFlags_.ImGuiSliderFlags_None);
                Wrappers.SliderFloat("scaleT", ref scaleY, 1, 3f,"",Internal.Sokol.ImGuiSliderFlags_.ImGuiSliderFlags_None);
                self.Rotation = rotation;
                self.ScaleX = scaleX;
                self.ScaleY = scaleY;
            });
        })
        {
            X = Engine.Width / 2f,
            Y = Engine.Height / 2f,
        };
    }
}
