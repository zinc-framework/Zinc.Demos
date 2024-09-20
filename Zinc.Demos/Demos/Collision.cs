using System.Numerics;
using Arch.Core.Extensions;

namespace Zinc.Sandbox.Demos;

[DemoScene("12 Collision")]
public class Collision : Scene
{
    Color pt = Palettes.ENDESGA[1];
    Color no_collide = Palettes.ENDESGA[7];
    Color collide = Palettes.ENDESGA[3];
    public override void Create()
    {
        var static_colliderA = new Shape(update:(self, dt) =>
            {
                self.Rotation = (float)Engine.Time;
                self.ScaleX = MathF.Sin((float)Engine.Time) + 2;
                self.ScaleY = MathF.Sin((float)Engine.Time) + 2;
                self.X = Engine.Width/2f + MathF.Cos((float)Engine.Time) * 50;
            }){
            Name = "static_colliderA",
            X = Engine.Width/2f,
            Y = Engine.Height/2f,
            Renderer_Color = no_collide,
            Collider_OnStart = (self, other) =>
            {
                ((Shape)self).Renderer_Color = collide;
            },
            Collider_OnEnd = (self, other) =>
            {
                ((Shape)self).Renderer_Color = no_collide;
            },
            Collider_Active = true,
        };

        var static_colliderB = new Shape(){
            Name = "static_colliderB",
            X = Engine.Width/2f - 200f,
            Y = Engine.Height/2f - 200f,
            Renderer_Color = no_collide,
            Collider_OnStart = (self, other) =>
            {
                ((Shape)self).Renderer_Color = collide;
            },
            Collider_OnEnd = (self, other) =>
            {
                ((Shape)self).Renderer_Color = no_collide;
            },
            Collider_Active = true,
        };

        var static_colliderC = new Shape(update:(self, dt) =>
            {
                self.Rotation = (float)Engine.Time;
                self.ScaleX = MathF.Sin((float)Engine.Time) + 2;
                self.ScaleY = MathF.Sin((float)Engine.Time) + 2;
            }){
            Name = "static_colliderC",
            X = Engine.Width/2f + 200f,
            Y = Engine.Height/2f - 200f,
            Renderer_Color = no_collide,
            Collider_OnStart = (self, other) =>
            {
                ((Shape)self).Renderer_Color = collide;
            },
            Collider_OnEnd = (self, other) =>
            {
                ((Shape)self).Renderer_Color = no_collide;
            },
            Collider_Active = true,
        };

        var ptA = new Shape(5,5,update:(self,dt) => {
            var pt = Zinc.Collision.GetClosestPoints(Engine.Cursor, static_colliderA);
            self.X = pt.b.Value.X;
            self.Y = pt.b.Value.Y;
        }){Name ="ptA", Renderer_Color = pt};

        var ptB = new Shape(5,5,update:(self,dt) => {
            var pt = Zinc.Collision.GetClosestPoints(Engine.Cursor, static_colliderB);
            self.X = pt.b.Value.X;
            self.Y = pt.b.Value.Y;
        } ){Name ="ptB", Renderer_Color = pt};

        var ptC = new Shape(5,5,update:(self,dt) => {
            var pt = Zinc.Collision.GetClosestPoints(Engine.Cursor, static_colliderC);
            self.X = pt.b.Value.X;
            self.Y = pt.b.Value.Y;
        }){Name ="ptC", Renderer_Color = pt};
    }
}