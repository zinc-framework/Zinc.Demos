using Arch.Core.Extensions;

namespace Zinc.Sandbox.Demos;

[DemoScene("01 Texture")]
public class Texture : Scene
{
    public override void Create()
    {
        var tex = Res.Assets.conscript.ToSprite();
        tex.X = Engine.Width / 2f;
        tex.Y = Engine.Height / 2f;
        tex.Update = (self, dt) =>
        {
            ((Sprite)self).Renderer_Rotation += (float)dt;
        };
    }
}