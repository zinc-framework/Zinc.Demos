using System.Numerics;
using Zinc.Core;
using Zinc.Core.ImGUI;
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
        // emitter.Emitter_Config = emitter.Emitter_Config with {EmissionRate = 0.1f};
        MoveToMouse(emitter);
        var rot = emitter.Rotation;
        // emitter.Emitter_Config.Gravity = StandardGravity * 100f;
        emitter.Emitter_Config.Gravity = StandardGravity;
        emitter.Emitter_Config.InitialMassFunc = () => 300.1f;
        // emitter.Emitter_Config.Color.StartValue = = new (new Color(1,1,0,0),new Color(1,0,1,0),Easing.Option.EaseInOutExpo);
        emitter.Emitter_Config.InitialAcclerationFunc = () => Vector2.Zero;
        emitter.Emitter_Config.InitialSpeedFunc = () => (RandFloat() + 0.5f) * 500f;
        // emitter.Emitter_Config.InitialEmissionDirectionFunc = () => RandUnitPos(MathF.PI * 0.4f,MathF.PI * 0.6f);
        emitter.Emitter_Config.InitialEmissionDirectionFunc = () => RandUnitCirclePos();
        DrawEditGUIForObject("emitter",ref config);
        
        // Color start = emitter.Emitter_Config.Color.StartValue;
        // Color end = emitter.Emitter_Config.Color.TargetValue;
        // ImGUIHelper.Wrappers.Color("start", ref emitter.Emitter_Config.Color.StartValue);
        // ImGUIHelper.Wrappers.Color("end", ref end);
        // ImGUIHelper.Wrappers.Color("test", ref test);
        // emitter.Emitter_Config.Color.StartValue = start;
        // emitter.Emitter_Config.Color.TargetValue = end;
        
        ImGUIHelper.Wrappers.SliderFloat("Rotation", ref rot, 0, 360,"",Internal.Sokol.ImGuiSliderFlags_.ImGuiSliderFlags_None);
        emitter.Rotation = rot;
        // var rand = RandUnitCircle();
        // emitter.Config.particleConfig.DX.StartValue = rand.x * 4;
        // emitter.Config.particleConfig.DY.StartValue = rand.y * 4;
    }
}