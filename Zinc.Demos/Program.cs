using Zinc;
using Zinc.Core;
using Zinc.Sandbox.Demos;
using System.Numerics;

InputSystem.Events.Key.Down += (key,_) =>  {
	if (key == Key.C)
	{
		Engine.Clear = !Engine.Clear;
	}
	if (key == Key.COMMA)
	{
		Engine.ShowMenu = !Engine.ShowMenu;
	}
	if (key == Key.F2)
	{
		Engine.Screenshot(); // timestamped PNG next to Engine.ScreenshotPath
	}
};

// Command-line options. When launching through `dotnet run`, put these after `--`:
//
//   dotnet run --project .\Zinc.Demos\Zinc.Demos.csproj -- --demo "08 Shape" --companion
//
//   --demo <name>         launch that demo at startup instead of the default
//   --shot <path.png>     once the scene settles, capture a screenshot there and quit
//   --transparent         composited, see-through window: whatever is behind it shows
//                         through wherever the demo draws nothing
//   --companion           the full desktop-companion shape: transparent + frameless +
//                         always on top + off the taskbar, draggable by its content
//   --dockspace           submit a full-viewport ImGui dock space
//   --clickthrough-test   alternate click-through on/off every 4s, logging each flip.
//                         Timed rather than key-bound so window focus can't confound the
//                         test: clicking through onto another app hands it focus, and a
//                         key binding would stop arriving exactly when it's needed.
//   --help                print this list and exit

var argList = args.ToList();

bool Flag(string name)
{
    int i = argList.FindIndex(a => string.Equals(a, name, StringComparison.OrdinalIgnoreCase));
    if (i < 0) return false;
    argList.RemoveAt(i);
    return true;
}

string? Option(string name)
{
    int i = argList.FindIndex(a => string.Equals(a, name, StringComparison.OrdinalIgnoreCase));
    if (i < 0) return null;
    if (i + 1 >= argList.Count)
    {
        Console.WriteLine($"[args] {name} needs a value");
        argList.RemoveAt(i);
        return null;
    }
    string value = argList[i + 1];
    argList.RemoveRange(i, 2);
    return value;
}

if (Flag("--help") || Flag("-h"))
{
    Console.WriteLine("""
        Zinc.Demos options (pass after `--` when using dotnet run)

          --demo <name>         launch that demo at startup instead of the default
          --shot <path.png>     capture a screenshot once the scene settles, then quit
          --transparent         composited, see-through window
          --companion           transparent + frameless + always on top + off-taskbar
          --dockspace           submit a full-viewport ImGui dock space
          --clickthrough-test   alternate click-through every 4s, logging each flip
          --help                print this list and exit
        """);
    return;
}

string? autoDemo = Option("--demo");
string? autoShot = Option("--shot");
bool transparent = Flag("--transparent");
bool companion = Flag("--companion");
bool dockSpace = Flag("--dockspace");
bool clickThroughTest = Flag("--clickthrough-test");

// --companion is the full preset; --transparent is just the see-through background
var windowOptions = companion
	? Engine.WindowOptions.Companion
	: Engine.WindowOptions.Default with { Transparent = transparent };

foreach (var leftover in argList)
{
    Console.WriteLine($"[args] ignoring unrecognised option '{leftover}' (try --help)");
}

double clickThroughClock = 0;
bool clickThroughState = false;
int autoTick = 0;

List<DemoSceneInfo> demoTypes = new ();
Engine.Run(new Engine.RunOptions(1280,720,"zinc",
	() =>
	{
		if (companion)
		{
			// The engine's menu bar doubles as the title bar when borderless (drag it to move,
			// X on the right to quit). That only exists while the menu is shown, so when it's
			// hidden with ',' fall back to dragging from anywhere.
			InputSystem.Events.Mouse.Down += (_) => { if (!Engine.ShowMenu) DesktopWindow.BeginDrag(); };
		}
		demoTypes = Util.GetDemoSceneTypes().ToList();
		Scene? scene = null;
		if (!string.IsNullOrEmpty(autoDemo))
		{
			var info = demoTypes.FirstOrDefault(d => d.Name == autoDemo);
			if (info != null) { scene = Util.CreateInstance(info.Type) as Scene; scene!.Name = info.Name; }
			else Console.WriteLine($"[auto] demo '{autoDemo}' not found");
		}
		scene ??= new SGP_Zinc_Rectangle();
		scene.Mount(0);
		scene.Load(() => scene.Start());
	},
	() =>
	{
		if (clickThroughTest)
		{
			clickThroughClock += Engine.DeltaTime;
			if (clickThroughClock >= 4.0)
			{
				clickThroughClock = 0;
				clickThroughState = !clickThroughState;
				Engine.ClickThrough = clickThroughState;
				Console.WriteLine(clickThroughState
					? "[clickthrough] ON  - clicks should reach whatever is behind the window"
					: "[clickthrough] OFF - the window should swallow clicks again");
			}
		}

		if (!string.IsNullOrEmpty(autoShot))
		{
			autoTick++;
			if (autoTick == 20) Engine.ShowMenu = false;
			if (autoTick == 25) Engine.Screenshot(autoShot);
			if (autoTick == 40) Zinc.Internal.Sokol.App.request_quit();
		}
		if(Engine.ShowMenu)
		{
			drawDemoOptions();
			Util.DrawDemoNav();
		}
	},
	imguiDockSpace: dockSpace,
	window: windowOptions
	));

void drawDemoOptions()
{
	ImGUI.MainMenu(() =>
	{
		ImGUI.Menu("Demos", () =>
		{
			Scene? scene = null;
			foreach (var type in demoTypes)
			{
				if (ImGUI.MenuItem(type.Name))
				{
					scene = Util.CreateInstance(type.Type) as Scene;
					// scene = Activator.CreateInstance(type.Type) as Scene;
					scene.Name = type.Name;
				}
			}
			if (scene != null)
			{
				Engine.TargetScene.Unmount(() =>
				{
					scene.Mount(0);
					scene.Load(() => scene.Start());
				});
			}
		});
		ImGUI.Button("Reload Scene",new Vector2(100,20),() => {
			var targetSceneType = Engine.TargetScene.GetType();
			Engine.TargetScene.Unmount(() => {
				var reloadedScene = Util.CreateInstance(targetSceneType) as Scene;
				reloadedScene.Mount(0);
				reloadedScene.Load(() => reloadedScene.Start());
			});
		});
	});
}
