using Microsoft.Xna.Framework;
using Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace project;

public abstract class EntityDecorator<TGameObject> : GameObject
    where TGameObject : Entity
{
    protected TGameObject gameObject;

    public Vector2 ParentSpeed { get; set; }

    public EntityDecorator(TGameObject gameObject, Texture2D texture2D)
        : base(texture2D)
    {
        this.gameObject = gameObject;

        Scale = 3f;
        Collider = new Rectangle(0, 0, 0, 0);
        PreviousCollider = Collider;
        Tag = "";
        ParentSpeed = gameObject.Speed;
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        gameObject.Draw(spriteBatch);
    }

    public override void Update(GameTime gameTime)
    {
        gameObject.Update(gameTime);

        PreviousCollider = Collider;
        UpdateColliderPos();
    }
    public override Vector2 GetGameObjectPos() => gameObject.GetGameObjectPos();

    protected new virtual void UpdateColliderPos()
    {
        var c = Collider;
        c.X = gameObject.Collider.X;
        c.Y = gameObject.Collider.Y;
        Collider = c;
    }

    public new void SetColliderSize(int width, int height)
    {
        var c = Collider;
        c.Width = width * (int)Scale;
        c.Height = height * (int)Scale;
        Collider = c;
    }

    public new void AddColliderTriggers(ICollidable collider)
    {
        gameObject.AddColliderTriggers(collider);
    }

    public new void HandleCollisions(ICollidable? decorator, List<Collision> colliders)
    {
        gameObject.HandleCollisions(decorator, colliders);
    }
    public new void HandleAttackCollisions<T>(T obj, List<Collision> colliders)
        where T : ICollidable, IAttack
    {
        gameObject.HandleAttackCollisions(obj, colliders);
    }
    public new List<Collision> CheckForCollisions<T>(ICollidable collider) where T : GameObject, IMovable
    {
        return gameObject.CheckForCollisions<T>(collider);
    }
    public new void SetOverlappedObjectBack(List<Collision> collisions)
    {
        gameObject.SetOverlappedObjectBack(collisions);
    }
    public new void UpdateColliderPos(int x, int y)
    {
        var c = Collider;
        c.X = x;
        c.Y = y;
        Collider = c;
    }
}
