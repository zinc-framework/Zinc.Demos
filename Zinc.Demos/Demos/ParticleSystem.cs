using System.Numerics;
using Zinc.Core;
using static Zinc.Quick;

namespace Zinc.Sandbox.Demos;

[DemoScene("14 Particle System")]
public class ParticleSystem : Scene
{
    Color startColor = Palettes.ENDESGA[4];
    Color endColor = Palettes.ENDESGA[16];
    ParticleEmitter emitter;
    ParticleEmitterConfig config = ParticleEmitterConfig.DefaultConfig;
    public override void Create()
    {
        config.Color.StartValue = startColor;
        config.Color.TargetValue = endColor;
        emitter = new ParticleEmitter(10000,config){
            X = 400,
            Y = 400,
        };
    }

    public override void Update(double time)
    {
        MoveToMouse(emitter);
        emitter.Emitter_Config.Gravity = StandardGravity * 10f;
        emitter.Emitter_Config.InitialMassFunc = () => 3f;
        emitter.Emitter_Config.InitialSpeedFunc = () => (RandFloat() + 0.5f) * 500f;
        emitter.Emitter_Config.InitialEmissionDirectionFunc = () => RandUnitCirclePos();
        DrawEditGUIForObject("emitter",ref config);
    }
}