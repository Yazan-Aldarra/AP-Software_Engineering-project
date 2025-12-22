using System.Collections.Generic;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace project;

public class Player : IGameObject, IMovable
{
    private Texture2D playerTexture2D;
    private Rectangle playerRectangle;
    private int xDrawingsCount;
    private int yDrawingsCount;
    private Vector2 position;
    public Vector2 Position { get => position; set => position = value; }
    private Vector2 speed;
    public Vector2 Speed { get => speed; set => speed = value; }
    MouseState state;
    Vector2 mouseVector;
    private IInputReader inputReader;
    public IInputReader InputReader { get => inputReader; set => inputReader = value; }
    private Dictionary<string, Animation> animations;
    private MovementManager movementManager;
    

    public Player(Texture2D texture2D, int xDrawingsCount, int yDrawingsCount, IInputReader inputReader)
    {
        playerTexture2D = texture2D;
        this.xDrawingsCount = xDrawingsCount;
        this.yDrawingsCount = yDrawingsCount;
        playerRectangle.Width = texture2D.Width / xDrawingsCount;
        playerRectangle.Height = texture2D.Height / yDrawingsCount;
        animations = new Dictionary<string, Animation>();
        AddAnimation("standing", 1);
        Position = new Vector2(0, 0);
        Speed = new Vector2(3, 0);
        this.InputReader = inputReader;
        movementManager = new MovementManager();
    }

    public void Update(GameTime gameTime)
    {

        animations["standing"].Update(gameTime);
        Move();
    }
    public void Draw(SpriteBatch spriteBatch)
    {

        spriteBatch.Draw(playerTexture2D, position, animations["standing"].CurrentFrame.SourceRectangle, Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
    }
    public void AddAnimation(string animationName, int spriteRowNum)
    {
        animations.Add(animationName, new Animation());
        animations[animationName].ExtractAnimationFramesRow(playerTexture2D, xDrawingsCount, yDrawingsCount, spriteRowNum);
    }
    private void Move()
    {

        // Vector2 direction = inputReader.ReadInput();
        // direction *= speed;
        // 
        // position += direction;
        movementManager.Move(this);
    }
    public void CropAnimationFrames(int verticalCropping, int horizontalCropping)
    {
        foreach (var animation in animations.Values)
            animation.CropAnimationFrames(verticalCropping, horizontalCropping);
    }
}
