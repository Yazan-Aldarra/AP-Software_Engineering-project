using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using project;


public class Map
{
    // int[,] gameboard = new int[,] {
    //     { 1,1,1,1,1,1,1,1 },
    //     { 0,0,1,1,0,1,1,1 },
    //     { 1,0,0,0,0,0,0,1 },
    //     { 1,1,1,1,1,1,0,1 },
    //     { 1,0,0,0,0,0,0,2 },
    //     { 1,0,1,1,1,1,1,2 },
    //     { 1,0,0,0,0,0,0,0 },
    //     { 1,1,1,1,1,1,1,1 }
    // };
    int trap = 0;
    int[,] gameboard;
    private readonly Point mapBlockSize;
    public readonly Block[,] blocks;
    public Point BlockSize { get; private set; }
    public Point MapSize { get; private set; }
    private List<Block> mapBoundaries = new List<Block>();
    private Texture2D emptyText;

    public Map(ContentManager content, GraphicsDevice graphicsDevice, Texture2D emptyText = null)
    {
        gameboard = new int[,] {
            // 0 = void, 1 = solid, trap = trap
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,1,1,0,1,1,0,1,1,0,1,1,0,1,1,0,1},
            {1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,1},
            {1,1,0,1,1,0,1,1,0,1,1,0,1,1,0,1,1,1},

            // Main traversal lane (never more than 2 blocks high)
            {0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0},
            {0,1,1,1,1,0,1,1,1,0,1,1,1,0,1,1,1,0},
            {0,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,1,0},

            // Trap corridor (reachable path)
            {0,0,1,trap,1,0,1,trap,1,0,1,trap,1,0,1,trap,1,0},
            {0,0,1,1,1,0,1,1,1,0,1,1,1,0,1,1,1,0},

            // Upper route with safe steps
            {0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,1,0,0},
            {0,0,1,1,1,0,1,1,1,0,1,1,1,0,1,1,1,0},

            // Final approach
            {0,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,1,0},
            {0,0,0,0,1,1,1,0,1,1,1,0,1,1,1,0,1,0},

            // Void fall
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
        };

        mapBlockSize = new(gameboard.GetLength(0), gameboard.GetLength(1));
        this.emptyText = emptyText;

        blocks = new Block[mapBlockSize.X, mapBlockSize.Y];

        List<Texture2D> textures = new(5);
        for (int i = 1; i < 6; i++) textures.Add(content.Load<Texture2D>($"blocks\\Tile_0{i}"));



        // CreateBlocks(textures, new List<BlockType> { BlockType.SIMPLE });
        CreateBlocks(textures, new List<BlockType> { BlockType.SIMPLE });
        CreateMapBoundaries(graphicsDevice);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (blocks == null)
            return;
        for (int y = 0; y < mapBlockSize.Y; y++)
        {
            for (int x = 0; x < mapBlockSize.X; x++) blocks[x, y]?.Draw(spriteBatch);
        }
    }
    public void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
    {
        UpdateBoundaries();
    }

    private void CreateBlocks(List<Texture2D> textures, List<BlockType> blockTypes, Texture2D text = null)
    {
        var size = 100;
        var scale = 3f;

        Random random = new();

        BlockSize = new(textures[0].Width, textures[0].Height);
        MapSize = new(BlockSize.X * mapBlockSize.X * (int)scale, BlockSize.Y * mapBlockSize.Y * (int)scale);

        for (int y = 0; y < mapBlockSize.Y; y++)
        {
            for (int x = 0; x < mapBlockSize.X; x++)
            {
                if (gameboard[x, y] == 1)
                {

                    int tI = random.Next(0, textures.Count);
                    int btI = random.Next(0, blockTypes.Count);
                    var xP = x * BlockSize.X;
                    var yP = y * BlockSize.Y;

                    // var block =  BlockFactory.CreateBlock(blockTypes[blockTypeN]);

                    var rec = new Rectangle(0, 0, BlockSize.X, BlockSize.Y);

                    var block = new SimpleBlock(
                        textures[tI],
                        initialPos: new Vector2(xP * scale, yP * scale),
                        scale: scale, colliderTexture2d: text
                    );

                    block.AddAnimation(AnimationType.IDLE, 1);
                    block.SetColliderSize(BlockSize.X, BlockSize.Y);

                    blocks[x, y] = block;

                    // System.Console.WriteLine(block.Collider);
                    // System.Console.WriteLine(block.GetGameObjectPos());
                }
            }
        }
    }
    public void AddBlocksAsTrigger<T>(T gameObject) where T : GameObject
    {

        for (int y = 0; y < mapBlockSize.Y; y++)
        {
            for (int x = 0; x < mapBlockSize.X; x++)
            {
                if (blocks[x, y] == null)
                    continue;
                gameObject.AddColliderTriggers(blocks[x, y]);
            }
        }
        mapBoundaries.ForEach(b => gameObject.AddColliderTriggers(b));
    }
    private void CreateMapBoundaries(GraphicsDevice graphicsDevice)
    {
        var v = graphicsDevice.Viewport;
        var thickness = 1;
        // left wall
        mapBoundaries.Add(new SimpleBlock(emptyText, width: thickness, height: v.Height));
        // Ceiling
        mapBoundaries.Add(new SimpleBlock(emptyText, width: v.Width, height: thickness));
        // right wall
        mapBoundaries.Add(new SimpleBlock(emptyText, new Vector2(v.Width, 0), width: thickness, height: v.Height));
        // Floor
        mapBoundaries.Add(new SimpleBlock(emptyText, new Vector2(0, v.Height - thickness), width: v.Width, height: thickness));
    }

    private void UpdateBoundaries()
    {
        var list = mapBoundaries;

        list[0].UpdateColliderPos(0, 0);
        list[1].UpdateColliderPos(0, 0);

        list[2].UpdateColliderPos(MapSize.X, 0);
        list[3].UpdateColliderPos(0, MapSize.Y);
    }
    private void UpdateBoundaries(GraphicsDevice gD)
    {
        var v = gD.Viewport;
        var list = mapBoundaries;

        list[0].UpdateColliderPos(v.X, v.Y);
        list[1].UpdateColliderPos(v.X, v.Y);

        list[2].UpdateColliderPos(v.Width, v.Y);
        list[3].UpdateColliderPos(v.X, v.Height);
    }
}