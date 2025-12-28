using Microsoft.Xna.Framework;
using Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace project;

public abstract class EntityDecorator<TGameObject> : IGameObject, ICollidable
    where TGameObject: IGameObject, ICollidable, IMovable
{
    protected TGameObject gameObject;

    // IGameObject
    public GameObjectState State { get; set; }

    public Texture2D Texture2D { get; set; }
    public float Scale { get; set; }

    // ICollidable
    protected Rectangle collider;
    public Rectangle PreviousCollider { get; set; }
    public Rectangle Collider { get => collider; set => collider = value; }
    public string Tag { get; set; }
    public Vector2 ParentSpeed { get; set; }

    public EntityDecorator(TGameObject gameObject, Texture2D texture2D)
    {
        this.gameObject = gameObject;

        Scale = 3f;
        collider = new Rectangle(0, 0, 0, 0);
        PreviousCollider = collider;
        Tag = "";
        Texture2D = texture2D;
        ParentSpeed = gameObject.Speed;
    }
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        gameObject.Draw(spriteBatch);
    }

    public virtual void Update(GameTime gameTime)
    {
        gameObject.Update(gameTime);

        PreviousCollider = collider;
        UpdateColliderPos();
    }
    public Vector2 GetGameObjectPos()
    {
        return gameObject.GetGameObjectPos();
    }

    public virtual void UpdateColliderPos()
    {
        collider.X = gameObject.Collider.X;
        collider.Y = gameObject.Collider.Y;
    }

    public void SetColliderSize(int width, int height)
    {
        collider.Width = width * (int)Scale;
        collider.Height = height * (int)Scale;
    }

    public void AddColliderTriggers(ICollidable collider)
    {
        gameObject.AddColliderTriggers(collider);
    }

    public void HandleCollisions(ICollidable? decorator, List<Collision> colliders)
    {
        gameObject.HandleCollisions(decorator, colliders);
    }
    public void HandleAttackCollisions<T>(T obj, List<Collision> colliders)
        where T : ICollidable, IAttack
    {
        gameObject.HandleAttackCollisions(obj, colliders);
    }
    public List<Collision> CheckForCollisions<T>(ICollidable collider) where T : IGameObject, ICollidable, IMovable
    {
        return gameObject.CheckForCollisions<T>(collider);
    }
    public void SetOverlappedObjectBack(List<Collision> collisions)
    {
        gameObject.SetOverlappedObjectBack(collisions);
    }
}
