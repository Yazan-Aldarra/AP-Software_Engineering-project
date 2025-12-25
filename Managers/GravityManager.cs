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
                tmp.Y += 4;
                gravityAffected.Position = tmp;
            } else gravityAffected.GravityHoldTimer--;
        }
    }
}
