using Microsoft.Xna.Framework;

namespace project;

public interface ICollider
{
    public Rectangle Collider { get; set; }
    public string Tag { get; set; }
    public void UpdateColliderPos();
    public ColliderManager ColliderManager { get; set;}
}
