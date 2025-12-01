using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace project;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D playerTexture;
    private int MoveWith;
    private Rectangle PlayerRectangle;
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        PlayerRectangle = new Rectangle(0, 0, 96, 64);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        playerTexture = Content.Load<Texture2D>("Player_Sprite");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        _spriteBatch.Draw(playerTexture, new Vector2(0,GraphicsDevice.Viewport.Height-PlayerRectangle.Height*2), PlayerRectangle, Color.White, 0,  Vector2.Zero, 2f,SpriteEffects.None, 0);

        if (PlayerRectangle.X >= PlayerRectangle.Width * 7)
        {
            PlayerRectangle.X = 0;
        } else PlayerRectangle.X += PlayerRectangle.Width;
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
