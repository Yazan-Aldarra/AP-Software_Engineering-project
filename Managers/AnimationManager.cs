using Microsoft.Xna.Framework;
namespace project;

public class AnimationManager
{
    private IAnimatable animatable;

    public AnimationManager(IAnimatable animatable)
    {
       this.animatable = animatable;
    }
    public void HandleAnimation() { }
    public void HandleAnimation(Vector2 direction)
    {
        var x = direction.X;
        var y = direction.Y;

        var res = (x,y) switch {
            (_, < 0) => AnimationType.RUNNING,
            (> 0, 0) => AnimationType.RUNNING,
            (< 0, 0) => AnimationType.RUNNING,
            _ => AnimationType.STANDING
        };

        SetCurrentAnimation(res);
    }
    public void SetCurrentAnimation(AnimationType animationType)
    {
        animatable.CurrentAnimation = animationType;
    }

}
