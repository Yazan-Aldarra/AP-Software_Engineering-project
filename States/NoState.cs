using System;
using Interfaces;

namespace project;

public class NoState : GameObjectState
{
    public NoState(IGameObject gameObject) : base(gameObject) { }
    public override AnimationType AnimationType => AnimationType.IDLE;
    public override void Update() { }
}
