namespace Zinc.Sandbox.Demos;

[DemoScene("02 Texture Frame")]
public class TextureFrame : Scene
{
    private Resources.Texture conscriptImage;
    private SpriteData conscriptFrame0;
    public override void Preload()
    {
        // conscriptImage = new Resources.Texture("res/conscript.png");
        
        // conscriptImage = Res.Assets.conscript.Texture;
        // conscriptFrame0 = new(conscriptImage, new Rect(0,0,64,64));

        conscriptFrame0 = Res.Assets.conscript.Texture.Slice(new Rect(0,0,64,64));

    }

    public override void Create()
    {
        Quick.Center(new Sprite(conscriptFrame0));
    }
}