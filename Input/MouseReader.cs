using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace project;

public class MouseReader : IInputReader
{
    public bool IsDestinationInput => true;

    public AttackType ReadAttack()
    {
        throw new System.NotImplementedException();
    }

    public Vector2 ReadInput()
    {
        MouseState state = Mouse.GetState();
        Vector2 directionMouse = new Vector2(state.X, state.Y);
        return directionMouse;
    }
}
