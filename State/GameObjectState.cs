using System;
using Interfaces;
using Microsoft.Xna.Framework;

namespace project;

public abstract class GameObjectState
{
    protected IMovableGameObject gameObject;
    public IMovableGameObject GameObject { get => gameObject; set => gameObject = value; }
    public abstract AnimationType AnimationType { get; }
    public GameObjectState(IMovableGameObject gameObject)
    {
        this.gameObject = gameObject;
    }
    public abstract void Move();
    public abstract void Update();
}
