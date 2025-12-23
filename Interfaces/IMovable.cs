using System.Reflection;
using Microsoft.Xna.Framework;

namespace project;

public interface IMovable
{
    public Vector2 Position {get; set;}
    public Vector2 Speed { get; set;}
    public IInputReader InputReader { get; set; }
}
