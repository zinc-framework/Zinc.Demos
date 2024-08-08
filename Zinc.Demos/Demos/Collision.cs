using System.Numerics;
using Arch.Core.Extensions;

namespace Zinc.Sandbox.Demos;

[DemoScene("12 Collision")]
public class Collision : Scene
{
    Color pt = Palettes.ENDESGA[1];
    Color no_collide = Palettes.ENDESGA[7];
    Color collide = Palettes.ENDESGA[3];

    private Shape static_colliderA;
    private Shape static_colliderB;
    private Shape static_colliderC;
    private Shape ptA;
    private Shape ptB;
    private Shape ptC;
    public override void Create()
    {
        static_colliderA = new Shape(){
            Name = "static_colliderA",
            X = Engine.Width/2f,
            Y = Engine.Height/2f,
            Color = no_collide,
            Collider_OnStart = (self, other) =>
            {
                self.Entity.Get<ShapeRenderer>().Color = collide;
            },
            Collider_OnEnd = (self, other) =>
            {
                self.Entity.Get<ShapeRenderer>().Color = no_collide;
            },
            Collider_Active = true,
        };

        static_colliderB = new Shape(){
            Name = "static_colliderB",
            X = Engine.Width/2f - 200f,
            Y = Engine.Height/2f - 200f,
            PivotX = 16,
            PivotY = 16,
            Color = no_collide,
            Collider_OnStart = (self, other) =>
            {
                self.Entity.Get<ShapeRenderer>().Color = collide;
            },
            Collider_OnEnd = (self, other) =>
            {
                self.Entity.Get<ShapeRenderer>().Color = no_collide;
            },
            Collider_Active = true,
        };

        static_colliderC = new Shape(){
            Name = "static_colliderC",
            X = Engine.Width/2f + 200f,
            Y = Engine.Height/2f - 200f,
            PivotX = 16,
            PivotY = 16,
            Color = no_collide,
            Collider_OnStart = (self, other) =>
            {
                self.Entity.Get<ShapeRenderer>().Color = collide;
            },
            Collider_OnEnd = (self, other) =>
            {
                self.Entity.Get<ShapeRenderer>().Color = no_collide;
            },
            Collider_Active = true,
        };

        ptA = new Shape(){Name ="ptA", Color = pt, Width = 5, Height = 5};
        ptB = new Shape(){Name ="ptB", Color = pt, Width = 5, Height = 5};;
        ptC = new Shape(){Name ="ptC", Color = pt, Width = 5, Height = 5};;
    }

    public override void Update(double dt)
    {
        static_colliderA.Rotation = (float)Engine.Time;
        static_colliderA.ScaleX = MathF.Sin((float)Engine.Time) + 2;
        static_colliderA.ScaleY = MathF.Sin((float)Engine.Time) + 2;
        static_colliderA.X = Engine.Width/2f + MathF.Cos((float)Engine.Time) * 50;
        // static_colliderB.Rotation = (float)Engine.Time;
        // static_colliderB.ScaleX = MathF.Sin((float)Engine.Time) + 2;
        // static_colliderB.ScaleY = MathF.Sin((float)Engine.Time) + 2;
        static_colliderC.Rotation = (float)Engine.Time;
        static_colliderC.ScaleX = MathF.Sin((float)Engine.Time) + 2;
        static_colliderC.ScaleY = MathF.Sin((float)Engine.Time) + 2;
        
        // raw way to get collision data instead of relying on the system callbacks
        // static_collider.Color = CollisionChecks.CheckCollision(pointer, static_collider)
        //     ? collide
        //     : no_collide;
        var ptsA = Zinc.Collision.GetClosestPoints(Engine.Cursor, static_colliderA);
        ptA.X = ptsA.b.Value.X;
        ptA.Y = ptsA.b.Value.Y;
        var ptsB = Zinc.Collision.GetClosestPoints(Engine.Cursor, static_colliderB);
        ptB.X = ptsB.b.Value.X;
        ptB.Y = ptsB.b.Value.Y;
        var ptsC = Zinc.Collision.GetClosestPoints(Engine.Cursor, static_colliderC);
        ptC.X = ptsC.b.Value.X;
        ptC.Y = ptsC.b.Value.Y;
    }
}