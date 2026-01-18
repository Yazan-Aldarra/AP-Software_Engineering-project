using System;
using System.Collections.Generic;

namespace project;

public interface IAnimatable
{
    public GameObjectState PreviousState { get; set; } 
    public GameObjectState State { get; set; } 
    public void AddAnimation(AnimationType animationType, int spriteRowNum, float delayEachFrame);
    public void CropAnimationFrames(int verticalCropping, int horizontalCropping, List<Animation> animationsNotToCrop = null);
}
