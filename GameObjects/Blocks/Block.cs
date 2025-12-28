using System.Collections.Generic;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project;

public abstract class Block : GameObject
{
    public Block(Texture2D texture2D, int xDrawingsCount = 1, int yDrawingsCount = 1, Texture2D colliderTexture2d = null) 
        : base(texture2D, xDrawingsCount, yDrawingsCount, colliderTexture2d)
    { }
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}
