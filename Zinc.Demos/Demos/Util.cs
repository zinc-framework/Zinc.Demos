using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Numerics;
using Zinc.Internal.Sokol;
namespace Zinc.Sandbox.Demos;


[AttributeUsage(AttributeTargets.Class)]
public class DemoSceneAttribute : Attribute
{
    public string Name { get; private set; }
    public DemoSceneAttribute(string name)
    {
        Name = name;
    }
}

public record DemoSceneInfo(Type Type, string Name);
public static class Util
{
    public static IEnumerable<DemoSceneInfo> GetDemoSceneTypes()
    {
        return Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => type.Namespace == "Zinc.Sandbox.Demos" || type.Namespace == "Zinc.Sandbox.Demos.dungeon" )
            .Select(type => new
            {
                Type = type,
                Attribute = type.GetCustomAttribute<DemoSceneAttribute>()
            })
            .Where(t => t.Attribute != null)
            .Select(t => new DemoSceneInfo(t.Type, t.Attribute.Name))
            .OrderBy((info => info.Name));
    }

    public static IEnumerable<DemoSceneInfo> GetGenuarySceneTypes()
    {
        return Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => type.Namespace == "Zinc.Sandbox.Demos.Genuary24")
            .Select(type => new
            {
                Type = type,
                Attribute = type.GetCustomAttribute<DemoSceneAttribute>()
            })
            .Where(t => t.Attribute != null)
            .Select(t => new DemoSceneInfo(t.Type, t.Attribute.Name))
            .OrderBy((info => info.Name));
    }

    public static object CreateInstance(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) != null)
        {
            return Activator.CreateInstance(type);
        }
        else
        {
            throw new InvalidOperationException("No default constructor found for type " + type.FullName);
        }
    }

    // Cached so we don't reflect over the assembly every frame.
    private static List<DemoSceneInfo>? _demosCache;
    public static List<DemoSceneInfo> Demos => _demosCache ??= GetDemoSceneTypes().ToList();
    // Reusable scratch list for the combo widget, repopulated when the demo set changes.
    private static List<string>? _demoNamesCache;
    private static List<string> DemoNames =>
        _demoNamesCache ??= Demos.Select(d => d.Name).ToList();

    public static void SwitchToDemo(DemoSceneInfo info)
    {
        var scene = CreateInstance(info.Type) as Scene;
        scene.Name = info.Name;
        Engine.TargetScene.Unmount(() =>
        {
            scene.Mount(0);
            scene.Load(() => scene.Start());
        });
    }

    // Shared nav strip pinned to the bottom-center of the window. Call once per
    // frame from the global Update callback; it inspects Engine.TargetScene to
    // figure out which demo is active and routes prev/next/jump back through
    // SwitchToDemo. No-op until at least one demo is mounted.
    public static unsafe void DrawDemoNav()
    {
        var demos = Demos;
        if (demos.Count == 0 || Engine.TargetScene == null) return;

        int currentIdx = demos.FindIndex(d => d.Name == Engine.TargetScene.Name);

        var viewport = Internal.Sokol.ImGUI.igGetMainViewport();
        var workPos = viewport->WorkPos;
        var workSize = viewport->WorkSize;
        const float padding = 10f;
        var pos = new ImVec2_t
        {
            x = workPos.x + workSize.x * 0.5f,
            y = workPos.y + workSize.y - padding,
        };
        var pivot = new ImVec2_t { x = 0.5f, y = 1f };
        Internal.Sokol.ImGUI.igSetNextWindowPosEx(pos, (int)Core.ImGUI.Condition.Always, pivot);
        Internal.Sokol.ImGUI.igSetNextWindowBgAlpha(0.65f);

        var flags = Core.ImGUI.WindowFlags.NoDecoration
                  | Core.ImGUI.WindowFlags.AlwaysAutoResize
                  | Core.ImGUI.WindowFlags.NoSavedSettings
                  | Core.ImGUI.WindowFlags.NoFocusOnAppearing
                  | Core.ImGUI.WindowFlags.NoNav
                  | Core.ImGUI.WindowFlags.NoMove;
        Core.ImGUI.Begin("##zinc-demo-nav", flags);

        // We accumulate the navigation intent here and resolve it after End()
        // so the scene unmount/mount doesn't happen mid-window.
        int target = -1;
        var btnSize = new Vector2(70, 22);

        Core.ImGUI.Button("< Prev", btnSize, () =>
        {
            target = currentIdx <= 0 ? demos.Count - 1 : currentIdx - 1;
        });
        Core.ImGUI.SameLine();

        int comboIdx = currentIdx < 0 ? 0 : currentIdx;
        if (Core.ImGUI.Combo("##zinc-demo-pick", DemoNames, ref comboIdx))
        {
            target = comboIdx;
        }
        Core.ImGUI.SameLine();

        Core.ImGUI.Button("Next >", btnSize, () =>
        {
            target = currentIdx < 0 || currentIdx >= demos.Count - 1 ? 0 : currentIdx + 1;
        });

        Core.ImGUI.End();

        if (target >= 0 && target != currentIdx)
        {
            SwitchToDemo(demos[target]);
        }
    }
}
