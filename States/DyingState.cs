using System;
using System.Threading;
using Interfaces;

namespace project;

public class DyingState : GameObjectState
{
    int dyingTime = Animation.FPS;
    public DyingState(IGameObject gameObject) : base(gameObject)
    {
        Update();
    }

    public override AnimationType AnimationType => AnimationType.DYING;

    public override void Update()
    {
        if (--dyingTime <= 0)
            gameObject.State = new DeadState(gameObject);
    }
}
