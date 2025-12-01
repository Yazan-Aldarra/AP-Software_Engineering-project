using System;
using Microsoft.Xna.Framework;

namespace project;

public class AnimationFrame
{
    public Rectangle SourceRectangle { get; set; }
    public AnimationFrame(Rectangle sourceRectangle)
    {
        this.SourceRectangle = sourceRectangle;
    }
    

}
