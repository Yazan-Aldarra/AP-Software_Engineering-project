using System.IO.Pipelines;
using System.Net.Security;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project;

public class Platform : IGameObject, IPlatform, ICollider
{
    public Texture2D Texture2D { get; set; }
    public Vector2 Position { get; set; }
    private Rectangle collider;
    public Rectangle Collider { get => collider; set => collider = value; }
    public float Scale { get; set; }
    public GameObjectState State { get; set; }
    public string Tag { get; set; }

    public Platform(Texture2D texture2D, Rectangle rectangle, Vector2 position)
    {
        this.Texture2D = texture2D;
        Collider = rectangle;
        collider.X = (int)position.X;
        collider.Y = (int)position.Y;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture2D, Collider, Color.Red);
    }

    public void Update(GameTime gameTime)
    {

    }

    public Vector2 GetGameObjectPos()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateColliderPos()
    {
        throw new System.NotImplementedException();
    }
}
