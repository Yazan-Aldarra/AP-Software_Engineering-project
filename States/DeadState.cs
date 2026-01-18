using System;
using Interfaces;

namespace project;

public class DeadState : GameObjectState
{
    public DeadState(IGameObject gameObject) : base(gameObject)
    {
        Update();
    }

    public override AnimationType AnimationType => AnimationType.DEAD;

    public override void Update()
    {

    }
}
