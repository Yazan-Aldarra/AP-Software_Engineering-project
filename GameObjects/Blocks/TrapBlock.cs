using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project;

public class TrapBlock : Block, IAttacker, IAttack
{
    public TrapBlock(Texture2D texture2D,
            Vector2? initialPos = null,
            int width = 10,
            int height = 10,
            float scale = 1f,
            Animation animation = null,
            int xDrawingsCount = 1,
            int yDrawingsCount = 1,
            Texture2D colliderTexture2d = null)
    : base(texture2D,
        initialPos,
        width,
        height,
        scale,
        animation,
        xDrawingsCount,
        yDrawingsCount,
        colliderTexture2d)
    {

    }
    public AttackType FutureAttack { get; set; }
    public float Damage { get; set; } = 100f;
    public bool isActive { get; set; } = true;
    private float reAttackTimer = 0f;
    private float reAttackDelay = Animation.FPS * 2;

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (--reAttackTimer <= 0)
        {
            isActive = true;
        }
    }
    public void UseAttack(IHasHealth hasHealth)
    {
        if (isActive)
            hasHealth.DecreaseHealth(Damage);
        isActive = false;
        reAttackTimer = reAttackDelay;
    }
}
