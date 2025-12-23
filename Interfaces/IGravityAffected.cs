using Microsoft.Xna.Framework;
namespace project;

public interface IGravityAffected
{
    public bool IsGrounded { get; set; }
    public Vector2 Position { get; set; }
}
