using Zinc.Core;
using static Zinc.Quick;

namespace Zinc.Sandbox.Demos;

[DemoScene("14 Particle System")]
public class ParticleSystem : Scene
{
    // Color startColor = Palettes.ENDESGA[4];
    private Color startColor = new Color(1.0f, 1.0f, 0f, 0f);
    private Color endColor = new Color(1.0f, 0.0f, 1.0f, 0f);
    // Color endColor = Palettes.ENDESGA[16];
    ParticleEmitter emitter;
    public override void Create()
    {
        emitter = new ParticleEmitter(
            new(100000, 100, new ParticleEmitterComponent.ParticleConfig()
            {
                Color = new Transition<Color>(startColor,endColor,Easing.Option.EaseInOutExpo)
            }))
        {
            X = 200,
            Y = 200
        };
    }

    public override void Update(double time)
    {
        MoveToMouse(emitter);
        DrawEditGUIForObject("emitter",ref emitter);
        // var rand = RandUnitCircle();
        // emitter.Config.particleConfig.DX.StartValue = rand.x * 4;
        // emitter.Config.particleConfig.DY.StartValue = rand.y * 4;
    }
}