using Microsoft.Xna.Framework;
using Interfaces;
using Microsoft.Xna.Framework.Graphics;

namespace project;

public abstract class GameObjectDecorator : IGameObject
{
    protected IGameObject gameObject;

    public Texture2D Texture2D { get; set; }
    public float Scale { get; set; }
    public GameObjectState State { get; set; }
    protected Rectangle collider;
    public Rectangle Collider { get => collider; set => collider = value; }
    public ColliderManager ColliderManager { get; set; }
    public string Tag { get; set; }

    public GameObjectDecorator(IGameObject gameObject)
    {
        this.gameObject = gameObject;
    }
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        gameObject.Draw(spriteBatch);
    }

    public virtual void Update(GameTime gameTime)
    {
        gameObject.Update(gameTime);
    }
    public Vector2 GetGameObjectPos()
    {
        return gameObject.GetGameObjectPos();
    }

    public abstract void UpdateColliderPos();
}
