using Interfaces;

namespace project;

public class CrouchingState : GameObjectState
{

    private int crouchingTimer;
    private AnimationType animationType = AnimationType.CROUCHING;
    public override AnimationType AnimationType => animationType;
    private IMovable movable;
    public CrouchingState(IGameObject gameObject) : base(gameObject)
    {
        movable = gameObject as IMovable;
    }

    public override void Update()
    {
        var direction = Utils.GetDirection(movable.FutureDirection);

        if (direction != Direction.DOWN)
        {
            gameObject.State = new StandingState(gameObject);
            return;
        }

        if (crouchingTimer <= 0)
            animationType = AnimationType.IS_CROUCHED;
        else crouchingTimer--;
    }
}
