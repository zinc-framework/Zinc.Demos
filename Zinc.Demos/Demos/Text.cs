namespace Zinc.Sandbox.Demos;
using static Zinc.Quick;
using static Zinc.Internal.Sokol.App;
using static Zinc.Internal.Sokol.Fontstash;
using Zinc.Internal.Sokol;
using System.Runtime.Intrinsics.X86;

[DemoScene("21 Text")]
public class TextDemo : Scene
{
    Text testText;
    public override void Create()
    {
        testText = new Text("hello text",$"{AppContext.BaseDirectory}/data/fonts/droidserif/DroidSerif-Regular.ttf");
    }

    float rotationSpeed = 0f;
    float spacing = 1f;
    float blur = 0f;
    float fontSize = 32f;
    float scaleX = 1f;
    float scaleY = 1f;
    public override void Update(double dt)
    {
        var txt = testText.Renderer_text;
        Core.ImGUI.Window("display text", () =>
        {
            Core.ImGUI.TextInput("display text", ref txt);
            Core.ImGUI.SliderFloat("size", ref fontSize, 1f, 200f, "");
            Core.ImGUI.SliderFloat("rotation speed", ref rotationSpeed, 0, 5f, "");
            Core.ImGUI.SliderFloat("char spacing", ref spacing, -10f, 10f, "");
            Core.ImGUI.SliderFloat("blur", ref blur, 0f, 10f, "");
            Core.ImGUI.SliderFloat("scale x", ref scaleX, 1f, 10f, "");
            Core.ImGUI.SliderFloat("scale y", ref scaleY, 1f, 10f, "");
        });

        testText.Rotation += rotationSpeed;
        testText.Renderer_size = fontSize;
        testText.Renderer_text = txt;
        testText.Renderer_spacing = spacing;
        testText.Renderer_blur = blur;
        testText.ScaleX = scaleX;
        testText.ScaleY = scaleY;
    }
}