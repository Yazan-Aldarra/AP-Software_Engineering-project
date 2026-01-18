using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using Interfaces;
using Microsoft.Xna.Framework;

namespace project;

public class ColliderManager
{
    private List<ICollidable> otherColliders = new List<ICollidable>();
    public ColliderManager() { }
    public List<Collision> CheckForCollisions<T>(ICollidable collider) where T : IGameObject, IMovable
    {
        var tmp = new List<Collision>();

        foreach (var otherCol in otherColliders)
        {
            // Use the velocity-aware hit test to avoid corner ambiguity and tunneling
            var hitSide = GetHitSide(collider.PreviousCollider, collider.Collider, otherCol.Collider);
            var overlap = GetOverlappedAmount(hitSide, collider.Collider, otherCol.Collider);

            // Ignore tiny overlaps (like 1px) which are usually due to rounding and cause sticky behavior
            if (hitSide == Direction.NONE)
                continue;

            if (!tmp.Exists(c => c.Collider == otherCol))
            {
                tmp.Add(new Collision(otherCol, hitSide, overlap));
            }
        }
        return tmp;
    }
    public void HandleCollisions<T>(T parent, ICollidable? decorator, List<Collision> collisions)
        where T : GameObject, IMovable
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

            if (collider is IAttack && !(collider is IAttacker))
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
    // Handles in coming attacks 
    public void HandleInComingAttack<T>(T obj, List<Collision> collisions)
        where T : ICollidable, IHasHealth
    {
        foreach (var col in collisions)
        {
            if (col.Collider is IAttack attack)
            {
                obj.DecreaseHealth(attack.Damage);
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

        prevMoving = new Rectangle(
            prevMoving.X,
            prevMoving.Y,
            prevMoving.Width,
            prevMoving.Height
        );

        // If we were not intersecting last frame, we can infer the entry side by where we came from.
        if (!prevMoving.Intersects(solid)) 
        {
            // ignore value otherwise object wil jitter
            var ignore = 1;

            if (prevMoving.Bottom - ignore <= solid.Top && moving.Bottom + ignore > solid.Top) return Direction.DOWN;
            if (prevMoving.Right <= solid.Left && moving.Right > solid.Left) return Direction.RIGHT;
            if (prevMoving.Left >= solid.Right && moving.Left < solid.Right) return Direction.LEFT;
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

    public float GetOverlappedAmount(Direction hitSide, Rectangle moving, Rectangle solid)
    {
        float amount = hitSide switch
        {
            Direction.LEFT => solid.Right - moving.Left,   // penetration along -X
            Direction.RIGHT => moving.Right - solid.Left,   // penetration along +X
            Direction.UP => solid.Bottom - moving.Top,   // penetration along -Y
            Direction.DOWN => moving.Bottom - solid.Top,   // penetration along +Y
            Direction.NONE => 0f,
            _ => throw new Exception("Not allowed")
        };

        // Safety: never return negative depth if side inference was wrong.
        return Math.Max(0f, amount);
    }
    public List<Collision> GetHighestCollisions(List<Collision> collisions)
    {
        var res = collisions.GroupBy(c => c.Direction == Direction.DOWN || c.Direction == Direction.UP).Select(g => g.MaxBy(c => c.OverlapAmount)).ToList();

        if (res.Count > 2)
            throw new Exception("More then 2 elements not allowed");

        return res;
    }

}
