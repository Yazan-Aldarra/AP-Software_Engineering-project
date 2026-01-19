using System;
using Interfaces;

namespace project;

public class TakingDamageState : GameObjectState
{
    private float takingDamageTimer = Animation.FPS*2;
    public TakingDamageState(IGameObject gameObject) : base(gameObject)
    {
        Update();
    }

    public override AnimationType AnimationType => AnimationType.TAKING_DAMAGE;

    public override void Update()
    {
        if (--takingDamageTimer <= 0)
            gameObject.State = new StandingState(gameObject);
    }
}
