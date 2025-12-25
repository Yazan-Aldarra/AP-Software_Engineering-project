using System;
using Microsoft.Xna.Framework;
using Interfaces;
using System.Reflection;

namespace project;

public class JumpingState : GameObjectState
{
    private int jumpingCount;
    private int doubleJumpCooldown;
    private float JumpPos;
    private bool IsJumpPosReached = false;
    public override AnimationType AnimationType { get; } = AnimationType.IN_AIR;
    public JumpingState(IMovableGameObject gameObject) : base(gameObject)
    {
        JumpPos = gameObject.Position.Y - gameObject.JumpPower;
        Move();
    }

    public override void Move()
    {
        IsJumpPosReached = gameObject.MovementManager.JumpTill(gameObject, JumpPos);
        gameObject.MovementManager.MoveInAir(gameObject);
    }

    public override void Update()
    {
        if (IsJumpPosReached)
        {
            gameObject.State = new FallingState(gameObject);
        } else Move();
    }
}