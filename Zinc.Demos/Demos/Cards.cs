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
    bool currentlyMovingCard = false;
    Tag card = "card";
    Tag deck = "deck";
    Tag held = "held";
    Tag positioned = "positioned";
    Tag moving = "moving";
    Grid placementRowAnchors;
    public override void Create()
    {
        placementRowAnchors = new Grid(128,128,8,1);
        for (int i = 0; i < 8; i++)
        {
            placementRowAnchors.AddChild(new Shape(64,128,Palettes.ENDESGA[9]){
                Name = $"shape{i}",
                Tags = [new GridPos(i)],
                Collider_Width = 64,
                Collider_Height = 128,
                Collider_Active = true,
                RenderOrder = 100000,
                Collider_OnStart = (self, other) =>
                {
                    if(other.Tagged(card))
                    {
                        ((Shape)self).Renderer_Color = Palettes.ENDESGA[3];
                    }
                },
                Collider_OnEnd = (self, other) =>
                {
                    if(other.Tagged(card))
                    {
                        ((Shape)self).Renderer_Color = Palettes.ENDESGA[9];
                    }
                }
            });
        }

        //create a "deck"
        new Shape(64,128,Palettes.ONE_BIT_MONITOR_GLOW[1]){
            Name = "deck",
            X = 100,
            Y = 100,
            RenderOrder = -1,
            Collider_Active = true,
            Tags = [deck],
            //clicking the deck draws a card
            Collider_OnMousePressed = (self, mods) =>
            {
                //"draw" a card
                // placementRowAnchors.GetGridPosition(0);
                var c = CreateCard(100,100);
                c.Tag(moving);
                new Coroutine(MoveCardToLocation(c,new Vector2(100 + 80, 100)));
            },
        };
    }

    Shape CreateCard(float x, float y)
    {
        return new Shape(64,128){
            Name = "test card",
            X = x,
            Y = y,
            RenderOrder = 0,
            Collider_Active = true,
            Tags = [card],
            Collider_OnMouseDown = (self, mods) =>
            {
                // if(self.NotTagged(moving) && !currentlyMovingCard)
                if(self.NotTagged(moving))
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
                //need to sourcegen this to forward to colliders
                if((self as Shape).ECSEntity.Get<Collider>().CollidingWithTagged<GridPos>(out var collisions))
                {
                    var target = collisions.First();
                    var aa = (target as Anchor);
                    self.Tag(moving);
                    self.Tag(positioned);
                    new Coroutine(MoveCardToLocation(self as Anchor,new Vector2(aa.X,aa.Y)));
                }
            }
        };
    }

    IEnumerator MoveCardToLocation(Anchor cardEntity, Vector2 loc)
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