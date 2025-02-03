using Volatile;

namespace Zinc.Sandbox.Demos;

[DemoScene("07 Physics")]
public class Physics : Scene
{
    //TODO: make physics and actual system to get rid of the jank setters
    private SpriteData conscript;
    public override void Preload()
    {
        conscript = Res.Assets.conscript.Texture.Slice(new Rect(0,0,64,64));
        VoltConfig.AreaMassRatio = 0.000007f;
    }
    
    Dictionary<Sprite, VoltBody> bods = new Dictionary<Sprite, VoltBody>();
    public override void Create()
    {
        var bot = new Vector2(0, Engine.Height / 2f);
        var poly = Engine.PhysicsWorld.CreatePolygonWorldSpace(
            new Vector2[]
            {
                new Vector2(0, Engine.Height),
                new Vector2(0, Engine.Height + 100),
                new Vector2(Engine.Width, Engine.Height + 100),
                new Vector2(Engine.Width, Engine.Height)
            }, 0f);
        var bod = Engine.PhysicsWorld.CreateStaticBody(bot, 0f, new[] { poly });
        bod.Set(bot,0f);

    }

    double timer = 0;
    public override void Update(double dt)
    {
        timer += dt;
        if (timer > 0.1)
        {
            var startPos = new Vector2(InputSystem.MouseX, InputSystem.MouseY);
            var a = new Sprite(conscript) {
                X = (int)startPos.x,
                Y = (int)startPos.y,
                Collider_Active = false
            };
            var poly = Engine.PhysicsWorld.CreatePolygonWorldSpace(
                [
                    new Vector2(startPos.x, startPos.y),
                    new Vector2(startPos.x, startPos.y + 64),
                    new Vector2(startPos.x + 64, startPos.y + 64),
                    new Vector2(startPos.x + 64, startPos.y)
                ],1f);
            var bod = Engine.PhysicsWorld.CreateDynamicBody(startPos, 0f, new []{poly});
            bods.Add(a,bod);
            // bod.AddForce(new Vector2(0,9.8f));
            // a.SetVelocity(-2,0);
            timer = 0;
        }

        foreach (var b in bods)
        {
            b.Value.AddForce(new Vector2(0,9.8f));
            b.Key.X = (int)b.Value.Position.x;
            b.Key.Y = (int)b.Value.Position.y;
        }
    }
}