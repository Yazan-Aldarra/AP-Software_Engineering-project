using Microsoft.Xna.Framework;

namespace project;

public class MovementManager
{
    public void Move(IMovable movable)
    {
        var direction = movable.FutureDirection;

        if (movable.InputReader.IsDestinationInput)
        {
            direction -= movable.Position;
            direction.Normalize();
        }
        var distance = direction * movable.Speed;
        var futurePosition = movable.Position + distance;

        movable.Position = futurePosition;
    }
    // returns true if the point is reached, and does not jump
    public bool JumpTill(IMovable movable, float point)
    {
        if (movable.Position.Y <= point)
            return true;

        var futureY = movable.Position.Y - movable.JumpPower/12;
        var futureX = movable.Position.X;

        movable.Position = new Vector2(futureX, futureY);
        return false;
    }
    public void MoveInAir(IMovable movable)
    {
        var futureX = movable.Position.X + movable.FutureDirection.X * movable.AirMoveSpeed;
        var futureY = movable.Position.Y;

        movable.Position = new Vector2(futureX, futureY);
    }
}
