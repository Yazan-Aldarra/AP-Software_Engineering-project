using Interfaces;

namespace project;

public abstract class GameObjectState
{
    protected IGameObject gameObject;
    protected IGameObject GameObject { get => gameObject; set => gameObject = value; }
    public abstract AnimationType AnimationType { get; }
    public GameObjectState(IGameObject gameObject)
    {
        this.gameObject = gameObject;
    }
    public abstract void Update();
}
