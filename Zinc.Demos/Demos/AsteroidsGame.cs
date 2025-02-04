using Arch.Core.Extensions;
using Zinc.Core;

namespace Zinc.Sandbox.Demos;

[DemoScene("15 Asteroids")]
public class AsteroidsGame : Scene
{
	Tag Tasteroid = "asteroid";
	Tag Tbullet = "bullet";
	Tag Tplayer = "player";
    private SpriteData fullConscript;
    private Sprite player;
    public override void Preload()
    {
		fullConscript = Res.Assets.conscript.Texture.Slice(new Rect(0,0,64,64));
    }

    public override void Create()
    {
        player = new Sprite(fullConscript){Name = "player",Tags = {Tplayer}};
        InputSystem.Events.Key.Down += OnKeyDown;

		Quick.Loop(5, () => {
			//spawn asteroids
			asteroids.Add(new Sprite(fullConscript,
			    update: (self,dt) =>
			    {
					if (self.X < -10)
					{
						asteroids.Remove(self);
						self.Destroy();
					}
				    self.X -= 1.5f;
			    }) 
			{
			    Name = "Asteroid",
				Tags = {Tasteroid},
	    		X = Engine.Width, 
	    		Y = (int)((Engine.Height / 2f) + MathF.Sin(Quick.RandFloat() * 2 - 1) * Engine.Height / 2.5f),
			    Collider_Active = true
	    	});
		});

    }

    private List<Entity> bullets = new ();
    private List<Entity> asteroids = new ();
    private int bulletCount = 0;
    private void OnKeyDown(Key key, List<Modifiers> arg2)
    {
	    if (key == Key.SPACE)
	    {
		    //spawn bullets
			bullets.Add(new Sprite(fullConscript, update: (self,dt) => {
					self.X += 1.5f;
					if (self.X > Engine.Width)
					{
						bullets.Remove(self);
						self.Destroy();
					}
				}){
					Name = "bullet" + bulletCount,
					X = player.X, 
					Y = player.Y,
					Tags = {Tbullet},
					Collider_Active = true,
					Collider_OnStart = (self,other) =>  {
						if (other.Tagged(Tasteroid))
						{
							asteroids.Remove(other);
							other.Destroy();
							bullets.Remove(self);
							self.Destroy();
						}
					}
		    });
		    bulletCount++;
	    }
    }

	public override void Update(double dt)
	{
		Quick.MoveToMouse(player);
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