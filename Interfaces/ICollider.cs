using Microsoft.Xna.Framework;

namespace project;

public interface ICollider
{
    public string Tag { get; set; }
    public Rectangle ColliderRec { get; set; }
}
