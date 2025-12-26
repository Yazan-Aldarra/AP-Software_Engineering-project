using Microsoft.Xna.Framework;
using Interfaces;

namespace project;

public class RunningState : GameObjectState
{
    public override AnimationType AnimationType { get; } = AnimationType.RUNNING;
    private IMovable movable;
    private IAttacker attacker;
    public RunningState(IGameObject gameObject) : base(gameObject)
    {
        attacker = gameObject as IAttacker;
        movable = gameObject as IMovable;
        Move();
    }
    public void Move()
    {
        movable.MovementManager.Move(movable);
    }

    public override void Update()
    {

        var direction = movable.FutureDirection;
        var dir = Utils.GetDirection(direction);

        if (attacker != null)
        {
            if (attacker.FutureAttack != AttackType.NONE)
            {
                gameObject.State = new AttackingState(gameObject);
                return;
            }
        }
        _ = dir switch
        {
            Direction.NONE => gameObject.State = new StandingState(gameObject),

            Direction.LEFT or Direction.RIGHT
                => gameObject.State = this,

            Direction.DOWN
                => gameObject.State = new CrouchingState(gameObject),

            Direction.UP
                => gameObject.State = new JumpingState(gameObject),

            _ => gameObject.State = this
        };
        Move();
    }
}