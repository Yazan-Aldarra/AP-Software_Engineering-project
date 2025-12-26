using Microsoft.Xna.Framework;
using Interfaces;

namespace project;

public class JumpingState : GameObjectState
{
    private float JumpPos;
    private bool IsJumpPosReached = false;
    private int doubleJumpCooldown;
    private Direction previousDirection;
    public override AnimationType AnimationType => AnimationType.IN_AIR;
    private IMovable movable;
    private float doubleJumpSpeed;
    private float originalJumpSpeed;
    private bool isDoubleJumpUsed;
    public JumpingState(IGameObject gameObject) : base(gameObject)
    {
        movable = gameObject as IMovable;
        JumpPos = movable.Position.Y - movable.JumpPower;

        Move();

        movable.FutureDirection = Vector2.Zero;
        doubleJumpCooldown = (int)(movable.JumpingSpeed * 0.75);
        isDoubleJumpUsed = false;

        originalJumpSpeed = movable.JumpingSpeed;
        doubleJumpSpeed = originalJumpSpeed * 2;
    }

    public void Move()
    {
        IsJumpPosReached = movable.MovementManager.JumpTill(movable, JumpPos);
        movable.MovementManager.MoveInAir(movable);
    }

    public override void Update()
    {
        var direction = Utils.GetDirection(movable.FutureDirection);

        if (IsJumpPosReached)
        {
            gameObject.State = new FallingState(gameObject);
            OnExit();
            return;
        }

        if (movable.IsDoubleJumpAvailable && !isDoubleJumpUsed && direction == Direction.UP &&
                previousDirection != direction && doubleJumpCooldown <= 0)
        {
            System.Console.WriteLine("SHOULD COME ONCE");
            JumpPos = movable.Position.Y - movable.JumpPower * .70f;
            movable.JumpingSpeed = doubleJumpSpeed;

            isDoubleJumpUsed = true;
        }
        else { doubleJumpCooldown--; previousDirection = direction; }
        Move();
    }
    private void OnExit()
    {
        movable.JumpingSpeed = originalJumpSpeed;
    }
}