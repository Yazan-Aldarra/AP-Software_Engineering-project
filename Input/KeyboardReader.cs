using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace project;

public class KeyboardReader : IInputReader
{
    public bool IsDestinationInput => false;
    public Vector2 ReadInput()
    {
        KeyboardState state = Keyboard.GetState();
        Vector2 direction = Vector2.Zero;
        if (state.IsKeyDown(Keys.A))
        {
            direction.X -= 1;
        }
        if (state.IsKeyDown(Keys.D))
        {
            direction.X += 1;
        }
        if (state.IsKeyDown(Keys.W))
        {
            direction.Y -= 1;
        }
        return direction;
    }
}
