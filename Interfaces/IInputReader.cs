using Microsoft.Xna.Framework;
namespace project;

public interface IInputReader
{
    public Vector2 ReadInput();
    public bool IsDestinationInput { get; }
}
