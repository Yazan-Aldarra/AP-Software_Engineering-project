using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project;

public abstract class GameObject : IGameObject, ICollidable, IAnimatable
{
    // Basic graphics / animation fields
    public Texture2D Texture2D { get; set; }
    private float scale = 1f;
    public virtual float Scale { get => scale; set => scale = value; }

    // Animation storage
    protected Dictionary<AnimationType, Animation> animations;
    protected AnimationManager animationManager;

    // Collider / collision helpers
    private Rectangle collider;
    public Rectangle Collider { get => collider; set => collider = value; }
    public Rectangle PreviousCollider { get; set; }
    public string Tag { get; set; }
    protected ColliderManager colliderManager;
    private Texture2D colliderTexture2d;

    // Drawing / sprite sheet layout
    private int xDrawingsCount;
    private int yDrawingsCount;

    // Position helper (useful for Draw / GetGameObjectPos)
    protected Vector2 position;
    public virtual Vector2 GetGameObjectPos() => position;

    // State tracking (from IAnimatable / IGameObject)

    public GameObjectState State { get; set; }
    public GameObjectState PreviousState { get; set; }

    // Constructor provides common initialization used by derived objects (like Player)
    protected GameObject(Texture2D texture2D, Vector2? initialPos = null, int width = 10, int height = 10, float scale = 1f, Animation animation = null, int xDrawingsCount = 1, int yDrawingsCount = 1, Texture2D colliderTexture2d = null)
    {
        Texture2D = texture2D;
        this.xDrawingsCount = xDrawingsCount;
        this.yDrawingsCount = yDrawingsCount;
        this.colliderTexture2d = colliderTexture2d;

        animations = new Dictionary<AnimationType, Animation>();
        animationManager = new AnimationManager();
        colliderManager = new ColliderManager();

        position = initialPos ?? Vector2.Zero;
        Scale = scale;

        // if a single animation object is provided, register it as IDLE by default
        if (animation != null)
            animations[AnimationType.IDLE] = animation;

        // initialize a default collider size from provided width/height
        Collider = new Rectangle((int)position.X, (int)position.Y, width * (int)Scale, height * (int)Scale);
        PreviousCollider = Collider;

        // if (animation == null)
        //     animations.Add(AnimationType.IDLE, new Animation(new AnimationFrame(Collider)));
    }

    // IGameObject.Update - base handles animation updates and resetting when state changes
    public virtual void Update(GameTime gameTime)
    {
        PreviousCollider = Collider;
        UpdateColliderPos();
        // Reset previous animation when state changed
        animationManager.HandleResettingAnimation(this, animations);

        // Update current animation frame if present
        if (State != null && animations.ContainsKey(State.AnimationType))
        {
            animations[State.AnimationType].Update(gameTime);
        }
    }

    // IGameObject.Draw - draws current frame if available, otherwise whole texture; also draws collider debug if texture provided
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (State != null && animations.ContainsKey(State.AnimationType))
        {
            var src = animations[State.AnimationType].CurrentFrame.SourceRectangle;
            spriteBatch.Draw(Texture2D, position, src, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
        }
        else if (Texture2D != null && animations.ContainsKey(AnimationType.IDLE))
        {
            spriteBatch.Draw(Texture2D, position, animations[AnimationType.IDLE].CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
        }
        if (colliderTexture2d != null)
            spriteBatch.Draw(colliderTexture2d, Collider, Color.Green * .5f);
    }

    // IAnimatable members
    public void AddAnimation(AnimationType animationType, int spriteRowNum)
    {
        if (animations.ContainsKey(animationType))
            throw new Exception($"Animation {animationType} already exists");
        animations.Add(animationType, new Animation());
        animations[animationType].ExtractAnimationFramesRow(Texture2D, xDrawingsCount, yDrawingsCount, spriteRowNum);
    }

    public void CropAnimationFrames(int verticalCropping, int horizontalCropping, List<Animation> animationsNotToCrop = null)
    {
        List<Animation> toCrop;
        if (animationsNotToCrop != null)
            toCrop = animations.Values.Where(a => !animationsNotToCrop.Contains(a)).ToList();
        else
            toCrop = animations.Values.ToList();

        toCrop.ForEach(a => a.CropAnimationFrames(verticalCropping, horizontalCropping));
    }

    // ICollidable members - delegate to ColliderManager / provide helper methods used by other systems
    public void AddColliderTriggers(ICollidable collider)
    {
        colliderManager.AddCollider(collider);
    }

    public List<Collision> CheckForCollisions<T>(ICollidable collider) where T : GameObject, IMovable
    {
        return colliderManager.CheckForCollisions<T>(collider);
    }

    public void HandleCollisions(ICollidable? decorator, List<Collision> colliders)
    {
        // If this GameObject also implements IMovable (like Player) call the generic handler via dynamic dispatch
        if (this is IMovable)
        {
            colliderManager.HandleCollisions((dynamic)this, decorator, colliders);
            return;
        }
        // Fallback: no-op for non-movable GameObjects (implementations that need full collision resolution should override)
    }

    public void HandleAttackCollisions<T>(T obj, List<Collision> colliders) where T : ICollidable, IAttack
    {
        colliderManager.HandleAttackCollisions(obj, colliders);
    }

    public void SetColliderSize(int width, int height)
    {
        collider.Width = width * (int)Scale;
        collider.Height = height * (int)Scale;
    }

    protected void UpdateColliderPos()
    {
        if (State != null && animations.ContainsKey(State.AnimationType))
        {
            var rec = animations[State.AnimationType].CurrentFrame.SourceRectangle;
            var vector = Utils.GetCenteredColliderPosition(this, rec);
            collider.X = (int)vector.X;
            collider.Y = (int)vector.Y;
        }
        else
        {
            // Fallback keeps collider positioned at GameObject position
            collider.X = (int)position.X;
            collider.Y = (int)position.Y;
        }
    }
    public void UpdateColliderPos(int x, int y)
    {
        collider.X = x;
        collider.Y = y;
        position.X = x;
        position.Y = y;
    }
}
