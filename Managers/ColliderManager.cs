using System.Collections.Generic;

namespace project;

public class ColliderManager
{
    private ICollider collider;
    private List<ICollider> otherColliders;
    public ColliderManager(ICollider collider)
    {
        this.collider = collider;
        otherColliders = new List<ICollider>();
    }
    public List<ICollider> CheckForCollisions()
    {
        var tmp = new List<ICollider>();
        foreach (var collider in otherColliders)
        {
            if (collider.ColliderRec.Intersects(this.collider.ColliderRec))
            {
                tmp.Add(collider);
            }
        }
        return tmp;
    }
    public void AddCollider(ICollider collider)
    {
        otherColliders.Add(collider);
    }
}
