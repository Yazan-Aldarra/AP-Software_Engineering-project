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
    public void UpdateColliderPos(int x, int y);
    public void SetColliderSize(int width, int height);
    public void AddColliderTriggers(ICollidable collider);
    public List<Collision> CheckForCollisions<T>(ICollidable collider) where T : GameObject, IMovable;
    public void HandleCollisions(ICollidable? decorator, List<Collision> colliders);
    // public void HandleInComingAttacks<T>(T obj, List<Collision> colliders)
    //     where T : ICollidable, IHasHealth;
}
