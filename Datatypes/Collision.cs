using Microsoft.Xna.Framework;

namespace project;

public class Collision
{
    public ICollidable Collider { get; }
    public Direction Direction { get; }
    public Vector2 OverlapAmount { get; }

    public Collision(ICollidable collider, Direction direction, Vector2 overlapAmount)
    {
        Collider = collider;
        Direction = direction;
        OverlapAmount = overlapAmount;
    }
}