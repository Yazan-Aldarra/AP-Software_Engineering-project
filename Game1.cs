using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Gum.Forms.Controls;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;

namespace project;

public enum ButtonType { START_GAME, QUIT, RESTART, RESUME }
public class Game1 : Game, IGame
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private GameManager gameManager;

    public bool isGameWon { get; set; } = false;
    public bool isGameOver { get; set; } = false;
    public bool isGameStarted { get; set; } = false;
    public bool isGamePaused { get; set; } = false;

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
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        InitializeGameObjects();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            gameManager.PauseGame();

        gameManager.Update(gameTime, GraphicsDevice);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin(transformMatrix: GameManager.TRANSLATION);

        gameManager.Draw(_spriteBatch);

        _spriteBatch.End();
        base.Draw(gameTime);
    }
    private void InitializeGameObjects()
    {
        gameManager = new GameManager(this, Content, GraphicsDevice);
    }
}
