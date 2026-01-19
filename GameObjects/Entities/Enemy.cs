using System;
using System.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project;

public class Enemy : Entity, IHasHealth, IAttacker, IGravityAffected
{
    private GravityManager gravityManager;
    public int GravityHoldTimer { get; set; }
    public AttackType FutureAttack { get; set; }
    public float Health { get; set; } = 100f;

    public Enemy(Texture2D texture2D, IInputReader inputReader, Vector2? initialPos = null, int width = 10, int height = 10, float scale = 1, Animation animation = null, int xDrawingsCount = 5, int yDrawingsCount = 5, Texture2D colliderTexture2d = null) : base(texture2D, inputReader, initialPos, width, height, scale, animation, xDrawingsCount, yDrawingsCount, colliderTexture2d)
    {
        gravityManager = new GravityManager();

        // register animations (same rows as Player)
        AddAnimation(AnimationType.IDLE, 4);
        AddAnimation(AnimationType.IN_AIR, 4);
        
        AddAnimation(AnimationType.RUNNING, 5);
        AddAnimation(AnimationType.ATTACKING, 1);
        AddAnimation(AnimationType.TAKING_DAMAGE, 6);
        AddAnimation(AnimationType.DYING, 3);
        AddAnimation(AnimationType.DEAD, 2);
    }

    public override void Update(GameTime gameTime)
    {

        // apply gravity 
        // gather input
        FutureAttack = InputReader?.ReadAttack() ?? AttackType.NONE;

        // base handles animation frame updates and resets
        base.Update(gameTime);

        var collisions = colliderManager.CheckForCollisions<Entity>(this);
        var tmp = colliderManager.GetHighestCollisions(collisions);
        colliderManager.HandleCollisions(this, null, tmp);

        SetOverlappedObjectBack(this, tmp);

        gravityManager.HandleApplyingGravity(this, State);
        UpdateColliderPos();

        colliderManager.HandleInComingAttack(this, collisions);
    }
    public void DecreaseHealth(float value)
    {
        Health -= value;
        if (Health < 0 && !(State is DyingState) && !(State is DeadState))
        {
            State = new DyingState(this);
        }
    }
}
