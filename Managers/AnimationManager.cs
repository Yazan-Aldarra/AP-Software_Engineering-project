using System.Collections.Generic;
namespace project;

public class AnimationManager
{
    public void HandleResettingAnimation(IAnimatable  animatable, Dictionary<AnimationType, Animation> animations)
    {
        if(animatable.PreviousState == null)
            return;

        if (animatable.PreviousState != animatable.State)
            animations[animatable.PreviousState.AnimationType].ResetAnimation();
    }
}
