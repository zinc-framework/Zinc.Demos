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


        //create a "deck"
        new Shape(64,128,Palettes.ENDESGA[12]){
            Name = "deck",
            X = 100,
            Y = 100,
            RenderOrder = -1,
            Collider_Active = true,
            Tags = [deck],
            Collider_OnMousePressed = (self, mods) =>
            {
                //"draw" a card
                // placementRowAnchors.GetGridPosition(0);
                var c = CreateCard(100,100);
                c.Tag(moving);
                // new Coroutine(MoveCardToLocation(c,new Vector2(185,100)));
                float x,y;
                placementRowAnchors.GetLocalGridPosition(gridPos,out x,out y);
                new Coroutine(MoveCardToLocation(c,new Vector2(placementRowAnchors.X + x, placementRowAnchors.Y + y)));
                gridPos = (gridPos + 1) % 8;
            },
        };

        InputSystem.Events.Key.Down += KeyDownListener;
    }

    int gridPos = 0;

    Shape CreateCard(float x, float y)
    {
        return new Shape(64,128,Palettes.ENDESGA[8]){
            Name = "test card",
            X = x,
            Y = y,
            RenderOrder = 0,
            Collider_Active = true,
            Tags = [card],
            Collider_OnMouseDown = (self, mods) =>
            {
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
            },
            
            Collider_OnContinue = (self, other) =>
            {
                if(other.HasTag<GridPos>() && self.NotTagged(held,positioned,moving))
                {
                    var aa = (other as Anchor).GetWorldPosition();
                    self.Tag(moving);
                    new Coroutine(MoveCardToLocation(self as Anchor,aa));
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