using Zinc;
using Zinc.Core;
using Zinc.Sandbox.Demos;
using Collision = Zinc.Sandbox.Demos.Collision;

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
		var scene = new GridDemo();
		Console.WriteLine("mounting scene now");
		scene.Mount(0);
		Console.WriteLine("scene mounted");
		scene.Load(() => scene.Start());
	}, 
	() =>
	{
		drawDemoOptions();
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
	});
}