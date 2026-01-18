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
    public float jumpPower;
    public float JumpPower
    {
        get => jumpPower; set
        {
            jumpPower = value;
            JumpingSpeed = jumpPower / Animation.FPS;
        }
    }
    public float AirMoveSpeed { get; set; }
    public bool IsDoubleJumpAvailable { get; set; }
    public float JumpingSpeed { get; set; }
    public List<Direction> BlockedSide { get; set; }

    // New convenience constructor matching the GameObject default signature
    protected Entity(Texture2D texture2D, IInputReader inputReader, Vector2? initialPos = null, int width = 10, int height = 10, float scale = 1f, Animation animation = null, int xDrawingsCount = 5, int yDrawingsCount = 5, Texture2D colliderTexture2d = null)
        : base(texture2D, initialPos, width, height, scale, animation, xDrawingsCount, yDrawingsCount, colliderTexture2d)
    {
        // defaults similar to player2
        BlockedSide = new List<Direction>();
        Speed = new Vector2(5, 0);
        AirMoveSpeed = Speed.X * .5f;
        JumpPower = 100f;
        JumpingSpeed = JumpPower / Animation.FPS;
        IsDoubleJumpAvailable = true;

        InputReader = inputReader;

        // InputReader remains null unless caller provides it explicitly via property
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

        // Sync collider to the new position after movement so collision checks use current frame rect
        UpdateColliderPos();
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
