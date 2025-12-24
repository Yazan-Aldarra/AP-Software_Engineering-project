using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace project;

public class Player : IGameObject, IMovable, IGravityAffected, ICollider, IAnimatable
{
    private Texture2D Texture2D;
    private float scale = 3f;
    private Rectangle colliderRec;
    public Rectangle ColliderRec { get => colliderRec; set => colliderRec = value; }
    private int xDrawingsCount;
    private int yDrawingsCount;
    private Vector2 position;
    public Vector2 Position { get => position; set => position = value; }
    private Vector2 speed;
    public Vector2 Speed { get => speed; set => speed = value; }
    MouseState state;
    Vector2 mouseVector;
    
    private IInputReader inputReader;
    public IInputReader InputReader { get => inputReader; set => inputReader = value; }
    
    private Dictionary<AnimationType, Animation> animations;
    private MovementManager movementManager;
    public bool IsGrounded { get; set; }
    public string Tag { get; set; }

    private AnimationManager animationManager;
    public AnimationManager AnimationManager { get => animationManager; set => animationManager = value; }

    private ColliderManager colliderManager;
    private GravityManager gravityManager;
    private Texture2D colliderTexture2d;

    private AnimationType currentAnimation;
    public AnimationType CurrentAnimation { get => currentAnimation; set => currentAnimation = value; }
    
    public Player(Texture2D texture2D, int xDrawingsCount, int yDrawingsCount, IInputReader inputReader, Texture2D colliderTexture2d)
    {
        IsGrounded = false;
        Texture2D = texture2D;
        this.colliderTexture2d = colliderTexture2d;

        this.xDrawingsCount = xDrawingsCount;
        this.yDrawingsCount = yDrawingsCount;

        ColliderRec = new Rectangle(0,0, texture2D.Width / xDrawingsCount,texture2D.Height / yDrawingsCount);
        
        animations = new Dictionary<AnimationType, Animation>();
        currentAnimation = AnimationType.STANDING;
        AddAnimation(AnimationType.STANDING, 1);
        AddAnimation(AnimationType.RUNNING, 2);
        AddAnimation(AnimationType.ATTACKING, 3);
        AddAnimation(AnimationType.FALLING, 4);
        AddAnimation(AnimationType.DYING, 5);
        
        Position = new Vector2(0, 50);
        Speed = new Vector2(3, 2);
        
        this.InputReader = inputReader;
        movementManager = new MovementManager();

        gravityManager = new GravityManager();

        colliderManager = new ColliderManager(this);

        animationManager = new AnimationManager(this);
    }

    public void Update(GameTime gameTime)
    {
        animations[currentAnimation].Update(gameTime);
        colliderRec.X = (int) position.X;
        colliderRec.Y = (int) position.Y;

        var colliders =  colliderManager.CheckForCollisions();
        if (!(colliders.Count <= 0))
        {
            foreach (var col in colliders)
            {
                if (col is IPlatform)
                    IsGrounded = true;
            }
        } else IsGrounded = false;

        gravityManager.Apply(this);
        Move();
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(colliderTexture2d, ColliderRec, Color.Green);
        spriteBatch.Draw(Texture2D, position, animations[currentAnimation].CurrentFrame.SourceRectangle, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
    }
    public void AddAnimation(AnimationType animationType, int spriteRowNum)
    {
        animations.Add(animationType, new Animation());
        animations[animationType].ExtractAnimationFramesRow(Texture2D, xDrawingsCount, yDrawingsCount, spriteRowNum);
    }
    private void Move()
    {
        movementManager.Move(this);
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
        } else toCrop = animations.Values.ToList();
        toCrop.ForEach(a => a.CropAnimationFrames(verticalCropping, horizontalCropping));
        ColliderRec =  new Rectangle (0,0,toCrop[0].CurrentFrame.SourceRectangle.Width * (int) scale, toCrop[0].CurrentFrame.SourceRectangle.Height * (int) scale);
    }
    public void AddColliderTriggers(ICollider collider)
    {
        colliderManager.AddCollider(collider);
    }
}
