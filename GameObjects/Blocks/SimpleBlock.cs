using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project;

public class SimpleBlock : Block
{
    public SimpleBlock(Texture2D texture2D, int xDrawingsCount = 1, int yDrawingsCount = 1, Texture2D colliderTexture2d = null) 
        : base(texture2D, xDrawingsCount, yDrawingsCount, colliderTexture2d)
    {
    }

    public SimpleBlock(Texture2D texture2D, Rectangle sizeRect, Vector2 position, Texture2D colliderTexture2d = null)
        : base(texture2D, 1, 1, colliderTexture2d)
    {
        // position and collider are set explicitly for static blocks
        this.position = position;
        this.Collider = new Rectangle((int)position.X, (int)position.Y, sizeRect.Width, sizeRect.Height);
    }
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}
