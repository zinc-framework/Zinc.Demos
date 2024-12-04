using Arch.Core;
using Arch.Core.Extensions;

namespace Zinc.Sandbox.Demos;
using static Zinc.Quick;

[DemoScene("06 Bunnymark")]
public class BunnyMark : Scene
{
    private SpriteData logo = Res.Assets.logo.ToSpriteData();
    private BunnySystem system = new BunnySystem();
    public override void Preload()
    {
        Engine.RegisterSystem(system);
    }

    int bunnies = 10000;
    public override void Create()
    {
        for (int i = 0; i < bunnies; i++)
        {
            new Bunny(logo);
        }
        Engine.DebugTextStr = $"{bunnies} buns";
        InputSystem.Events.Key.Down += KeyDownListener;
    }

    void KeyDownListener(Key key, List<Modifiers> mods)
    {
        int addedbuns = 1000;
        if (key == Key.SPACE)
        {
            for (int i = 0; i < addedbuns; i++)
            {
                new Bunny(logo);
            }
            bunnies += addedbuns;
            Engine.DebugTextStr = $"{bunnies} buns";
        }
    }

    public override void Cleanup()
    {
        InputSystem.Events.Key.Down -= KeyDownListener;
        Engine.UnregisterSystem(system);
    }
}

[Arch.AOT.SourceGenerator.Component]
public record struct BunnyMarkComponent(float VelX, float VelY) : IComponent;
[Component<BunnyMarkComponent>]
public partial class Bunny : Sprite
{
    public Bunny(SpriteData spriteData)
        : base(spriteData)
    {
        
        VelX = RandFloat() * 10;
        VelY = RandFloat() * 5;
    }
}


public class BunnySystem : DSystem, IUpdateSystem
{
    QueryDescription bunny = new QueryDescription().WithAll<Position,BunnyMarkComponent>();      // Should have all specified components
    public void Update(double dt)
    {
        float randCheck = 0;
        Engine.ECSWorld.Query(in bunny, (Arch.Core.Entity e, ref Position pos, ref BunnyMarkComponent bm) => {
            pos.X += bm.VelX;
            pos.Y += bm.VelY;
            
            bm.VelY += 9.8f;
            
            if (pos.X > Engine.Width)
            {
                bm.VelX *= -1;
                pos.X = Engine.Width;
            }
            else if (pos.X < 0)
            {
                bm.VelX *= -1;
                pos.X = 0;
            }
            
            if (pos.Y > Engine.Height)
            {
                bm.VelY *= -0.85f;
                pos.Y = Engine.Height;
                randCheck = Quick.RandFloat();
                if (randCheck > 0.5)
                {
                    bm.VelY -= (randCheck * 6);
                }
            }
            else if (pos.Y < 0)
            {
                bm.VelY = 0;
                pos.Y = 0;
            }
        });
    }
}