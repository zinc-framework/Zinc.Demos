using System.Numerics;

namespace Zinc.Sandbox.Demos;

[DemoScene("07 Physics")]
public class Physics : Scene
{
    private SpriteData conscript;
    public override void Preload()
    {
        conscript = Res.Assets.conscript.Texture.Slice(new Rect(0, 0, 64, 64));
    }

    Dictionary<Sprite, PhysicsBody> bods = new();

    public override void Create()
    {
        // Ground: a wide static box sitting at the bottom of the window.
        var groundCenter = new Vector2(Engine.Width / 2f, Engine.Height + 50f);
        var ground = Engine.PhysicsWorld.CreateStaticBody(groundCenter);
        ground.AddBoxShape(Engine.Width, 100f);
    }

    double timer = 0;
    public override void Update(double dt)
    {
        timer += dt;
        if (timer > 0.1)
        {
            var startPos = new Vector2(InputSystem.MouseX, InputSystem.MouseY);
            var a = new Sprite(conscript)
            {
                X = (int)startPos.X,
                Y = (int)startPos.Y,
                Collider_Active = false
            };
            var bod = Engine.PhysicsWorld.CreateDynamicBody(startPos);
            bod.AddBoxShape(64f, 64f, density: 1f);
            bods.Add(a, bod);
            timer = 0;
        }

        foreach (var b in bods)
        {
            b.Key.X = (int)b.Value.Position.X;
            b.Key.Y = (int)b.Value.Position.Y;
        }
    }
}
