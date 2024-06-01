namespace Zinc.Sandbox.Demos;

[DemoScene("01 Texture")]
public class Texture : Scene
{
    public override void Create()
    {
        var tex = Res.Assets.conscript.CreateSprite();
        tex.X = Engine.Width / 2f;
        tex.Y = Engine.Height / 2f;
        tex.PivotX = 256;
        tex.PivotY = 256;
        
        foreach (var line in Depot.Generated.depot.testSheet.Lines)
        {
            Console.WriteLine(line.ID);
        }
    }
}