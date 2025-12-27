using System.Text;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project;

public class BaseWeaponDecorator<TGameObject> : EntityDecorator<TGameObject>, IAttack
    where TGameObject : IGameObject, ICollidable, IMovable
{
    public float Damage { get; set; }
    public BaseWeaponDecorator(TGameObject gameObject, Texture2D texture2D)
        : base(gameObject, texture2D)
    {
        Damage = 5f;
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        if (gameObject.State is AttackingState)
        spriteBatch.Draw(Texture2D, Collider, Color.Red*0.5f);
    }
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (!(gameObject.State is AttackingState))
            return;

        var collisions = CheckForCollisions<TGameObject>(this);

        HandleCollisions(this, collisions);
        HandleAttackCollisions(this, collisions);
    }
    public override void UpdateColliderPos()
    {
        base.UpdateColliderPos();

        collider.Y += gameObject.Collider.Height - collider.Height;
        collider.X += gameObject.Collider.Width;
    }
}
