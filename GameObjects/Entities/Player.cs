using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project;

public class Player : Entity, IAttacker, IGravityAffected, IHasHealth
{
    // Position is inherited from GameObject (protected)

    private GravityManager gravityManager;

    public int GravityHoldTimer { get; set; }

    public AttackType FutureAttack { get; set; }

    public float Health { get; set; }

    public Player(Texture2D texture2D,
            IInputReader inputReader,
            Vector2? initialPos = null,
            int width = 10,
            int height = 10,
            float scale = 2f,
            Animation animation = null,
            int xDrawingsCount = 8,
            int yDrawingsCount = 8,
            Texture2D colliderTexture2d = null)
        : base(texture2D,
            inputReader,
            initialPos,
            width,
            height,
            scale,
            animation,
            xDrawingsCount,
            yDrawingsCount,
            colliderTexture2d)
    {
        Position = initialPos ?? new Vector2(0, 100);
        FutureAttack = AttackType.NONE;
        gravityManager = new GravityManager();

        // register animations (same rows as Player)
        AddAnimation(AnimationType.IDLE, 1);
        AddAnimation(AnimationType.RUNNING, 2);
        AddAnimation(AnimationType.ATTACKING, 3);
        AddAnimation(AnimationType.TAKING_DAMAGE, 4);
        AddAnimation(AnimationType.DYING, 5);
        AddAnimation(AnimationType.IN_AIR, 6);
        AddAnimation(AnimationType.CROUCHING, 7);
        AddAnimation(AnimationType.IS_CROUCHED, 8);
        AddAnimation(AnimationType.DEAD, 8);

        var rec = animations[State.AnimationType].CurrentFrame.SourceRectangle;
        Collider = new Rectangle(0, 0, rec.Width * (int)Scale, rec.Height * (int)Scale);
        AirMoveSpeed *= 1.5f;
        JumpPower *= 1.8f;
        Health = 100f;
    }

    public override void Update(GameTime gameTime)
    {
        // apply gravity 
        // gather input
        FutureAttack = InputReader?.ReadAttack() ?? AttackType.NONE;

        // base handles animation frame updates and resets
        base.Update(gameTime);

        var collisions = colliderManager.CheckForCollisions<Entity>(this);
        var solid =  collisions.Where(c => !(c.Collider is IAttack) && !(c.Collider is EntityDecorator<Enemy>)).ToList();
        var tmp = colliderManager.GetHighestCollisions(solid);
        
        colliderManager.HandleCollisions(this, null, tmp);

        SetOverlappedObjectBack(this, tmp);

        gravityManager.HandleApplyingGravity(this, State);
        UpdateColliderPos();

        HandleInComingAttacks(this, collisions);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }

    public void DecreaseHealth(float value)
    {
        Health -= value;
        if (Health <= 0 && !(State is DyingState) && !(State is DeadState))
        {
            State = new DyingState(this);
        }
    }
    public void HandleInComingAttacks<T>(T obj, List<Collision> colliders)
        where T : ICollidable, IHasHealth
    {
        colliderManager.HandleInComingAttack(obj, colliders);
    }
}