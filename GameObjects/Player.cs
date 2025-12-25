using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project;

public class Player :  IMovableGameObject, IGravityAffected, ICollider, IAnimatable
{

    public Texture2D Texture2D { get; set; }
    private float scale = 3f;
    public float Scale { get => scale; set => scale = value; }

    private Rectangle colliderRec;
    public Rectangle ColliderRec { get => colliderRec; set => colliderRec = value; }

    private int xDrawingsCount;
    private int yDrawingsCount;

    private Vector2 position;
    public Vector2 Position { get => position; set => position = value; }

    private Vector2 speed;
    public Vector2 Speed { get => speed; set => speed = value; }

    // MouseState state;
    // Vector2 mouseVector;

    private IInputReader inputReader;
    public IInputReader InputReader { get => inputReader; set => inputReader = value; }

    private Dictionary<AnimationType, Animation> animations;
    private MovementManager movementManager;
    public MovementManager MovementManager { get => movementManager; set => movementManager = value; }
    public bool IsGrounded { get; set; }
    public string Tag { get; set; }

    private AnimationManager animationManager;
    public AnimationManager AnimationManager { get => animationManager; set => animationManager = value; }

    private ColliderManager colliderManager;
    private GravityManager gravityManager;
    private Texture2D colliderTexture2d;

    // private AnimationType currentAnimation;
    // public AnimationType CurrentAnimation { get => currentAnimation; set => currentAnimation = value; }

    public int GravityHoldTimer { get; set; }

    public GameObjectState State { get; set; } 

    public Vector2 FutureDirection { get; set; }
    public float JumpPower { get; set; }
    public float AirMoveSpeed { get; set; }
    public Player(Texture2D texture2D, int xDrawingsCount, int yDrawingsCount, IInputReader inputReader, Texture2D colliderTexture2d)
    {
        IsGrounded = false;
        GravityHoldTimer = 3;
        Texture2D = texture2D;
        this.colliderTexture2d = colliderTexture2d;

        this.xDrawingsCount = xDrawingsCount;
        this.yDrawingsCount = yDrawingsCount;

        ColliderRec = new Rectangle(0, 0, texture2D.Width / xDrawingsCount, texture2D.Height / yDrawingsCount);

        animations = new Dictionary<AnimationType, Animation>();

        // currentAnimation = AnimationType.STANDING;

        AddAnimation(AnimationType.STANDING, 1);
        AddAnimation(AnimationType.RUNNING, 2);
        AddAnimation(AnimationType.ATTACKING, 3);
        AddAnimation(AnimationType.TAKING_DAMAGE, 4);
        AddAnimation(AnimationType.DYING, 5);
        AddAnimation(AnimationType.IN_AIR, 6);


        Position = new Vector2(0, 50);
        Speed = new Vector2(10, 0);
        AirMoveSpeed = Speed.X/2;
        JumpPower = 150f;

        this.InputReader = inputReader;

        movementManager = new MovementManager();
        gravityManager = new GravityManager();
        colliderManager = new ColliderManager(this);
        animationManager = new AnimationManager(this);

        State = new FallingState(this);
    }

    public void Update(GameTime gameTime)
    {
        animations[State.AnimationType].Update(gameTime);
        colliderRec.X = (int)position.X;
        colliderRec.Y = (int)position.Y;

        var colliders = colliderManager.CheckForCollisions();
        if (colliders.Count > 0)
        {
            foreach (var col in colliders)
            {
                if (col is IPlatform) { IsGrounded = true; }
            }
        }
        else { IsGrounded = false; }


        // animationManager.HandleAnimation(IsGrounded);
        gravityManager.Apply(this);
        // Move();
        FutureDirection =  inputReader.ReadInput();
        State.Update();
       System.Console.WriteLine(State.AnimationType); 
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(colliderTexture2d, ColliderRec, Color.Green);
        spriteBatch.Draw(Texture2D, position, animations[State.AnimationType].CurrentFrame.SourceRectangle, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);

        // spriteBatch.Draw(Texture2D, position, animations[currentAnimation].CurrentFrame.SourceRectangle, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
    }
    public void AddAnimation(AnimationType animationType, int spriteRowNum)
    {
        animations.Add(animationType, new Animation());
        animations[animationType].ExtractAnimationFramesRow(Texture2D, xDrawingsCount, yDrawingsCount, spriteRowNum);
    }
    // private void Move()
    // {
    //     var direction = movementManager.Move(this);
    //     animationManager.HandleAnimation(direction);
    //     if (direction.Y < 0)
    //         GravityHoldTimer = 15;
    //     System.Console.WriteLine(currentAnimation);
    // }
    public void CropAnimationFrames(int verticalCropping, int horizontalCropping, List<Animation> animationsNotToCrop = null)
    {
        List<Animation> toCrop = new List<Animation>();
        if (animationsNotToCrop != null)
        {
            foreach (var animation in animations.Values)
            {
                toCrop = animations.Values.Where(a => !animationsNotToCrop.Contains(a)).ToList();
            }
        }
        else toCrop = animations.Values.ToList();
        toCrop.ForEach(a => a.CropAnimationFrames(verticalCropping, horizontalCropping));
        ColliderRec = new Rectangle(0, 0, toCrop[0].CurrentFrame.SourceRectangle.Width * (int)scale, toCrop[0].CurrentFrame.SourceRectangle.Height * (int)scale);
    }
    public void AddColliderTriggers(ICollider collider)
    {
        colliderManager.AddCollider(collider);
    }
}
