namespace Zinc.Sandbox.Demos;
using static Zinc.Quick;

[DemoScene("03 Animation")]
public class Animation : Scene
{
    private Resources.Texture conscriptImage;
    private AnimatedSpriteData animatedConscript;
    public override void Preload()
    {
        conscriptImage = new Resources.Texture("res/conscript.png");
        var rects = Quick.CreateTextureSlices(512, 512, 64, 64);
        animatedConscript = new AnimatedSpriteData(
            conscriptImage,
            new() { new("test", rects[..3],
                0.4f) });
    }

    public override void Create()
    {
        new AnimatedSprite(animatedConscript){X = Engine.Width/2f,Y = Engine.Height/2f};
    }
}