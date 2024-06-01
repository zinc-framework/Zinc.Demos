using System.Runtime.InteropServices;
using Dinghy;
using Zinc.Core.ImGUI;
using Zinc.Sandbox;
using Zinc.Sandbox.Demos;
using Zinc.Sandbox.Demos.dungeon;
using static Zinc.Quick;
using Collision = Zinc.Sandbox.Demos.Collision;

NativeLibResolver.kick();

InputSystem.Events.Key.Down += (key,_) =>  {
	if (key == Key.C)
	{
		Engine.Clear = !Engine.Clear;
	}
};

List<DemoSceneInfo> demoTypes = new();  
Engine.Run(new Engine.RunOptions(1920,1080,"zinc", 
	() =>
	{
		demoTypes = Util.GetDemoSceneTypes().ToList();
		var scene = new ShapeDemo();
		scene.Mount(0);
		scene.Load(() => scene.Start());
	}, 
	() =>
	{
		drawDemoOptions();
	}
	));

void drawDemoOptions()
{
	ImGUIHelper.Wrappers.MainMenu(() =>
	{
		ImGUIHelper.Wrappers.Menu("Zinc", () =>
		{
			ImGUIHelper.Wrappers.Menu("Demos", () =>
			{
				Scene? scene = null;
				ImGUIHelper.Wrappers.Menu("Examples", () =>
				{
					foreach (var type in demoTypes)
					{
						if (ImGUIHelper.Wrappers.MenuItem(type.Name))
						{
							scene = Util.CreateInstance(type.Type) as Scene;
							scene.Name = type.Name;
						}
					}
				});

				if (scene != null)
				{
					Engine.TargetScene.Unmount(() =>
					{
						scene.Mount(0);
						scene.Load(() => scene.Start());
					});
				}
			});
		});
	});
}