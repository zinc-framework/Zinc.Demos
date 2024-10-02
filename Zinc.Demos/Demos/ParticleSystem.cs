using System.Numerics;
using Zinc.Core;
using Zinc.Core.ImGUI;
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
    ParticleEmitterConfig config = ParticleEmitterConfig.DefaultConfig;
    public override void Create()
    {
        var baseConfg = ParticleEmitterConfig.DefaultConfig;
        
        emitter = new ParticleEmitter(10000,config){
            X = 400,
            Y = 400,
        };
    }

    public override void Update(double time)
    {
        // emitter.Emitter_Config = emitter.Emitter_Config with {EmissionRate = 0.1f};
        // MoveToMouse(emitter);
        var rot = emitter.Rotation;
        emitter.Emitter_Config.Gravity = StandardGravity * 10f;
        emitter.Emitter_Config.InitialMassFunc = () => 0.1f;
        emitter.Emitter_Config.InitialAcclerationFunc = () => Vector2.Zero;
        emitter.Emitter_Config.InitialSpeedFunc = () => RandFloat() * 20f;
        emitter.Emitter_Config.InitialEmissionDirectionFunc = () => UnitUp * 10;
        DrawEditGUIForObject("emitter",ref config);
        ImGUIHelper.Wrappers.SliderFloat("Rotation", ref rot, 0, 360,"",Internal.Sokol.ImGuiSliderFlags_.ImGuiSliderFlags_None);
        emitter.Rotation = rot;
        // var rand = RandUnitCircle();
        // emitter.Config.particleConfig.DX.StartValue = rand.x * 4;
        // emitter.Config.particleConfig.DY.StartValue = rand.y * 4;
    }
}