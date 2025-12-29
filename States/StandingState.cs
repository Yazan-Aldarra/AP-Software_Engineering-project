using System;
using Interfaces;

namespace project;

public class StandingState : GameObjectState
{
    private IAttacker attacker;
    private IMovable movable;
    public StandingState(IGameObject gameObject) : base(gameObject)
    {
        attacker = gameObject as IAttacker;
        movable = gameObject as IMovable;
    }
    public override AnimationType AnimationType { get; } = AnimationType.IDLE;
    public override void Update()
    {
        var direction = movable.FutureDirection;

        var dir = Utils.GetDirection(direction);

        if (!movable.IsGrounded && !movable.BlockedSide.Contains(Direction.DOWN))
        {
            gameObject.State = new FallingState(gameObject);
            return;
        }
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
            Direction.NONE => gameObject.State = this,

            Direction.LEFT or Direction.RIGHT
                => gameObject.State = new RunningState(gameObject),

            Direction.DOWN
                => gameObject.State = new CrouchingState(gameObject),

            Direction.UP
                => gameObject.State = new JumpingState(gameObject),

            _ => gameObject.State = this
        };
    }
}
