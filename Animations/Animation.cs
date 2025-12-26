using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project;

public enum AnimationType { IDLE, RUNNING, ATTACKING, DYING, IN_AIR, TAKING_DAMAGE, CROUCHING, IS_CROUCHED}
public class Animation
{
    public static readonly int FPS = 15;
    public AnimationFrame CurrentFrame { get; set; }
    private List<AnimationFrame> frames;
    private int counter;
    private double secondsCounter = 0;
    public Animation()
    {
        frames = new List<AnimationFrame>();
    }
    public void AddFrame(AnimationFrame frame)
    {
        frames.Add(frame);
        CurrentFrame = frames[0];
    }
    public void Update(GameTime gameTime)
    {
        CurrentFrame = frames[counter];

        secondsCounter += gameTime.ElapsedGameTime.TotalSeconds;
        if (secondsCounter >= 1d / FPS)
        {
            counter++;
            secondsCounter = 0;
        }
        if (counter >= frames.Count)
            counter = 0;
    }
    public void ExtractAnimationFramesRow(Texture2D texture2D, int xDrawingsCount, int yDrawingsCount, int rowNumToExtract)
    {
        int rectangleWidth = texture2D.Width / xDrawingsCount;
        int rectangleHeight = texture2D.Height / yDrawingsCount;
        for (int i = 0; i < xDrawingsCount; i++)
        {
            AddFrame(new AnimationFrame(new Rectangle(rectangleWidth * i, rectangleHeight * (rowNumToExtract - 1), rectangleWidth, rectangleHeight)));
        }
    }
    public void CropAnimationFrames(int verticalCropping, int horizontalCropping)
    {
        foreach (var frame in frames)
            frame.CropFrame(verticalCropping, horizontalCropping);
    }
    public void ResetAnimation()
    {
        counter = 0;
        CurrentFrame =  frames[0];
    }
}
