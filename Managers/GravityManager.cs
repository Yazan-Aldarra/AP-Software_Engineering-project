namespace project;

public class GravityManager
{
    public void Apply(IGravityAffected gravityAffected)
    {
        if (!gravityAffected.IsGrounded)
        {
            if (gravityAffected.GravityHoldTimer <= 0)
            {
                var tmp = gravityAffected.Position;
                tmp.Y += 6;
                gravityAffected.Position = tmp;
            }
            else gravityAffected.GravityHoldTimer--;
        }
    }
    public void HandleApplyingGravity(IGravityAffected gravityAffected, GameObjectState state)
    {
        if (!(state is JumpingState))
            Apply(gravityAffected);
    }
}
