using Microsoft.Xna.Framework;
namespace project;

public interface IInputReader
{
    public Vector2 ReadInput();
    public AttackType ReadAttack();
    public bool IsDestinationInput { get; }
}
