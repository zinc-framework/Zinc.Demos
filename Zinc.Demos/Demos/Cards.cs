using System.Collections;
using System.Numerics;
using System.Transactions;
using Arch.Core.Extensions;
using Zinc.Core;

namespace Zinc.Sandbox.Demos;

[DemoScene("17 Cards")]
public class Cards : Scene
{
    public record GridPos(int x) : Tag($"GRID:{x}");
    public Tag held = "held";
    public Tag positioned = "positioned";
    Shape s;
    Grid g;
    public override void Create()
    {
        g = new Grid(128,128,8,1);
        for (int i = 0; i < 7; i++)
        {
            g.AddChild(new Shape(64,128,Palettes.ENDESGA[9]){
                Name = $"shape{i}",
                Tags = [new GridPos(i)],
                Collider_Width = 64,
                Collider_Height = 128,
                Collider_Active = true,
                Collider_OnStart = (self, other) =>
                {
                    if(other == s)
                    {
                        ((Shape)self).Renderer_Color = Palettes.ENDESGA[3];
                    }
                },
                Collider_OnEnd = (self, other) =>
                {
                    if(other == s)
                    {
                        ((Shape)self).Renderer_Color = Palettes.ENDESGA[9];
                    }
                }
            });
        }

        s = new Shape(64,128,Palettes.ENDESGA[8]){
            Name = "test card",
            X = 100,
            Y = 100,
            RenderOrder = -1,
            Collider_Active = true,
            Collider_OnMouseDown = (self, mods) =>
            {
                s.X = InputSystem.MouseX;
                s.Y = InputSystem.MouseY;
                self.Tag(held);
                self.Untag(positioned);
            },
            Collider_OnMouseUp = (self, mods) =>
            {
                self.Untag(held);
            },
            
            Collider_OnContinue = (self, other) =>
            {
                Console.WriteLine($"{self.Tagged(held,positioned)}");
                if(other.HasTag<GridPos>() && self.NotTagged(held,positioned))
                {
                    var aa = (other as Anchor).GetWorldPosition();
                    new Coroutine(MoveTestCardToLocation(aa));
                    self.Tag(positioned);
                }
            }
        };
    }

    IEnumerator MoveTestCardToLocation(Vector2 loc)
    {
        yield return new Vector2Tween(new Vector2(s.X,s.Y),loc,Easing.EaseOutBounce)
        {
            Duration = 0.5f,
            ValueUpdated = (v) => {s.X = v.X; s.Y = v.Y;}
        };
        yield return null;
    }
}