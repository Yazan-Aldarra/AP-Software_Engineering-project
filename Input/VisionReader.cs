using System;
using System.Buffers;
using Microsoft.Xna.Framework;

namespace project;

public class VisionReader<TEntity> : IInputReader where TEntity : Entity, IHasHealth
    // public class VisionReader : IInputReader
{
    public bool IsDestinationInput => true;
    public TEntity Entity { get; set; }
    public Entity Owner { get; set; }

    public float AttackRange { get; set; } = 60f;
    public float FollowRange { get; set; } = 200f;
    public VisionReader(TEntity entity, Entity owner)
    {
        Entity = entity;
        Owner = owner;
    }
    public VisionReader()
    {

    }
    public AttackType ReadAttack()
    {
        if (Math.Abs(Entity.Collider.X - Owner.Collider.X) <= AttackRange)
        {
            return AttackType.BASE;
        }
        return AttackType.NONE;
    }

    public Vector2 ReadInput()
    {
        Vector2 direction = Vector2.Zero;
        if (Math.Abs(Entity.Collider.X - Owner.Collider.X) <= FollowRange)
        {
            // if (Entity.Collider.X > Owner.Collider.X)
            // {
            //     System.Console.WriteLine("Entity BIGGER X");
            //     direction.X += 1;
            // }
            // else
            // {
            //     System.Console.WriteLine("Player BIGGER X");
            //     direction.X -= 1;
            // }
            float dx = Entity.Collider.X - Owner.Collider.X;
            if (dx > 0)
            {
                // System.Console.WriteLine("Moving Right");
                direction.X += 1;
            }
            else if (dx < 0)
            {
                
                // System.Console.WriteLine("Moving Left");
                direction.X -= 1;  // do nothing if dx == 0
            }
        }
        return direction;
    }
}
