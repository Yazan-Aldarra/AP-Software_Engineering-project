using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Microsoft.Xna.Framework;

namespace project;

public class ColliderManager
{
    private List<ICollidable> otherColliders = new List<ICollidable>();
    public ColliderManager() { }
    public List<Collision> CheckForCollisions<T>(ICollidable collider) where T : IGameObject, ICollidable, IMovable
    {
        Vector2 Speed;
        if (collider is IMovable)
            Speed = (collider as IMovable).Speed;
        else if (collider is EntityDecorator<T>)
            Speed = (collider as EntityDecorator<T>).ParentSpeed;
        else
            throw new Exception("Not Allowed");

        var tmp = new List<Collision>();
        foreach (var otherCol in otherColliders)
        {
            var hitSide = GetHitSide(collider.PreviousCollider, collider.Collider, otherCol.Collider);
            var overlap = GetOverlapVector(collider.Collider, otherCol.Collider);

            if (hitSide != Direction.NONE && !tmp.Exists(c => c.Collider == otherCol))
            {
                tmp.Add(new Collision(otherCol, hitSide, overlap));
            }
        }
        return tmp;
    }
    public void HandleCollisions<T>(T parent, ICollidable? decorator, List<Collision> collisions)
        where T : IGameObject, ICollidable, IMovable
    {

        var isForDecorator = decorator == null ? false : true;

        if (collisions == null || collisions.Count <= 0)
        {
            if (!isForDecorator)
            {
                parent.IsGrounded = false;
            }
            parent.BlockedSide = new List<Direction>();
            return;
        }

        // reset blocked sides for this collision handling
        parent.BlockedSide = new List<Direction>();

        foreach (var collision in collisions)
        {
            var collider = collision.Collider;
            var hitSide = collision.Direction;

            if (collider is IAttack)
                continue;

            if (hitSide == Direction.DOWN && !isForDecorator)
            {
                parent.IsGrounded = true;
            }
            else
            {
                if (!parent.BlockedSide.Contains(hitSide))
                    parent.BlockedSide.Add(hitSide);
            }
        }
    }
    public void HandleAttackCollisions<T>(T obj, List<Collision> collisions)
        where T : ICollidable, IAttack
    {
        foreach (var col in collisions)
        {
            var collider = col.Collider;
            if (collider is IHasHealth)
            {
                var hasHealth = collider as IHasHealth;
                hasHealth.DecreaseHealth(obj.Damage);
            }
        }
    }
    public void AddCollider(ICollidable collider)
    {
        otherColliders.Add(collider);
    }

    /// <summary>
    /// Returns which side of "moving" hit "solid".
    /// Pass the moving rect's position from the previous frame and the new (current) frame.
    /// </summary>
    public Direction GetHitSide(Rectangle prevMoving, Rectangle moving, Rectangle solid)
    {
        if (!moving.Intersects(solid)) return Direction.NONE; // AABB overlap test [web:2]

        // If we were not intersecting last frame, we can infer the entry side by where we came from.
        if (!prevMoving.Intersects(solid)) // Uses Rectangle.Intersects like MonoGame/XNA AABB checks [web:2]
        {
            if (prevMoving.Right <= solid.Left && moving.Right > solid.Left) return Direction.RIGHT;
            if (prevMoving.Left >= solid.Right && moving.Left < solid.Right) return Direction.LEFT;
            if (prevMoving.Bottom <= solid.Top && moving.Bottom > solid.Top) return Direction.DOWN;
            if (prevMoving.Top >= solid.Bottom && moving.Top < solid.Bottom) return Direction.UP;
        }

        // Fallback (works even if already overlapping): pick the axis of minimum penetration depth. [web:19]
        int overlapLeft = moving.Right - solid.Left;
        int overlapRight = solid.Right - moving.Left;
        int overlapTop = moving.Bottom - solid.Top;
        int overlapBottom = solid.Bottom - moving.Top;

        int minX = Math.Min(overlapLeft, overlapRight);
        int minY = Math.Min(overlapTop, overlapBottom);

        if (minX < minY)
            return (overlapLeft < overlapRight) ? Direction.RIGHT : Direction.LEFT;
        else
            return (overlapTop < overlapBottom) ? Direction.DOWN : Direction.UP;
    }

    public Vector2 GetOverlapVector(Rectangle moving, Rectangle solid)
    {
        // No overlap => no push needed.
        if (!moving.Intersects(solid))
            return Vector2.Zero; // Intersects is an AABB overlap check [web:1]

        // Compute penetration depths on each side (positive values if intersecting).
        int overlapLeft = moving.Right - solid.Left;   // moving penetrated solid from left
        int overlapRight = solid.Right - moving.Left;  // moving penetrated solid from right
        int overlapTop = moving.Bottom - solid.Top;    // moving penetrated solid from top
        int overlapBottom = solid.Bottom - moving.Top;   // moving penetrated solid from bottom

        int minX = Math.Min(overlapLeft, overlapRight);
        int minY = Math.Min(overlapTop, overlapBottom);

        // Choose axis of minimum penetration (the MTV concept: smallest push-out) [web:7]
        if (minX < minY)
        {
            // Push left or right (note sign: this vector should be ADDED to moving's position).
            return (overlapLeft < overlapRight)
                ? new Vector2(-overlapLeft, 0)   // move left
                : new Vector2(overlapRight, 0);  // move right
        }
        else
        {
            // Push up or down.
            return (overlapTop < overlapBottom)
                ? new Vector2(0, -overlapTop)     // move up
                : new Vector2(0, overlapBottom);  // move down
        }
    }
    public List<Collision> GetHighestCollisions(List<Collision> collisions)
    {
        var res = collisions.GroupBy(c => c.Direction).Select(g => g.MaxBy(c => c.OverlapAmount.LengthSquared())).ToList();

        if (res.Count > 4)
            throw new Exception("More then 4 elements not allowed");

        return res;
    }


    /// <summary>
    /// Determines which side of "solid" was hit by "moving".
    /// Uses previous position and velocity to avoid corner ambiguity and tunneling.
    /// </summary>
    public Direction GetHitSide(
        Rectangle prev,
        Vector2 velocity,
        Rectangle moving,
        Rectangle solid)
    {
        // If no collision, early out
        if (!moving.Intersects(solid))
            return Direction.NONE;

        // If we entered this frame, determine side by movement direction
        if (!prev.Intersects(solid))
        {
            // Horizontal movement takes priority if dominant
            if (Math.Abs(velocity.X) > Math.Abs(velocity.Y))
            {
                if (velocity.X > 0)
                    return Direction.RIGHT; // hit solid from left
                else if (velocity.X < 0)
                    return Direction.LEFT;
            }
            else
            {
                if (velocity.Y > 0)
                    return Direction.DOWN; // hit solid from top
                else if (velocity.Y < 0)
                    return Direction.UP;
            }
        }

        // --- Stable fallback: minimum translation vector ---
        float overlapLeft = moving.Right - solid.Left;
        float overlapRight = solid.Right - moving.Left;
        float overlapTop = moving.Bottom - solid.Top;
        float overlapBottom = solid.Bottom - moving.Top;

        float minX = Math.Min(overlapLeft, overlapRight);
        float minY = Math.Min(overlapTop, overlapBottom);

        if (minX < minY)
            return overlapLeft < overlapRight ? Direction.RIGHT : Direction.LEFT;
        else
            return overlapTop < overlapBottom ? Direction.DOWN : Direction.UP;
    }
}
