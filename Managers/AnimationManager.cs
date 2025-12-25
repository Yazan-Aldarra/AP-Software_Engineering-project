using Microsoft.Xna.Framework;
namespace project;

public class AnimationManager
{
    private IAnimatable animatable;
    public AnimationManager(IAnimatable animatable)
    {
        this.animatable = animatable;
    }

    // public void HandleAnimation(bool IsGrounded)
    // {
    //     if (!IsGrounded) { SetCurrentAnimation(AnimationType.FALLING); }
    // }
    public void HandleAnimation(Vector2 direction)
    {

        var dir = Utils.GetDirection(direction);
        var res = dir switch
        {
            Direction.NONE => AnimationType.STANDING,

            Direction.LEFT or Direction.RIGHT or
            Direction.UP or Direction.DOWN or
            Direction.LEFT_TOP or Direction.RIGHT_TOP or
            Direction.LEFT_DOWN or Direction.RIGHT_DOWN
                => AnimationType.RUNNING,

            _ => AnimationType.STANDING
        };

        SetCurrentAnimation(res);
    }
    public void SetCurrentAnimation(AnimationType animationType)
    {
        // animatable.CurrentAnimation = animationType;
    }
}
