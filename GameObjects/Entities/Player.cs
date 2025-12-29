using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project;

public class Player : Entity, IAttacker, IGravityAffected, IHasHealth
{
    // Position is inherited from GameObject (protected)

    private GravityManager gravityManager;

    public int GravityHoldTimer { get; set; }

    public AttackType FutureAttack { get; set; }

    private float health;
    public float Health => health;

    public Player(Texture2D texture2D, IInputReader inputReader, Vector2? initialPos = null, int width = 10, int height = 10, float scale = 2f, Animation animation = null, int xDrawingsCount = 8, int yDrawingsCount = 8, Texture2D colliderTexture2d = null)
        : base(texture2D, inputReader,initialPos, width, height, scale, animation, xDrawingsCount, yDrawingsCount, colliderTexture2d)
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

        var rec = animations[State.AnimationType].CurrentFrame.SourceRectangle;
        Collider = new Rectangle(0, 0, rec.Width * (int)Scale, rec.Height * (int)Scale);
    }

    public override void Update(GameTime gameTime)
    {
        // apply gravity 
        // gather input
        FutureAttack = InputReader?.ReadAttack() ?? AttackType.NONE;

        // base handles animation frame updates and resets
        base.Update(gameTime);
        gravityManager.HandleApplyingGravity(this, State);
        UpdateColliderPos();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }

    public void DecreaseHealth(float value) => health -= value;
}