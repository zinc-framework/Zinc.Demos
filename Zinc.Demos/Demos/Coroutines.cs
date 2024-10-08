using System.Collections;
using System.Numerics;
using System.Transactions;
using Arch.Core.Extensions;
using Zinc.Core;

namespace Zinc.Sandbox.Demos;

[DemoScene("16 Coroutines")]
public class Coroutines : Scene
{
    Shape s;
    Coroutine c;
    public override void Create()
    {
        s = new Shape(){
            X = Engine.Width/2f,
            Y = Engine.Height/2f,
            Renderer_Color = Palettes.ENDESGA[1],
            Collider_Active = false
        };
        
        c = new Coroutine(Patrol(),"patrol");
    }

    public IEnumerator Patrol()
    {
        while(true)
        {
            var nextPos = new Vector2(Quick.RandFloat() * 800 + 100,Quick.RandFloat() * 800 + 100);
            new TemporaryShape(width:8,height:8,lifetime:2){
                X = nextPos.X,
                Y = nextPos.Y,
                Renderer_Color = Palettes.ENDESGA[3]
            };
            yield return MoveToLocation(nextPos);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public IEnumerator MoveToLocation(Vector2 loc)
    {
        // version that uses a tween to move the shape to the target location
        yield return new Vector2Tween(new Vector2(s.X,s.Y),loc,Easing.EaseOutBounce)
        {
            Duration = 1f,
            ValueUpdated = (v) => {s.X = v.X; s.Y = v.Y;}
        };

        /*
            version that creates a tween and samples it over time in a while loop
            
            Vector2Tween posTween = new Vector2Tween(new Vector2(s.X,s.Y),loc,Easing.EaseInOutQuad);
            double t = 0;
            float timeToDest = 1f;
            while(t < timeToDest)
            {
                t += Engine.DeltaTime;
                var sample = posTween.Sample(t/timeToDest);
                s.X = sample.X;
                s.Y = sample.Y;
                yield return null;
            }
        */

        /*
            basic version that just moves the shape in a straight line to the target location without a tween
            
            while(MathF.Abs(s.X-loc.X) > 1 && MathF.Abs(s.Y-loc.Y) > 1)
            {
                //get the direction to the target location
                var dir = Vector2.Normalize(loc - new Vector2(s.X, s.Y));
                //move the shape in that direction
                s.X += dir.X;
                s.Y += dir.Y;
                yield return null;
            }
        */

        yield return null;
    }

    public override void Update(double dt)
    {
        
        Core.ImGUI.ImGUIHelper.Wrappers.Window("coroutine controls",() =>
        {
            Core.ImGUI.ImGUIHelper.Wrappers.Button("Start Coroutine",new Vector2(120,20),() => c.Start());
            Core.ImGUI.ImGUIHelper.Wrappers.Button("Pause Coroutine",new Vector2(120,20),() => c.Pause());
            Core.ImGUI.ImGUIHelper.Wrappers.Button("Reset Coroutine",new Vector2(120,20),() => c.Reset());
        });
    }
}