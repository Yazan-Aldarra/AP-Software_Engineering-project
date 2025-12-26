using Interfaces;

namespace project;

public enum AttackType { BASE, NONE }
public class AttackingState : GameObjectState
{
    private IAttacker attacker;
    private IMovable movable;
    private int attackTimer = Animation.FPS;
    public AttackingState(IGameObject gameObject) : base(gameObject)
    {
        attacker = gameObject as IAttacker;
        movable = gameObject as IMovable;
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
