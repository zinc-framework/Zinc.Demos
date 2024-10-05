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
        Transition<float> xtrans = new Transition<float>(s.X,loc.X,Easing.Option.EaseInOutCirc);
        Transition<float> ytrans = new Transition<float>(s.Y,loc.Y,Easing.Option.EaseInOutCirc);

        double t = 0;
        float timeToDest = 1f;
        while(MathF.Abs(s.X-loc.X) > 1 && MathF.Abs(s.Y-loc.Y) > 1)
        {
            t += Engine.DeltaTime;
            float sample = (float)t/timeToDest;
            float sampleX = (float)xtrans.Sample(sample);
            float sampleY = (float)ytrans.Sample(sample);
            s.X = Quick.MapF(sampleX, 0f, 1f, xtrans.StartValue, xtrans.TargetValue);
            s.Y = Quick.MapF(sampleY, 0f, 1f, ytrans.StartValue, ytrans.TargetValue);
            yield return null;

            // //get the direction to the target location
            // var dir = Vector2.Normalize(loc - new Vector2(s.X, s.Y));
            // //move the shape in that direction
            // s.X += dir.X;
            // s.Y += dir.Y;
            // yield return null;
        }
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