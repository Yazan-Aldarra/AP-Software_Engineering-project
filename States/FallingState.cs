using System;
using System.ComponentModel;
using Interfaces;

namespace project;

public class FallingState : GameObjectState
{
    public override AnimationType AnimationType { get; } = AnimationType.IN_AIR;
    private IMovable movable;
    public FallingState(IGameObject gameObject) : base(gameObject)
    {
        movable = gameObject as IMovable;
        Move();
    }

    public void Move()
    {
        movable.MovementManager.MoveInAir(movable);
    }

    public override void Update()
    {

        if (movable.IsGrounded)
        {
            gameObject.State = new StandingState(gameObject);
        }
        else Move();
    }
}
