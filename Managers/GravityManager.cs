namespace project;

public class GravityManager
{
    public static float GRAVITY_POWER = 6f;
    public void Apply(IGravityAffected gravityAffected)
    {
        if (!gravityAffected.IsGrounded)
        {
            if (gravityAffected.GravityHoldTimer <= 0)
            {
                var tmp = gravityAffected.Position;
                tmp.Y += GRAVITY_POWER;
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
