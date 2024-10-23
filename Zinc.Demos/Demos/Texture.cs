using Arch.Core.Extensions;

namespace Zinc.Sandbox.Demos;

[DemoScene("01 Texture")]
public class Texture : Scene
{
    public override void Create()
    {
        var tex = Res.Assets.conscript.ToSprite();
        tex.Update = (self, dt) =>
        {
            ((Sprite)self).Rotation += (float)dt;
        };
    }
}