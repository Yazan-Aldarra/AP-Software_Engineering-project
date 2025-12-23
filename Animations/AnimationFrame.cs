using Microsoft.Xna.Framework;
namespace project;

public class AnimationFrame
{
    public int FrameWidth { get; set; }
    public int FrameHeight { get; set; }
    public Rectangle SourceRectangle { get; set; }
    public AnimationFrame(Rectangle sourceRectangle)
    {
        this.SourceRectangle = sourceRectangle;
        FrameWidth =  SourceRectangle.Width;
        FrameHeight =  SourceRectangle.Height;
    }
    
    public void CropFrame(int horizontalCropping, int verticalCropping)
    {
        SourceRectangle =  new Rectangle(SourceRectangle.X + horizontalCropping/2, SourceRectangle.Y + verticalCropping, 
            SourceRectangle.Width - horizontalCropping,
            SourceRectangle.Height - verticalCropping
        );
    }

}
