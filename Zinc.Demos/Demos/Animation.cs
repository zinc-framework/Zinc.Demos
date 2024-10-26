using System.Collections;

namespace Zinc.Sandbox.Demos;

[DemoScene("03 Animation")]
public class Animation : Scene
{
    private AnimatedSpriteData animationData;
    AnimatedSprite animatedSprite;
    public override void Preload()
    {
        var rects = Quick.CreateTextureSlices(512, 512, 64, 64);
        animationData = new AnimatedSpriteData(
            Res.Assets.conscript,
            [
                new("north", rects[..3],0.4f),
                new("south", rects[4..7],0.4f),
                new("east", rects[8..11],0.4f),
                new("west", rects[12..15],0.4f),
            ]);
    }

    public override void Create()
    {
        animatedSprite = new AnimatedSprite(animationData);
        new Coroutine(changeAnimationDirection(),"animator");
    }

    IEnumerator changeAnimationDirection()
    {
        while (true)
        {
            animatedSprite.SetAnimation("north");
            yield return new WaitForSeconds(0.8f);
            animatedSprite.SetAnimation("east");
            yield return new WaitForSeconds(0.8f);
            animatedSprite.SetAnimation("south");
            yield return new WaitForSeconds(0.8f);
            animatedSprite.SetAnimation("west");
            yield return new WaitForSeconds(0.8f);
        }
    }
}