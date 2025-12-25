using System;
using System.ComponentModel;

namespace project;

public class FallingState : GameObjectState
{
    public override AnimationType AnimationType { get; } = AnimationType.IN_AIR;
    public FallingState(IMovableGameObject gameObject) : base(gameObject)
    {
        Move();
    }

    public override void Move()
    {
        gameObject.MovementManager.MoveInAir(gameObject);
    }

    public override void Update()
    {
        if (gameObject.IsGrounded)
        {
            gameObject.State = new StandingState(gameObject);
        } else Move();
    }
}
