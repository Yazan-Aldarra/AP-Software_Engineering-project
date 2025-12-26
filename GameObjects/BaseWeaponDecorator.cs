using System.Linq;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project;

public class BaseWeaponDecorator : GameObjectDecorator, ICollider, IAttacker
{
    public float Damage { get; set; }
    public AttackType FutureAttack { get; set; }
    public BaseWeaponDecorator(IGameObject gameObject)
        : base(gameObject)
    {
        Damage = 5f;
        FutureAttack = AttackType.BASE;
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        spriteBatch.Draw(Texture2D, Collider, Color.Red);
    }
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        UpdateColliderPos();
        var colliders = ColliderManager.CheckForCollisions(this);

        foreach (var col in colliders)
        {
            if (col is IEnemy)
            {
                var enemy = col as IHasHealth;
                enemy.DecreaseHealth(Damage);
            }
        }
    }
    public override void UpdateColliderPos()
    {
        var pos = GetGameObjectPos();
        collider.X = (int)pos.X;
        collider.X = (int)pos.X;
    }
}
