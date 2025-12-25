using System;
using System.Collections.Generic;

namespace project;

public interface IAnimatable
{
    
    public void AddAnimation(AnimationType animationType, int spriteRowNum);
    public void CropAnimationFrames(int verticalCropping, int horizontalCropping, List<Animation> animationsNotToCrop = null);
}
