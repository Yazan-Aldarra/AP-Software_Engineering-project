using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace project;

public class GameManager
{

    private Map map;
    private Entity player;
    private GameObject playerDecorator;
    private Texture2D playerText;
    private Texture2D emptyText;
    public static Matrix TRANSLATION;
    private GraphicsDevice graphicsDevice;
    private ContentManager content;
    public GameManager(ContentManager content, GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice;
        this.content = content;

        emptyText = new Texture2D(graphicsDevice, 1, 1);
        emptyText.SetData(new[] { Color.White });

        playerText = content.Load<Texture2D>("Player_Sprite");

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
        playerDecorator.Update(gameTime);
        map.Update(gameTime, graphicsDevice);
        CalculateTranslation(graphicsDevice);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        map.Draw(spriteBatch);
        playerDecorator.Draw(spriteBatch);

    }

    private void InitializeGameObjects()
    {
        map = new Map(content, graphicsDevice, emptyText);

        player = new Player(playerText, new KeyboardReader(), initialPos: new Vector2(50, 200), xDrawingsCount: 8, yDrawingsCount: 8, colliderTexture2d: emptyText);
        (player as Player).CropAnimationFrames(0, 30);
        (player as Player).SetColliderSize(30, 35);

        // mapBoundaries.ForEach(b => player.AddColliderTriggers(b));

        playerDecorator = new BaseWeaponDecorator<Player>(player as Player, emptyText)
        { Scale = 3, Texture2D = emptyText };

        playerDecorator.SetColliderSize(25, 15);

        map.AddBlocksAsTrigger(player);
    }
}
