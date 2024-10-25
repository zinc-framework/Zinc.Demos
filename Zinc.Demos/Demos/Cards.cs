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
    Tag card = "card";
    Tag held = "held";
    Tag positioned = "positioned";
    Tag moving = "moving";
    Grid g;
    public override void Create()
    {
        g = new Grid(128,128,8,1);
        for (int i = 0; i < 8; i++)
        {
            g.AddChild(new Shape(64,128,Palettes.ENDESGA[9]){
                Name = $"shape{i}",
                Tags = [new GridPos(i)],
                Collider_Width = 64,
                Collider_Height = 128,
                Collider_Active = true,
                Collider_OnStart = (self, other) =>
                {
                    if(other.HasTag(card))
                    {
                        ((Shape)self).Renderer_Color = Palettes.ENDESGA[3];
                    }
                },
                Collider_OnEnd = (self, other) =>
                {
                    if(other.HasTag(card))
                    {
                        ((Shape)self).Renderer_Color = Palettes.ENDESGA[9];
                    }
                }
            });
        }

        CreateCard(100,100);

        InputSystem.Events.Key.Down += KeyDownListener;
    }

    void CreateCard(float x, float y)
    {
        new Shape(64,128,Palettes.ENDESGA[8]){
            Name = "test card",
            X = x,
            Y = y,
            RenderOrder = -1,
            Collider_Active = true,
            Tags = [card],
            Collider_OnMouseDown = (self, mods) =>
            {
                if(!self.Tagged(moving))
                {
                    var a = self as Anchor;
                    a.X = InputSystem.MouseX;
                    a.Y = InputSystem.MouseY;
                    self.Tag(held);
                    self.Untag(positioned);
                }
            },
            Collider_OnMouseUp = (self, mods) =>
            {
                self.Untag(held);
            },
            
            Collider_OnContinue = (self, other) =>
            {
                if(other.HasTag<GridPos>() && self.NotTagged(held,positioned,moving))
                {
                    var aa = (other as Anchor).GetWorldPosition();
                    self.Tag(moving);
                    new Coroutine(MoveTestCardToLocation(self as Anchor,aa));
                    self.Tag(positioned);
                }
            }
        };
    }

    void KeyDownListener(Key key, List<Modifiers> mods)
    {
        if(key == Key.SPACE)
        {
            CreateCard(InputSystem.MouseX,InputSystem.MouseY);
        }
    }

    public override void Cleanup()
    {
        InputSystem.Events.Key.Down -= KeyDownListener;
    }

    IEnumerator MoveTestCardToLocation(Anchor cardEntity, Vector2 loc)
    {
        yield return new Vector2Tween(new Vector2(cardEntity.X,cardEntity.Y),loc,Easing.EaseOutBounce)
        {
            Duration = 0.5f,
            ValueUpdated = (v) => {cardEntity.X = v.X; cardEntity.Y = v.Y;}
        };
        cardEntity.Untag(moving);
        yield return null;
    }
}