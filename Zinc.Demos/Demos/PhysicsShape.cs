using System.Numerics;

namespace Zinc.Sandbox.Demos;

[DemoScene("09 PhysicsShape")]
public class PhysicsShape : Scene
{
    Dictionary<Shape, PhysicsBody> bods = new();

    public override void Create()
    {
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
            var a = new Shape()
            {
                Renderer_Color = new Color(Palettes.ENDESGA[Quick.Random.Next(Palettes.ENDESGA.Count)]),
                X = (int)startPos.X,
                Y = (int)startPos.Y,
                Collider_Active = false
            };
            var bod = Engine.PhysicsWorld.CreateDynamicBody(startPos);
            bod.AddBoxShape(32f, 32f, density: 1f);
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
