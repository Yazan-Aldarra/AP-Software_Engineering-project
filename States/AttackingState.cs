using Interfaces;

namespace project;

public enum AttackType { BASE, NONE }
public class AttackingState : GameObjectState
{
    private IAttacker attacker;
    private IMovable movable;
    private int attackTimer;
    // private IAnimatable animatable;
    public AttackingState(IGameObject gameObject) : base(gameObject)
    {
        attacker = gameObject as IAttacker;
        movable = gameObject as IMovable;
        attackTimer = Animation.FPS*2;
        Update();
    }

    public override AnimationType AnimationType => AnimationType.ATTACKING;

    public override void Update()
    {
        if (attackTimer <= 0)
        {
            gameObject.State = new StandingState(gameObject);
            return;
        }
        attackTimer--;
    }

}
