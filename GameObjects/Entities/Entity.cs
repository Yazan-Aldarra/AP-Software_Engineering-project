using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project;

public abstract class Entity : GameObject, IMovable
{
    // IMovable implementation - backed by GameObject.position/systems
    public Vector2 Position { get => position; set => position = value; }

    private Vector2 speed;
    public Vector2 Speed { get => speed; set => speed = value; }

    public IInputReader InputReader { get; set; }
    public MovementManager MovementManager { get; set; }
    public bool IsGrounded { get; set; }

    public Vector2 FutureDirection { get; set; }
    public float JumpPower { get; set; }
    public float AirMoveSpeed { get; set; }
    public bool IsDoubleJumpAvailable { get; set; }
    public float JumpingSpeed { get; set; }
    public List<Direction> BlockedSide { get; set; }

    protected Entity(Texture2D texture2D, IInputReader inputReader, int xDrawingsCount = 5, int yDrawingsCount = 5 , Texture2D colliderTexture2d = null)
        : base(texture2D, xDrawingsCount, yDrawingsCount, colliderTexture2d)
    {
        // defaults similar to player2
        Scale = 3f;
        BlockedSide = new List<Direction>();
        Speed = new Vector2(10, 0);
        AirMoveSpeed = Speed.X / 2;
        JumpPower = 200f;
        JumpingSpeed = JumpPower / Animation.FPS;
        IsDoubleJumpAvailable = true;

        InputReader = inputReader;

        MovementManager = new MovementManager();

        State = new StandingState(this);
    }

    public override void Update(GameTime gameTime)
    {
        // gather inputs (if available)
        FutureDirection = InputReader?.ReadInput() ?? Vector2.Zero;

        // base updates animation frames and resets
        base.Update(gameTime);

        PreviousState = State;
        State.Update();

        var collisions = colliderManager.CheckForCollisions<Entity>(this);
        colliderManager.HandleCollisions(this, null, collisions);
        SetOverlappedObjectBack(this, collisions);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }

    public void SetOverlappedObjectBack<T>(T obj, System.Collections.Generic.List<Collision> collisions) where T : ICollidable, IMovable
    {
        var uniqueCollision = colliderManager.GetHighestCollisions(collisions);
        MovementManager.SetOverlappedObjectBack(obj, uniqueCollision);
    }

    public void SetOverlappedObjectBack(System.Collections.Generic.List<Collision> DecoratorCollisions)
    {
        var uniqueCollision = colliderManager.GetHighestCollisions(DecoratorCollisions);
        MovementManager.SetOverlappedObjectBack(this, uniqueCollision);
    }
}
