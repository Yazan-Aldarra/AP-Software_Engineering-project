using System.IO.Pipelines;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project;
public class Platform : IGameObject, IPlatform, ICollider{
    private Texture2D texture2D;
    public Vector2 Position { get; set; }
    private Rectangle colliderRec;
    public Rectangle ColliderRec { get => colliderRec; set => colliderRec = value; }
    public string Tag { get; set; }
    
    public Platform(Texture2D texture2D, Rectangle rectangle, Vector2 position)
    {
        this.texture2D =  texture2D;
        ColliderRec = rectangle;
        colliderRec.X = (int) position.X;
        colliderRec.Y = (int) position.Y;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture2D, ColliderRec, Color.Red);
    }

    public void Update(GameTime gameTime)
    {
        
    }
}
