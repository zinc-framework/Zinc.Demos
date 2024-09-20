using Volatile;

namespace Zinc.Sandbox.Demos;

[DemoScene("08 Shape")]
public class ShapeDemo : Scene
{
    public override void Create()
    {
        new Shape()
        {
            X = Engine.Width / 2f,
            Y = Engine.Height / 2f,
        };
    }
}
