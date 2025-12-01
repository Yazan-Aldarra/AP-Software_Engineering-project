using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Wasm;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace project;

public class Player : IGameObject
{
    private Texture2D playerTexture2D;
    private Rectangle playerRectangle; 
    private int xDrawingsCount;
    private int yDrawingsCount;

    private Dictionary<string, Animation> animations;
    public Player(Texture2D texture2D, int xDrawingsCount, int yDrawingsCount)
    {
        playerTexture2D = texture2D;
        this.xDrawingsCount = xDrawingsCount;
        this.yDrawingsCount = yDrawingsCount;
        animations = new Dictionary<string, Animation>();
        AddAnimation("standing", 1);
    }
    
    public void Update()
    {
        animations["standing"].Update();
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(playerTexture2D, new Vector2(10,10), animations["standing"].CurrentFrame.SourceRectangle, Color.White, 0,  Vector2.Zero, 2f,SpriteEffects.None, 0);
    }
    public void AddAnimation(string animationName, int spriteRowNum)
    {
        animations.Add(animationName, new Animation());
        animations[animationName].ExtractAnimationFramesRow(playerTexture2D, xDrawingsCount, yDrawingsCount, spriteRowNum);
    }
}
