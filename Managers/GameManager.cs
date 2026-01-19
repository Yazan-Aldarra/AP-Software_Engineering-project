using Gum.Forms.Controls;
using Gum.Wireframe;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;
using System;

namespace project;

public class GameManager
{


    private IGame game;
    public static Matrix TRANSLATION;
    private GraphicsDevice graphicsDevice;
    private ContentManager content;

    private Map map;
    
    private Player player;
    private Vector2 startPosition = new Vector2(50, 200);
    private GameObject playerDecorator;
    private Texture2D playerText;
    private Texture2D emptyText;

    private List<Enemy> enemies;
    private Texture2D enemyText;

    private StackPanel mainPanel;
    private GumService Gum => GumService.Default;
    private Dictionary<ButtonType, Button> menuButtons;

    public GameManager(IGame game, ContentManager content, GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice;
        this.content = content;

        enemies = new List<Enemy>();
        emptyText = new Texture2D(graphicsDevice, 1, 1);
        emptyText.SetData(new[] { Color.White });

        playerText = content.Load<Texture2D>("Player_Sprite");
        enemyText = content.Load<Texture2D>("Skeleton_Sprite");

        this.game = game;

        InitializeGameObjects();
    }

    private void CalculateTranslation(GraphicsDevice graphicsDevice)
    {
        var dx = (graphicsDevice.Viewport.Width / 2) - (player.Collider.X + player.Collider.Width);
        dx = MathHelper.Clamp(dx, -map.MapSize.X + graphicsDevice.Viewport.Width + (map.BlockSize.X / 2), map.BlockSize.X / 2);

        var dy = (graphicsDevice.Viewport.Height / 2) - (player.Position.Y + player.Collider.Height);
        dy = MathHelper.Clamp(dy, -map.MapSize.Y + graphicsDevice.Viewport.Height + (map.BlockSize.Y / 2), map.BlockSize.Y / 2);

        TRANSLATION = Matrix.CreateTranslation(dx, dy, 0f);
    }

    public void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
    {
        Gum.Update(gameTime);

        if (game.isGameStarted == true && game.isGamePaused == false && game.isGameOver == false)
        {
            playerDecorator.Update(gameTime);
            map.Update(gameTime, graphicsDevice);
            CalculateTranslation(graphicsDevice);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {

        Gum.Draw();

        if (game.isGameStarted == true && game.isGamePaused == false && game.isGameOver == false)
        {

            map.Draw(spriteBatch);
            playerDecorator.Draw(spriteBatch);
        }

    }

    private void InitializeGameObjects()
    {
        map = new Map(content, graphicsDevice, emptyText);

        player = new Player(playerText, new KeyboardReader(), initialPos: startPosition, xDrawingsCount: 8, yDrawingsCount: 8, colliderTexture2d: emptyText);
        (player as Player).CropAnimationFrames(0, 30);
        (player as Player).SetColliderSize(30, 35);

        playerDecorator = new BaseWeaponDecorator<Player>(player as Player, emptyText)
        { Scale = 3, Texture2D = emptyText };

        playerDecorator.SetColliderSize(25, 15);

        map.AddBlocksAsTrigger(player);

        InitGum(game);
        InitStartMenu();
    }
    private void InitEndMenu()
    {

    }
    private void InitStartMenu()
    {
        mainPanel = new StackPanel();

        mainPanel.AddToRoot();
        mainPanel.Spacing = 10f;
        mainPanel.Anchor(Anchor.Center);


        menuButtons = new Dictionary<ButtonType, Button>()
        {
            {ButtonType.START_GAME, new Button()},
            {ButtonType.RESUME, new Button()},
            {ButtonType.QUIT, new Button()},
            {ButtonType.RESTART,  new Button()}
        };


        foreach (var keyPair in menuButtons)
        {
            var key = keyPair.Key;
            var btn = keyPair.Value;

            // Make from ButtonType the text for the btn
            var str = string.Join(" ", key.ToString().Split("_"));
            btn.Text = $"{str[0].ToString().ToUpper()}{str.Substring(1).ToLower()}";

            mainPanel.AddChild(btn);

            if (key == ButtonType.START_GAME)
            {
                btn.Click += (_, _) =>
                {
                    mainPanel.IsEnabled = false;
                    mainPanel.IsVisible = false;

                    btn.IsEnabled = false;
                    btn.IsVisible = false;

                    menuButtons[ButtonType.RESUME].IsEnabled = true;
                    menuButtons[ButtonType.RESUME].IsVisible = true;

                    game.isGameStarted = true;
                };
            }
            else if (key == ButtonType.RESUME)
            {
                btn.IsVisible = false;
                btn.IsEnabled = false;

                btn.Click += (_, _) =>
                {
                    mainPanel.IsEnabled = false;
                    mainPanel.IsVisible = false;

                    game.isGamePaused = false;
                };
            }
            else if (key == ButtonType.QUIT)
            {
                btn.Click += (_, _) => game.Exit();
            }
            else if (key == ButtonType.RESTART)
            {
                btn.Click += (_, _) => RestartGame();
            }
        }

    }
    public void PauseGame()
    {
        mainPanel.IsVisible = true;
        mainPanel.IsEnabled = true;
        game.isGamePaused = true;
    }
    public void InitGum(IGame game)
    {
        Gum.Initialize(game as Game);
    }
    public void RestartGame()
    {
        player.Position = startPosition;
        player.Health = 100f;

        enemies?.Clear();
        SpawnEnemies(3);

        game.isGameOver = false;
    }
    private void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // var enemy =  new Enemy(enemyText, new VisionReader(), );
            // enemies.Add(enemy);
        }
    }
}
