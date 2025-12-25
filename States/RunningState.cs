using Microsoft.Xna.Framework;
using Interfaces;

namespace project;

public class RunningState : GameObjectState
{
    public override AnimationType AnimationType { get; } = AnimationType.RUNNING;
    public RunningState(IMovableGameObject gameObject) : base(gameObject)
    {
        Move();
    }
    public override void Move()
    {
        gameObject.MovementManager.Move(gameObject);
    }

    public override void Update()
    {

        var direction = gameObject.FutureDirection;
        var dir = Utils.GetDirection(direction);

        _ = dir switch
        {
            Direction.NONE => gameObject.State = new StandingState(gameObject),

            Direction.LEFT or Direction.RIGHT or Direction.DOWN or
            Direction.LEFT_DOWN or Direction.RIGHT_DOWN
                => gameObject.State = this,

            Direction.RIGHT_TOP or Direction.LEFT_TOP or
            Direction.UP
                => gameObject.State = new JumpingState(gameObject),

            _ => gameObject.State = this
        };
        Move();
    }
}