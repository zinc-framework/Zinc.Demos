using Zinc;
using Zinc.Core;
using Zinc.Sandbox.Demos;
using System.Numerics;

InputSystem.Events.Key.Down += (key,_) =>  {
	if (key == Key.C)
	{
		Engine.Clear = !Engine.Clear;
	}
};

// DemoSceneInfo[] demoTypes = new DemoSceneInfo[]
// {
// 	new DemoSceneInfo(typeof(Zinc.Sandbox.Demos.Animation),"Animation"),
// 	new DemoSceneInfo(typeof(Zinc.Sandbox.Demos.AsteroidsGame),"AsteroidsGame"),
// 	new DemoSceneInfo(typeof(Zinc.Sandbox.Demos.BunnyMark),"BunnyMark"),
// 	new DemoSceneInfo(typeof(Zinc.Sandbox.Demos.Cards),"Cards"),
// 	new DemoSceneInfo(typeof(Zinc.Sandbox.Demos.ChildrenDemo),"ChildrenDemo"),
// 	new DemoSceneInfo(typeof(Zinc.Sandbox.Demos.Collision),"Collision"),
// 	new DemoSceneInfo(typeof(Zinc.Sandbox.Demos.Coroutines),"Coroutines"),
// 	new DemoSceneInfo(typeof(Zinc.Sandbox.Demos.EntityEmitter),"EntityEmitter"),
// 	new DemoSceneInfo(typeof(Zinc.Sandbox.Demos.FontstashDemo),"FontstashDemo"),
// 	new DemoSceneInfo(typeof(Zinc.Sandbox.Demos.GridDemo),"GridDemo"),
// 	new DemoSceneInfo(typeof(Zinc.Sandbox.Demos.Interaction),"Interaction"),
// 	new DemoSceneInfo(typeof(Zinc.Sandbox.Demos.ParticleSystem),"ParticleSystem"),
// 	new DemoSceneInfo(typeof(Zinc.Sandbox.Demos.Physics),"Physics"),
// 	new DemoSceneInfo(typeof(Zinc.Sandbox.Demos.PhysicsShape),"PhysicsShape"),
// 	new DemoSceneInfo(typeof(Zinc.Sandbox.Demos.ShapeDemo),"ShapeDemo"),
// 	new DemoSceneInfo(typeof(Zinc.Sandbox.Demos.SimpleUpdate),"SimpleUpdate"),
// 	new DemoSceneInfo(typeof(Zinc.Sandbox.Demos.TextDemo),"TextDemo"),
// 	new DemoSceneInfo(typeof(Zinc.Sandbox.Demos.Texture),"Texture"),
// 	new DemoSceneInfo(typeof(Zinc.Sandbox.Demos.TextureFrame),"TextureFrame"),
// };

List<DemoSceneInfo> demoTypes = new ();
Engine.Run(new Engine.RunOptions(1920,1080,"zinc", 
	() =>
	{
		demoTypes = Util.GetDemoSceneTypes().ToList();
		var scene = new ShapeDemo();
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
