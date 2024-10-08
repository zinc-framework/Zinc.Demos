namespace Zinc.Sandbox.Demos;
using static Zinc.Quick;

[DemoScene("05 Interaction")]
public class Interaction : Scene
{
    private Resources.Texture conscriptImage;
    private AnimatedSpriteData animatedConscript;
    public override void Preload()
    {
        conscriptImage = new Resources.Texture("res/conscript.png");
        var rects = Quick.CreateTextureSlices(512, 512, 64, 64);
        animatedConscript = new AnimatedSpriteData(
            conscriptImage,
            new() { new("test", rects[..3],
                0.4f) });
    }

    private float DX;
    private float DY;
    private AnimatedSprite e;
    public override void Create()
    {
        e = new AnimatedSprite(animatedConscript,update: (self,dt) =>
        {
	        self.X += DX;
	        self.Y += DY;
        });
        Console.WriteLine("subbing");
        InputSystem.Events.Key.Down += KeyDownListener;
    }

    void KeyDownListener(Key key, List<Modifiers> mods)
    {
        (float dx, float dy) v = key switch {
            Key.LEFT => (-0.3f, 0),
            Key.RIGHT => (0.3f, 0),
            Key.UP => (0, -0.3f),
            Key.DOWN => (0, 0.3f),
            _ => (0, 0)
        }; 
        DX += v.dx;
        DY += v.dy;
    }

    public override void Cleanup()
    {
	    Console.WriteLine("unsubbing");
	    InputSystem.Events.Key.Down -= KeyDownListener;
    }
}

/*
 
saving old style here as a ref - its quicker to write!


void interaction()
   {
   	AnimatedSpriteData animatedConscript = new AnimatedSpriteData(
   		conscriptImage,
   		new() { new("test", HorizontalFrameSequence(0, 0, 64, 64, 4),
   			0.4f) });
   	var e = new AnimatedSprite(animatedConscript);
   	
   	OnKeyDown += (key,_) =>  {
   		(float dx, float dy) v = key switch {
   			Key.LEFT => (-1f, 0),
   			Key.RIGHT => (1f, 0),
   			Key.UP => (0, -1f),
   			Key.DOWN => (0, 1f),
   			_ => (0, 0)
   		};
   		e.DX += v.dx;
   		e.DY += v.dy;
   	};
   }
   
   Engine.Run() 
*/