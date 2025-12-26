using Microsoft.Xna.Framework;

namespace project;

public interface IMovable
{
    public Vector2 Position { get; set; }
    public Vector2 Speed { get; set; }
    public IInputReader InputReader { get; set; }
    public bool IsGrounded { get; set; }
    public MovementManager MovementManager { get; set; }
    public Vector2 FutureDirection { get; set; }
    public float JumpPower { get; set; }
    public float AirMoveSpeed { get; set; }
    public bool IsDoubleJumpAvailable { get; set; }
    public float JumpingSpeed { get; set; }
}