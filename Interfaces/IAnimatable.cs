using System;
using System.Collections.Generic;

namespace project;

public interface IAnimatable
{
    public AnimationType CurrentAnimation { get; set; }
    public AnimationManager AnimationManager { get; set; }
    public void AddAnimation(AnimationType animationType, int spriteRowNum);
    public void CropAnimationFrames(int verticalCropping, int horizontalCropping, List<Animation> animationsNotToCrop = null);
}
