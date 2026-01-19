using System.Reflection.PortableExecutable;
using System.Text;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project;

public class BaseWeaponDecorator<TGameObject> : EntityDecorator<TGameObject>, IAttack
    where TGameObject : Entity
{
    public float Damage { get; set; } = 50f;
    public bool isActive { get; set; } = false;
    private float reAttackTimer = 0f;
    private float reAttackDelay = Animation.FPS * 2;
    public BaseWeaponDecorator(TGameObject gameObject, Texture2D texture2D)
        : base(gameObject, texture2D)
    {
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        if (gameObject.State is AttackingState)
            spriteBatch.Draw(Texture2D, Collider, Color.Red * 0.5f);
    }
    public override void Update(GameTime gameTime)
    {

        if ((gameObject.State is AttackingState))
        {
            var collisions = CheckForCollisions<TGameObject>(this);

            HandleCollisions(this, collisions);
        }
        base.Update(gameTime);

        if (isActive)
            System.Console.WriteLine($"PRE: {isActive}");
        if (reAttackTimer <= 0 && gameObject.State is AttackingState)
        {
            isActive = true;
        }
        else if (reAttackTimer > 0)
        {
            reAttackTimer--;
        }
        else if (!(gameObject.State is AttackingState))
        {
            isActive = false;
        }
        if (isActive)
            System.Console.WriteLine($"After: {isActive}");
    }
    protected override void UpdateColliderPos()
    {
        base.UpdateColliderPos();

        // var c = Collider;
        // c.Y += gameObject.Collider.Height - c.Height;
        // c.X += gameObject.Collider.Width;
        // Collider = c;
    }
    public void UseAttack(IHasHealth hasHealth)
    {
        if (isActive)
            hasHealth.DecreaseHealth(Damage);

        isActive = false;
        reAttackTimer = reAttackDelay;
    }
}
