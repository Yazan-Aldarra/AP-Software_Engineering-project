using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace project;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Player player;
    private Texture2D playerTexture;
    private Texture2D floorText;
    private Platform floor;
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        floorText = new Texture2D(GraphicsDevice, 1,1);
        floorText.SetData( new[] { Color.White });

        _spriteBatch = new SpriteBatch(GraphicsDevice);

        playerTexture = Content.Load<Texture2D>("Player_Sprite");
        InitializeGameObjects();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        player.Update(gameTime);
        base.Update(gameTime);
    }

    private void InitializeGameObjects()
    {
        player = new Player(playerTexture, 8, 6, new KeyboardReader(), floorText);
        player.CropAnimationFrames(60, 30);

        var floorRec = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, 10); 
        floor = new Platform(floorText, floorRec, new Vector2(0, GraphicsDevice.Viewport.Height - floorRec.Height));
        player.AddColliderTriggers(floor);
    }
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();

        player.Draw(_spriteBatch);
        floor.Draw(_spriteBatch);

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
