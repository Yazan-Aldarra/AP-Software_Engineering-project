using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project;

public abstract class Block : GameObject
{
    public Block(
            Texture2D texture2D,
            Vector2? initialPos = null,
            int width = 10,
            int height = 10,
            float scale = 1f,
            Animation animation = null,
            int xDrawingsCount = 1,
            int yDrawingsCount = 1,
            Texture2D colliderTexture2d = null
        ) : base(
            texture2D,
            initialPos,
            width,
            height,
            scale,
            animation,
            xDrawingsCount,
            yDrawingsCount,
            colliderTexture2d
        )
    {

    }
}
