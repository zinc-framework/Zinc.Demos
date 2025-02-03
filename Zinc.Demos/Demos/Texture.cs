using Arch.Core.Extensions;

namespace Zinc.Sandbox.Demos;

[DemoScene("01 Texture")]
public class Texture : Scene
{
    public override void Create()
    {
        //use res folder
        var sprite = Res.Assets.conscript.ToSprite();
        Quick.Center(sprite);
        sprite.Update = (self, dt) =>
        {
            ((Sprite)self).Rotation += (float)dt;
        };

        //could also load by path:
        // var tex = new Resources.Texture("res/conscript.png");
        // var sprite = new Sprite(new SpriteData(tex,tex.GetFullRect()));
    }
}