using System.Collections.Generic;
using System.Data;
using System.Linq;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project;

public class Player : IGameObject, IMovable, IGravityAffected, ICollider, IAnimatable, IAttacker
{

    public Texture2D Texture2D { get; set; }
    private float scale = 3f;
    public float Scale { get => scale; set => scale = value; }

    private Rectangle collider;
    public Rectangle Collider { get => collider; set => collider = value; }

    private int xDrawingsCount;
    private int yDrawingsCount;

    private Vector2 position;
    public Vector2 Position { get => position; set => position = value; }

    private Vector2 speed;
    public Vector2 Speed { get => speed; set => speed = value; }

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
    public ColliderManager ColliderManager { get => colliderManager; set => colliderManager = value; }

    private GravityManager gravityManager;
    private Texture2D colliderTexture2d;

    public int GravityHoldTimer { get; set; }

    public GameObjectState State { get; set; }

    public Vector2 FutureDirection { get; set; }
    public float JumpPower { get; set; }
    public float AirMoveSpeed { get; set; }
    public bool IsDoubleJumpAvailable { get; set; }
    public float JumpingSpeed { get; set; }
    public float Damage { get; set; }
    public AttackType FutureAttack { get; set; }
    public GameObjectState PreviousState { get; set; }

    public Player(Texture2D texture2D, int xDrawingsCount, int yDrawingsCount, IInputReader inputReader, Texture2D colliderTexture2d)
    {
        IsGrounded = false;
        GravityHoldTimer = 3;
        Texture2D = texture2D;
        this.colliderTexture2d = colliderTexture2d;

        this.xDrawingsCount = xDrawingsCount;
        this.yDrawingsCount = yDrawingsCount;

        animations = new Dictionary<AnimationType, Animation>();

        AddAnimation(AnimationType.IDLE, 1);
        AddAnimation(AnimationType.RUNNING, 2);
        AddAnimation(AnimationType.ATTACKING, 3);
        AddAnimation(AnimationType.TAKING_DAMAGE, 4);
        AddAnimation(AnimationType.DYING, 5);
        AddAnimation(AnimationType.IN_AIR, 6);
        AddAnimation(AnimationType.CROUCHING, 7);
        AddAnimation(AnimationType.IS_CROUCHED, 8);

        Position = new Vector2(0, 50);
        Speed = new Vector2(10, 0);
        AirMoveSpeed = Speed.X / 2;
        JumpPower = 200f;
        JumpingSpeed = JumpPower / Animation.FPS;
        IsDoubleJumpAvailable = true;

        this.InputReader = inputReader;

        movementManager = new MovementManager();
        gravityManager = new GravityManager();
        colliderManager = new ColliderManager();
        animationManager = new AnimationManager();

        State = new FallingState(this);

        var rec = animations[State.AnimationType].CurrentFrame.SourceRectangle;
        Collider = new Rectangle(0, 0, rec.Width * (int)scale, rec.Height * (int)scale);
    }

    public void Update(GameTime gameTime)
    {
        UpdateColliderPos();

        colliderManager.HandleCollisionLogic(this);
        gravityManager.HandleApplyingGravity(this, State);
        animationManager.HandleResettingAnimation(this, animations);

        FutureAttack = inputReader.ReadAttack();
        FutureDirection = inputReader.ReadInput();
        State.Update();

        animations[State.AnimationType].Update(gameTime);
        PreviousState = State;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture2D, position, animations[State.AnimationType].CurrentFrame.SourceRectangle, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(colliderTexture2d, Collider, Color.Green * .5f);
    }
    public void AddAnimation(AnimationType animationType, int spriteRowNum)
    {
        animations.Add(animationType, new Animation());
        animations[animationType].ExtractAnimationFramesRow(Texture2D, xDrawingsCount, yDrawingsCount, spriteRowNum);
    }
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
    }
    public void AddColliderTriggers(ICollider collider)
    {
        colliderManager.AddCollider(collider);
    }
    public void UpdateColliderPos()
    {
        var rec = animations[State.AnimationType].CurrentFrame.SourceRectangle;
        var vector = Utils.GetCenteredColliderPosition(this, rec);

        collider.X = (int)vector.X;
        collider.Y = (int)vector.Y;
    }
    public Vector2 GetGameObjectPos()
    {
        return Position;
    }
    public void SetColliderSize(int width, int height)
    {
        collider.Width = width * (int)scale;
        collider.Height = height * (int)scale;
    }
}
