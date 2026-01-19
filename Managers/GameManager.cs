using Gum.Forms.Controls;
using Gum.Wireframe;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;
using System;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;
using Gum.Forms.DefaultVisuals;
using System.Linq;

namespace project;

public enum ButtonType { START_GAME, QUIT, RESTART, RESUME }
public enum MenuLabelsType { YOU_WON, GAME_OVER }
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

    private List<GameObject> enemies;
    private Texture2D enemyText;

    private StackPanel mainPanel;
    // private GumService gum;
    private Dictionary<ButtonType, Button> menuButtons;
    private Dictionary<MenuLabelsType, Label> menuTexts;

    public GameManager(IGame game, ContentManager content, GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice;
        this.content = content;

        enemies = new List<GameObject>();
        emptyText = new Texture2D(graphicsDevice, 1, 1);
        emptyText.SetData(new[] { Color.White });

        playerText = content.Load<Texture2D>("Player_Sprite");
        enemyText = content.Load<Texture2D>("Skeleton_Sprite");

        this.game = game;
        // this.gum = gum;
        InitGameObjects();
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


        if (game.isGameStarted == true && game.isGamePaused == false && game.isGameOver == false && game.isGameWon == false)
        {
            playerDecorator.Update(gameTime);

            if (player.State is DeadState)
                GameOver();

            UpdateEnemies(gameTime);

            ClearDeadEnemies();
            CheckIsGameWon();

            map.Update(gameTime, graphicsDevice);
            CalculateTranslation(graphicsDevice);
        }
    }
    private void CheckIsGameWon()
    {
        if (enemies.Count <= 0)
            WinGame();
    }
    private void ClearDeadEnemies()
    {
        enemies = enemies.Where(e =>
        {
            Entity obj = e as Entity;
            if (e is EntityDecorator<Enemy> ed)
                obj = ed.GameObject;
            else
                obj = e as Entity;
            return !(obj.State is DeadState);
        }
        ).ToList();
    }
    public void Draw(SpriteBatch spriteBatch)
    {


        if (game.isGameStarted == true && game.isGamePaused == false && game.isGameOver == false && game.isGameWon == false)
        {

            DrawEnemies(spriteBatch);
            map.Draw(spriteBatch);
            playerDecorator.Draw(spriteBatch);
        }

    }

    private void InitGameObjects()
    {
        map = new Map(content, graphicsDevice, emptyText);

        player = new Player(playerText, new KeyboardReader(), initialPos: startPosition, xDrawingsCount: 8, yDrawingsCount: 8, colliderTexture2d: null);
        (player as Player).CropAnimationFrames(0, 30);
        (player as Player).SetColliderSize(30, 35);

        playerDecorator = new BaseWeaponDecorator<Player>(player as Player, null)
        { Scale = 3, Texture2D = emptyText };
        (playerDecorator as BaseWeaponDecorator<Player>).InitialPos = new Vector2(player.Collider.Width, 20f);

        playerDecorator.SetColliderSize(25, 15);

        map.AddBlocksAsTrigger(player);

        SpawnEnemies(2);

        InitMenu();
    }
    private void InitMenu()
    {
        mainPanel = new StackPanel();

        mainPanel.AddToRoot();
        mainPanel.Spacing = 10f;
        mainPanel.Anchor(Anchor.Center);


        menuButtons = new Dictionary<ButtonType, Button>()
        {
            {ButtonType.START_GAME, new Button()},
            {ButtonType.RESUME, new Button()},
            {ButtonType.RESTART,  new Button()},
            {ButtonType.QUIT, new Button()},
        };
        menuTexts = new Dictionary<MenuLabelsType, Label>()
        {
            {MenuLabelsType.YOU_WON, new Label()},
            {MenuLabelsType.GAME_OVER, new Label()},
        };

        foreach (var keyPair in menuTexts)
        {

            var key = keyPair.Key;
            var label = keyPair.Value;
            // Make from MenuLabelsType the text for the label
            var str = string.Join(" ", key.ToString().Split("_"));
            label.Text = $"{str[0].ToString().ToUpper()}{str.Substring(1).ToLower()}";

            label.Anchor(Anchor.Center);

            mainPanel.AddChild(label);
            SetFrameworkEl(label, false);
        }
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

                    SetFrameworkEl(mainPanel, false);
                    SetFrameworkEl(btn, false);

                    SetFrameworkEl(menuButtons[ButtonType.RESUME], true);
                    SetFrameworkEl(menuButtons[ButtonType.RESTART], true);

                    game.isGameStarted = true;
                };
            }
            else if (key == ButtonType.RESUME)
            {
                SetFrameworkEl(btn, false);

                btn.Click += (_, _) =>
                {
                    SetFrameworkEl(mainPanel, false);
                    game.isGamePaused = false;
                };
            }
            else if (key == ButtonType.QUIT)
            {
                btn.Click += (_, _) => game.Exit();
            }
            else if (key == ButtonType.RESTART)
            {
                SetFrameworkEl(btn, false);
                btn.Click += (_, _) => game.RestartGame();
            }
        }

    }
    public void PauseGame()
    {
        SetFrameworkEl(mainPanel, true);
        game.isGamePaused = true;
    }
    private void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var pos = 300f;
            var reader = new VisionReader<Player>();
            var enemy = new Enemy(enemyText, reader, initialPos: new Vector2(pos * (i + 1), 240), xDrawingsCount: 8, yDrawingsCount: 6, colliderTexture2d: null, scale: 3);

            enemy.SetColliderSize(20, 35);
            enemy.CropAnimationFrames(0, 30);

            enemy.AddColliderTriggers(playerDecorator);
            player.AddColliderTriggers(enemy);
            var enemyDec = new BaseWeaponDecorator<Enemy>(enemy, null)
            { Scale = 3, Texture2D = emptyText };

            enemyDec.SetColliderSize(20, 45);
            enemyDec.InitialPos = new Vector2(-enemyDec.Collider.Width, 0);
            player.AddColliderTriggers(enemyDec);

            map.AddBlocksAsTrigger(enemy);
            reader.Entity = player;
            reader.Owner = enemy;
            enemies.Add(enemyDec);
        }
    }
    private void WinGame()
    {
        game.isGameWon = true;

        SetFrameworkEl(mainPanel, true);
        DisableAllFrameworkEl();

        EnableEndGameMenu();
        // SetFrameworkEl(menuTexts[MenuLabelsType.YOU_WON], true);
    }
    private void GameOver()
    {
        game.isGameOver = true;

        SetFrameworkEl(mainPanel, true);
        DisableAllFrameworkEl();

        EnableEndGameMenu();
        SetFrameworkEl(menuTexts[MenuLabelsType.GAME_OVER], true);
    }
    private void EnableEndGameMenu()
    {
        SetFrameworkEl(menuButtons[ButtonType.RESTART], true);
        SetFrameworkEl(menuButtons[ButtonType.QUIT], true);
    }
    private void DisableAllFrameworkEl()
    {
        foreach (var item in mainPanel.Children)
        {
            SetFrameworkEl(item, false);
        }

    }
    private void DrawEnemies(SpriteBatch spriteBatch) => enemies.ForEach(e => e.Draw(spriteBatch));
    private void UpdateEnemies(GameTime gameTime) => enemies.ForEach(e => e.Update(gameTime));

    private void SetFrameworkEl(FrameworkElement frameworkElement, bool state)
    {
        if (state)
        {
            frameworkElement.IsVisible = true;
            frameworkElement.IsEnabled = true;
        }
        else
        {
            frameworkElement.IsVisible = false;
            frameworkElement.IsEnabled = false;
        }
    }
}
