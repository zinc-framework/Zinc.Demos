using System.Collections;
using System.Numerics;
using Arch.Core.Extensions;

namespace Zinc.Sandbox.Demos;

[DemoScene("16 Coroutines")]
public class Coroutines : Scene
{
    Shape s;
    public override void Create()
    {
        s = new Shape(){
            X = Engine.Width/2f,
            Y = Engine.Height/2f,
            Renderer_Color = Palettes.ENDESGA[1],
            Collider_Active = false
        };
        
        new Coroutine(Patrol(),"patrol");
        // Core.Coroutines.Start(Patrol(),"patrol");
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
            yield return new WaitForSeconds(2);
        }
    }

    public IEnumerator MoveToLocation(Vector2 loc)
    {
        while(MathF.Abs(s.X-loc.X) > 1 && MathF.Abs(s.Y-loc.Y) > 1)
        {
            //get the direction to the target location
            var dir = Vector2.Normalize(loc - new Vector2(s.X, s.Y));
            //move the shape in that direction
            s.X += dir.X;
            s.Y += dir.Y;
            yield return null;
        }
        yield return null;
    }
}