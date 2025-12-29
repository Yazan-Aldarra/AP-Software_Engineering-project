using Microsoft.Xna.Framework;

namespace project;

public class Collision
{
    public ICollidable Collider { get; }
    public Direction Direction { get; }
    public float OverlapAmount { get; }

    public Collision(ICollidable collider, Direction direction, float overlapAmount)
    {
        Collider = collider;
        Direction = direction;
        OverlapAmount = overlapAmount;
    }
}