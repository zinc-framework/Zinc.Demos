using System.Numerics;
using Zinc.Core;
using static Zinc.Quick;

namespace Zinc.Sandbox.Demos;

[DemoScene("11 Entity Emitter")]
public class EntityEmitter : Scene
{
    private int emissionRate = 1;
    public override void Create()
    {
        Loop(0.00001f,() => {
            var startPos = new Vector2(InputSystem.MouseX, InputSystem.MouseY);
            for (int i = 0; i < emissionRate; i++)
            {
                var rand = RandUnitCirclePos() * 4;
                new Shape(update:(self, dt) =>
                {
                    self.X += rand.X;
                    self.Y += rand.Y;
                }) 
                {
                    X = startPos.X,
                    Y = startPos.Y,
                    Collider_Active = false
                };
            }
        });
    }
    public override void Update(double dt)
    {
        ImGUI.Window("controls", () => {
            ImGUI.SliderInt("rate", ref emissionRate, 1, 10, "", ImGUI.SliderFlags.Logarithmic);
        });
        
    }
}