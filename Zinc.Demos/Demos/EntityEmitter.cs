using System.Numerics;
using Zinc.Core.ImGUI;
using Zinc.Internal.Sokol;
using static Zinc.Quick;

namespace Zinc.Sandbox.Demos;

[DemoScene("11 Entity Emitter")]
public class EntityEmitter : Scene
{
    double timer = 0;
    private int emissionRate = 1;
    public override void Update(double dt)
    {
        ImGUIHelper.Wrappers.Begin("mods",ImGuiWindowFlags_.ImGuiWindowFlags_None);
        ImGUIHelper.Wrappers.SliderInt("rate", ref emissionRate, 1, 10, "", ImGuiSliderFlags_.ImGuiSliderFlags_Logarithmic);
        ImGUIHelper.Wrappers.End();
        timer += dt;
        if (timer > 0.00001)
        {
            var startPos = new Vector2(InputSystem.MouseX, InputSystem.MouseY);
            for (int i = 0; i < emissionRate; i++)
            {
                var rand = RandUnitCirclePos();
                var dx = rand.X * 4;
                var dy = rand.Y * 4;
                new Shape(update:(self, dt) =>
                {
                    self.X += dx;
                    self.Y += dy;
                }) 
                {
                    Renderer_Color = new Color(Palettes.ENDESGA[Quick.Random.Next(Palettes.ENDESGA.Count)]),
                    X = startPos.X,
                    Y = startPos.Y,
                    Collider_Active = false
                };
            }
            timer = 0;
        }
    }
}