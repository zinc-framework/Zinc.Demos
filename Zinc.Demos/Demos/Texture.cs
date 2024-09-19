using Arch.Core.Extensions;

namespace Zinc.Sandbox.Demos;

[DemoScene("01 Texture")]
public class Texture : Scene
{
    public override void Create()
    {
        var tex = Res.Assets.conscript.ToSprite();
        foreach (var c in tex.ECSEntity.GetAllComponents())
        {
            Console.WriteLine(c);
        }
        Console.WriteLine(tex.ECSEntity.Version()); // tex.ECSEntity.Version
        Console.WriteLine(tex.ECSEntityReference.Version); // tex.ECSEntity.Version
        tex.X = Engine.Width / 2f;
        tex.Y = Engine.Height / 2f;
        tex.PivotX = 256;
        tex.PivotY = 256;
        tex.Update = (self, dt) =>
        {
            ((Anchor)self).Rotation += (float)dt;
            // Console.WriteLine(((Anchor)self).Rotation);
        };
    }
}