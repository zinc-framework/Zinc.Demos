using Arch.Core;
using Arch.Core.Extensions;

namespace Zinc.Sandbox.Demos;
using static Zinc.Quick;

[DemoScene("06 Bunnymark")]
public class BunnyMark : Scene
{
    private Resources.Texture logoImage;
    private SpriteData logo;
    private BunnySystem system = new BunnySystem();
    public override void Preload()
    {
        var logoImage = new Resources.Texture("res/logo.png");
        logo = new SpriteData(logoImage,logoImage.GetFullRect());
        Engine.RegisterSystem(system);
    }

    TestBunny b;
    int bunnies = 10000;
    public override void Create()
    {
        for (int i = 0; i < bunnies; i++)
        {
            b = new TestBunny(logo,RandFloat() * 10,RandFloat() * 10-5);
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
                b = new TestBunny(logo,RandFloat() * 10,RandFloat()*10-5);
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

public record struct BunnyMarkComponent(float x, float y);

public class TestBunny : SceneEntity
{
    public SpriteData Data { get; init; }
    public TestBunny(SpriteData spriteData, float velx, float vely, Scene? scene = null, bool startEnabled = true) : base(startEnabled,scene)
    {
        Data = spriteData;
        ECSEntity.Add(
            new SpriteRenderer(Data.Texture, Data.Rect),
            new BunnyMarkComponent(velx,vely));
    }
}

public class BunnySystem : DSystem, IUpdateSystem
{
    QueryDescription bunny = new QueryDescription().WithAll<ActiveState,EntityID,Position,BunnyMarkComponent>();      // Should have all specified components
    public void Update(double dt)
    {
        float randCheck = 0;
        Engine.ECSWorld.Query(in bunny, (Arch.Core.Entity e, ref Position pos, ref BunnyMarkComponent vel) => {
            pos.X += vel.x;
            pos.Y += vel.y;
            
            vel.y += 9.8f;
            
            if (pos.X > Engine.Width)
            {
                vel.x *= -1;
                pos.X = Engine.Width;
            }
            else if (pos.X < 0)
            {
                vel.x *= -1;
                pos.X = 0;
            }
            
            if (pos.Y > Engine.Height)
            {
                vel.y *= -0.85f;
                pos.Y = Engine.Height;
                randCheck = Quick.RandFloat();
                if (randCheck > 0.5)
                {
                    vel.y -= (randCheck * 6);
                }
            }
            else if (pos.Y < 0)
            {
                vel.y = 0;
                pos.Y = 0;
            }
        });
    }
}