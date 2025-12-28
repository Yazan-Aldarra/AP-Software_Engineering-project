using System.Collections.Generic;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project;

public class Player2 : Entity, IAttacker, IGravityAffected, IHasHealth
{
    // Position is inherited from GameObject (protected)

    private GravityManager gravityManager;

    public int GravityHoldTimer { get; set; }

    public AttackType FutureAttack { get; set; }

    private float health;
    public float Health => health;

    public Player2(Texture2D texture2D, IInputReader inputReader,int xDrawingsCount = 8, int yDrawingsCount = 8, Texture2D colliderTexture2d = null)
        : base(texture2D, inputReader,xDrawingsCount, yDrawingsCount, colliderTexture2d)
    {

        Position = new Vector2(0, 100);

        FutureAttack = AttackType.NONE;

        // defaults similar to Player
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

        var rec = animations[State.AnimationType].CurrentFrame.SourceRectangle;
        Collider = new Rectangle(0, 0, rec.Width * (int)Scale, rec.Height * (int)Scale);
    }

    public override void Update(GameTime gameTime)
    {
        // apply gravity 
        gravityManager.HandleApplyingGravity(this, State);
        
        // gather input
        FutureAttack = InputReader?.ReadAttack() ?? AttackType.NONE;

        // base handles animation frame updates and resets
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }

    public void DecreaseHealth(float value) => health -= value;
}
