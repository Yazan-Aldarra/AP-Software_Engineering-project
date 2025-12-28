using System;
using System.Collections.Generic;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace project;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private IGameObject player;
    private Texture2D playerTexture;
    private Texture2D text;
    int[,] gameboard = new int[,] {
        { 1,1,1,1,1,1,1,1 },
        { 0,0,1,1,0,1,1,1 },
        { 1,0,0,0,0,0,0,1 },
        { 1,1,1,1,1,1,0,1 },
        { 1,0,0,0,0,0,0,2 },
        { 1,0,1,1,1,1,1,2 },
        { 1,0,0,0,0,0,0,0 },
        { 1,1,1,1,1,1,1,1 }
    };
    private List<Block> block = new List<Block>();
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
        text = new Texture2D(GraphicsDevice, 1, 1);
        text.SetData(new[] { Color.White });

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
        player = new Player2(playerTexture, new KeyboardReader(), 8, 8, text);
        (player as Player2).CropAnimationFrames(0, 30);
        (player as Player2).SetColliderSize(40, 35);


        block.Add(new SimpleBlock(text, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, 10), new Vector2(0, GraphicsDevice.Viewport.Height - 10)));
        block.Add(new SimpleBlock(text, new Rectangle(0, 0, 100, 30), new Vector2(300, 200)));
        block.Add(new SimpleBlock(text, new Rectangle(0, 0, 100, 100), new Vector2(400, GraphicsDevice.Viewport.Height - 100 - 10)));

        block.ForEach(b => (player as Player2).AddColliderTriggers(b));

        player = new BaseWeaponDecorator<Player2>(player as Player2, text)
        { Scale = 3, Texture2D = text };

        (player as EntityDecorator<Player2>).SetColliderSize(30,20);
    }
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();

        player.Draw(_spriteBatch);

        block.ForEach(b => b.Draw(_spriteBatch));

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
