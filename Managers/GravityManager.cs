namespace project;

public class GravityManager
{
    public void Apply(IGravityAffected gravityAffected)
    {
        if (!gravityAffected.IsGrounded)
        {
            var tmp = gravityAffected.Position;
            tmp.Y  += 2;
            gravityAffected.Position = tmp;
        }
    }
}
