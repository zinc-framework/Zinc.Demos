using Arch.Core.Extensions;
using Zinc.Core;

namespace Zinc.Sandbox.Demos;

[DemoScene("15 Asteroids")]
public class AsteroidsGame : Scene
{
    public class Bullet(SpriteData spriteData, Scene? scene = null, bool startEnabled = true, Action<Sprite, double>? update = null) 
		: Sprite(spriteData, scene, startEnabled, update);
    public class Asteroid(SpriteData spriteData, Scene? scene = null, bool startEnabled = true, Action<Sprite, double>? update = null) 
		: Sprite(spriteData, scene, startEnabled, update);
    public class Player(SpriteData spriteData, Scene? scene = null, bool startEnabled = true, Action<Sprite, double>? update = null) 
		: Sprite(spriteData, scene, startEnabled, update);
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
        player = new Player(fullConscript){Name = "player",X = Engine.Width/2f,Y = Engine.Height/2f};
        InputSystem.Events.Key.Down += OnKeyDown;

		Engine.Cursor.Update = (cursor,dt) => {
			player.X = ((Pointer)cursor).X;
			player.Y = ((Pointer)cursor).Y;
		};
    }

    double bulletCooldown = 0;
    private List<Bullet> bullets = new ();
    private List<Asteroid> asteroids = new ();
    private int bulletCount = 0;
    private void OnKeyDown(Key key, List<Modifiers> arg2)
    {
	    if (key == Key.SPACE)
	    {
		    //spawn bullets
			bullets.Add(new Bullet(fullConscript, update: (self,dt) => {
					((Bullet)self).X += 1.5f;
					if (self.X > Engine.Width)
					{
						bullets.Remove((Bullet)self);
						self.Destroy();
					}
				}){
					Name = "bullet" + bulletCount,
					X = player.X, 
					Y = player.Y,
					Collider_Active = true,
					Collider_OnStart = (self,other) =>  {
						if (other is Asteroid asteroid)
						{
							asteroids.Remove(asteroid);
							asteroid.Destroy();
							bullets.Remove((Bullet)self);
							self.Destroy();
						}
					}
		    });
		    bulletCooldown = 0f;
		    bulletCount++;
	    }
    }

    private double timer = 0;
    public override void Update(double dt)
    {
	    //spawn asteroids
	    timer += Engine.DeltaTime;
	    bulletCooldown += Engine.DeltaTime;
	    if (timer > 5)
	    {
			asteroids.Add(new Asteroid(fullConscript,
			    update: (self,dt) =>
			    {
					if (self.X < -10)
					{
						asteroids.Remove((Asteroid)self);
						self.Destroy();
					}
				    self.X -= 1.5f;
			    }) 
			{
			    Name = "Asteroid",
	    		X = Engine.Width, 
	    		Y = (int)((Engine.Height / 2f) + MathF.Sin(Quick.RandFloat() * 2 - 1) * Engine.Height / 2.5f),
			    Collider_Active = true
	    	});
	    	timer = 0;
	    }
    }

    public override void Cleanup()
    {
	    InputSystem.Events.Key.Down -= OnKeyDown;
		Engine.Cursor.Update = null;
    }
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