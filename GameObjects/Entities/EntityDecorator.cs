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

    protected override void UpdateColliderPos()
    {
        var c = Collider;
        c.X = gameObject.Collider.X;
        c.Y = gameObject.Collider.Y;
        Collider = c;
    }

    public override void SetColliderSize(int width, int height)
    {
        var c = Collider;
        c.Width = width * (int)Scale;
        c.Height = height * (int)Scale;
        Collider = c;
    }

    public override void AddColliderTriggers(ICollidable collider)
    {
        gameObject.AddColliderTriggers(collider);
    }

    public override void HandleCollisions(ICollidable? decorator, List<Collision> colliders)
    {
        gameObject.HandleCollisions(decorator, colliders);
    }
    public override List<Collision> CheckForCollisions<T>(ICollidable collider) 
    {
        return gameObject.CheckForCollisions<T>(collider);
    }
    public void SetOverlappedObjectBack(List<Collision> collisions)
    {
        gameObject.SetOverlappedObjectBack(collisions);
    }
    public override void UpdateColliderPos(int x, int y)
    {
        var c = Collider;
        c.X = x;
        c.Y = y;
        Collider = c;
    }
}
