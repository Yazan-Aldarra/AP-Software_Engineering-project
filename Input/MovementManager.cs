using System.Diagnostics.Contracts;
using System.IO.Pipes;
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

        var isReached = false;
        var futureY = movable.Position.Y - movable.JumpingSpeed;
        var futureX = movable.Position.X;

        if (futureY <= point) { futureY = point; isReached = true; }

        System.Console.WriteLine($"point to reach: {point}");
        System.Console.WriteLine($"Pos.Y: {futureY}");
        System.Console.WriteLine();
        movable.Position = new Vector2(futureX, futureY);

        return isReached;
    }
    public void MoveInAir(IMovable movable)
    {
        var futureX = movable.Position.X + movable.FutureDirection.X * movable.AirMoveSpeed;
        var futureY = movable.Position.Y;

        movable.Position = new Vector2(futureX, futureY);
    }
}
