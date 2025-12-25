
namespace project;

public class StandingState : GameObjectState
{
    public StandingState(IMovableGameObject gameObject) : base(gameObject) { }
    public override AnimationType AnimationType { get; } = AnimationType.STANDING;
    public override void Move() { }
    public override void Update()
    {
        var direction = gameObject.FutureDirection;

        var dir = Utils.GetDirection(direction);

        if (dir == Direction.NONE)
        {

        }
        else
        {
            _ = dir switch
            {
                Direction.NONE => gameObject.State = this,

                Direction.LEFT or Direction.RIGHT or
                Direction.DOWN or Direction.LEFT_DOWN or Direction.RIGHT_DOWN
                    => gameObject.State = new RunningState(gameObject),

                Direction.UP or
                Direction.LEFT_TOP or Direction.RIGHT_TOP
                    => gameObject.State = new JumpingState(gameObject),

                _ => gameObject.State = this
            };
        }


    }
}
