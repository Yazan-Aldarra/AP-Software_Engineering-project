using Microsoft.Xna.Framework;

namespace project;

public class MovementManager
{
    public void Move(IMovable movable)
    {
        var direction = movable.InputReader.ReadInput();
        
        if (movable is IAnimatable)
        {
           var animatable = movable as IAnimatable;
           animatable.AnimationManager.HandleAnimation(direction);
        }

        if (movable.InputReader.IsDestinationInput)
        {
            direction -= movable.Position;
            direction.Normalize();
        }
        var distance = direction * movable.Speed;
        var futurePosition = movable.Position + distance;

        movable.Position = futurePosition;
        movable.Position += distance;
    }

}
