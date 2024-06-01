using Arch.Core.Extensions;
using Zinc.Core;

namespace Zinc.Sandbox.Demos;

[DemoScene("15 Asteroids")]
public class AsteroidsGame : Scene
{
    private Resources.Texture conscriptImage;
    private SpriteData fullConscript;

    private Sprite player;
    public override void Preload()
    {
        conscriptImage = new Resources.Texture("res/conscript.png");
        fullConscript = new(conscriptImage,(0,0,64,64));
    }

    public override void Create()
    {
        player = new Sprite(fullConscript){Name = "player",X = Engine.Width/2f,Y = Engine.Height/2f,PivotX = 32,PivotY = 32};
        player.ECSEntity.Add(new Player());
        InputSystem.Events.Key.Down += OnKeyDown;
    }

    double bulletCooldown = 0;
    private List<Sprite> bullets = new ();
    private List<Sprite> asteroids = new ();
    private int bulletCount = 0;
    private void OnKeyDown(Key key, List<Modifiers> arg2)
    {
	    // (float dx, float dy) v = key switch {
		   //  Key.LEFT => (-0.2f, 0),
		   //  Key.RIGHT => (0.2f, 0),
		   //  Key.UP => (0, -0.2f),
		   //  Key.DOWN => (0, 0.2f),
		   //  _ => (0, 0)
	    // };
	    // player.SetVelocity(player.DX + v.dx, player.DY + v.dy);

	    if (key == Key.SPACE)
	    {
		    //spawn bullets
		    var bullet = new Sprite(fullConscript,
			    collisionStart: (self,other) =>
			    {
				    if (other.Entity.Has<Asteroid>())
				    {
					    asteroids.RemoveAt(asteroids.FindIndex(ast => ast.ECSEntity.Id == other.Entity.Id));
					    other.Destroy();
					    bullets.RemoveAt(bullets.FindIndex(bullet => bullet.ECSEntity.Id == self.Entity.Id));
					    self.Destroy();
				    }
			    },
			    update: (self,dt) =>
			    {
				    self.X += 1.5f;
			    })
		    {
			    Name = "bullet" + bulletCount,
			    X = player.X, 
			    Y = player.Y,
			    ColliderActive = true,
		    };
		    bullet.ECSEntity.Add(new Bullet());
		    bullets.Add(bullet);
		    bulletCooldown = 0f;
		    bulletCount++;
	    }
    }

    private double timer = 0;
    public override void Update(double dt)
    {
	    Quick.MoveToMouse(player);
	    //spawn asteroids
	    timer += Engine.DeltaTime;
	    bulletCooldown += Engine.DeltaTime;
	    if (timer > 5)
	    {
	    	var a = new Sprite(fullConscript,
			    update: (self,dt) =>
			    {
				    Console.WriteLine("asteroid update");
				    self.X -= 1.5f;
			    }) {
			    Name = "Asteroid",
	    		X = Engine.Width, 
	    		Y = (int)((Engine.Height / 2f) + MathF.Sin(Quick.RandFloat() * 2 - 1) * Engine.Height / 2.5f),
			    ColliderActive = true
	    	};
		    a.ECSEntity.Add(new Asteroid());
		    asteroids.Add(a);
	    	timer = 0;
	    }

	    bullets.RemoveAll(b =>
	    {
		    if (b.X > Engine.Width)
		    {
			    Console.WriteLine("well destory " + b.Name);
			    b.Destroy();
			    return true;
		    }
		    return false;
	    });
	    
	    asteroids.RemoveAll(a =>
	    {
		    if (a.X < -10)
		    {
			    a.Destroy();
			    return true;
		    }
		    return false;
	    });
    }

    public override void Cleanup()
    {
	    InputSystem.Events.Key.Down -= OnKeyDown;
    }
    public readonly record struct Bullet();
    public readonly record struct Asteroid();
    public readonly record struct Player();
}

/*
add(sprite with tex = res.ship)
add(new sprite(res.ship))



```c#

Engine.Update += (() => {

foreach (var bullet in bullets) {

var status = bullet.Get<Collider>().Status;

if(status.colliding && status.object is not Ship) {

Destroy(bullet,status.object);

}

if(bullet.X > Screen.Width) {

Destroy(bullet);

}

}

  

foreach (var asteroid in asteroids) {

if(asteroid.X < 0) {

Destroy(asteroid);

}

}

  

if(asteroids.Count <= 5) {

for (int i = 0; i < 10-asteroids.Count; i++) {

asteroids.Add(new Asteroid(

Screen.Width / 2 + Random(0,Screen.Width),

Random(0,Screen.Height),

true));

}

}

}

kdl also maybe
objects can be constructed manually, or you can get something like prefabs from script defines like this

bullet.eno
# Bullet << Object2D
X: 10
Y: 50
## Components:
- Sprite
- Collider
- Mover
### Sprite:
img = bullet.png
### Mover:
X = 1;


asteroid.eno
# Bullet << Object2D
X: 10
Y: 50
## Components:
- Sprite
- Collider
- Mover
### Sprite:
img = bullet.png
### Mover:
X = 1;


ship.eno
# Ship << Object2D
X: 50
Y: Screen.Height / 2
## Components:
- Sprite
- Collider
- Mover
### Sprite:
img = ship.png
*/