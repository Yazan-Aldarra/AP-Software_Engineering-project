

using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project;

public class Animation
{
   public AnimationFrame CurrentFrame {get; set;} 
   private List<AnimationFrame> frames;
   private int counter;
    public Animation()
    {
        frames = new List<AnimationFrame>();
    }
    public void AddFrame(AnimationFrame frame)
    {
        frames.Add(frame);
        CurrentFrame = frames[0];
    }
    public void Update()
    {
        CurrentFrame = frames[counter];
        counter++;
        if (counter >= frames.Count)
            counter =0;
    }
    public void ExtractAnimationFramesRow(Texture2D texture2D, int xDrawingsCount, int yDrawingsCount ,int rowNumToExtract)
    {
        int rectangleWidth = texture2D.Width/xDrawingsCount;
        int rectangleHeight = texture2D.Height/yDrawingsCount;
        for (int i = 0; i< xDrawingsCount; i++)
        {
            AddFrame(new AnimationFrame(new Rectangle(rectangleWidth*i,rectangleHeight*(rowNumToExtract -1) ,rectangleWidth, rectangleHeight)));
        }
    }
}
