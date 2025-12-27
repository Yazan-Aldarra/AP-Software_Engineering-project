using System.Collections.Generic;
using Interfaces;
using Microsoft.Xna.Framework;

namespace project;

public interface ICollidable
{
    // public ColliderManager ColliderManager { get; set; }
    public Rectangle Collider { get; set; }
    public Rectangle PreviousCollider { get; set; }
    public string Tag { get; set; }
    public void UpdateColliderPos();
    public void SetColliderSize(int width, int height);
    public void AddColliderTriggers(ICollidable collider);
    public List<Collision> CheckForCollisions<T>(ICollidable collider) where T : IGameObject, ICollidable, IMovable;
    public void HandleCollisions(ICollidable? decorator, List<Collision> colliders);
    public void HandleAttackCollisions<T>(T obj, List<Collision> colliders)
        where T : ICollidable, IAttack;
}
