using System.Numerics;
using Zinc.Core;

namespace Zinc.Sandbox.Demos;

[DemoScene("04 Simple Update")]
public class SimpleUpdate : Scene
{
    private Resources.Texture conscriptImage;
    private SpriteData conscriptFrame0;
    public override void Preload()
    {
        conscriptImage = new Resources.Texture("res/conscript.png");
        
        conscriptFrame0 = new(conscriptImage, new Rect(0,0,64,64));
    }

    Sprite e;
    Vector2 startPos = Vector2.Zero;
    public override void Create()
    {
        startPos = new((Engine.Width / 2f) - 32, (Engine.Height / 2f) - 32);
        e = new Sprite(conscriptFrame0)
        {
            X = (int)startPos.X,
            Y = (int)startPos.Y,
            PivotX = 32,
            PivotY = 32
        };
    }

    public override void Update(double dt)
    {
        e.X = (int)startPos.X + (int)(Math.Sin(Engine.Time) * 100);
        e.Rotation = (float)Engine.Time;
        var scale = (Math.Cos(Engine.Time) * 2);
        e.ScaleX = (float)scale;
        e.ScaleY = (float)scale;
    }
}