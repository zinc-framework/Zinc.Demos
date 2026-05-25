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

// Optional headless-ish automation for the screenshot/diff workflow:
//   ZINC_DEMO=<DemoName>  launch that demo at startup instead of the default
//   ZINC_SHOT=<path.png>  after the scene settles, capture a screenshot to <path> and quit
string? autoDemo = Environment.GetEnvironmentVariable("ZINC_DEMO");
string? autoShot = Environment.GetEnvironmentVariable("ZINC_SHOT");
int autoTick = 0;

List<DemoSceneInfo> demoTypes = new ();
Engine.Run(new Engine.RunOptions(1280,720,"zinc",
	() =>
	{
		demoTypes = Util.GetDemoSceneTypes().ToList();
		Scene? scene = null;
		if (autoDemo != null)
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
		if (autoShot != null)
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
	}
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
