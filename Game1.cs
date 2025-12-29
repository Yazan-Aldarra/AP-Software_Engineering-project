using System;
using System.Collections.Generic;
using System.Net.Http;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace project;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private GameObject player;
    private Texture2D playerTexture;
    private Texture2D text;
    private List<Block> blocks = new List<Block>();
    private Map map;
    private GameObject playerDecorator;
    private GameManager gameManager;
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
        // text = new Texture2D(GraphicsDevice, 1, 1);
        // text.SetData(new[] { Color.White });

        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // playerTexture = Content.Load<Texture2D>("Player_Sprite");
        InitializeGameObjects();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // playerDecorator.Update(gameTime);
        gameManager.Update(gameTime, GraphicsDevice);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin(transformMatrix: GameManager.TRANSLATION);

        // player.Draw(_spriteBatch);

        // blocks.ForEach(b => b.Draw(_spriteBatch));
        // map.Draw(_spriteBatch);

        gameManager.Draw(_spriteBatch);

        _spriteBatch.End();
        base.Draw(gameTime);
    }
    private void InitializeGameObjects()
    {
        gameManager = new GameManager(Content,GraphicsDevice);
    }

}
