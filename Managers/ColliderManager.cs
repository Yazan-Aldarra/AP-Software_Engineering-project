using System.Collections.Generic;
using Interfaces;

namespace project;

public class ColliderManager
{
    private List<ICollider> otherColliders;
    public ColliderManager()
    {
        otherColliders = new List<ICollider>();
    }
    public List<ICollider> CheckForCollisions(ICollider collider)
    {
        var tmp = new List<ICollider>();
        foreach (var otherCol in otherColliders)
        {
            if (otherCol.Collider.Intersects(collider.Collider))
            {
                tmp.Add(otherCol);
            }
        }
        return tmp;
    }
    public void HandleCollisionsWithPlatform<T>(T obj, List<ICollider> colliders) where T : ICollider, IMovable
    {
        // var colliders = CheckForCollisions(obj);
        if (colliders.Count > 0)
        {
            foreach (var col in colliders)
            {
                if (col is IPlatform)
                    obj.IsGrounded = true;
            }
        }
        else { obj.IsGrounded = false; }
    }
    public void HandleCollisionsWithAttacks<T>(T obj, List<ICollider> colliders) where T : ICollider
    {
        // var colliders = CheckForCollisions(obj);
        if (colliders.Count > 0)
        {
            foreach (var col in colliders)
            {
                if (col is IAttacker && col.Tag != "player")
                {
                    // var attack =  col as IAttacker;
                    // var heatlh = obj as IHasHealth;
                    // obj.DecreaseHealth(attack.Damage);
                }
            }
        }
        else {  }
    }
    public void AddCollider(ICollider collider)
    {
        otherColliders.Add(collider);
    }
}
