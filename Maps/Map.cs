using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using project;


public class Map
{
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

        // 0 = void, 1 = solid, 2 = trap
        gameboard = new int[,] {
            // Sky (3 rows)
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},

            // Main (7 rows) - platforms + traps + gaps, now with central walled area
            // Row 3: upper platform with new left/right walls
            {0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,1,1,1,0,0,1,1,1,0,0,1,1,1,},

            // Row 4: islands + extended walls
            {0,0,0,0,0,0,1,0,0,0,0,0,1,1,1,1,0,0,1,1,0,1,1,0,0,0,1,1,0,0,},

            // Row 5: mid-height supports
            {0,0,0,0,0,0,1,0,0,0,0,1,1,0,0,0,0,0,1,1,0,0,0,1,1,0,0,0,1,1,},

            // Row 6: early ramp/ledge + safer high road start
            {1,1,1,1,1,1,1,0,1,0,1,1,0,0,0,1,1,1,0,0,0,0,1,1,1,0,0,0,1,1,},

            // Row 7: trap pits under common landing zones
            {0,0,0,0,0,0,0,0,0,0,2,2,2,0,0,0,2,2,2,0,0,0,2,2,2,0,0,0,2,2,},

            // Row 8: mostly empty for fall-threat + rescue ledges
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},

            // Row 9: ground with gaps + trap-lined death lanes
            {1,1,1,1,1,0,0,0,1,1,1,1,1,0,0,0,1,1,1,1,1,0,0,0,1,1,1,1,1,0,},

            // Void fall (3 rows)
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
        };
        mapBlockSize = new Point(gameboard.GetLength(1), gameboard.GetLength(0));
        this.emptyText = emptyText;

        blocks = new Block[mapBlockSize.Y, mapBlockSize.X];

        var textures = new Dictionary<BlockType, List<Texture2D>>();
        textures.Add(BlockType.SIMPLE, new List<Texture2D>());
        textures.Add(BlockType.TRAP, new List<Texture2D>());

        for (int i = 1; i <= 2; i++)
            textures[BlockType.SIMPLE].Add(content.Load<Texture2D>($"blocks\\Tile_0{i}"));

        textures[BlockType.TRAP].Add(content.Load<Texture2D>($"blocks\\Trap_Tile_01"));

        CreateBlocks(textures, emptyText);
        CreateMapBoundaries(graphicsDevice);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (blocks == null)
            return;
        for (int y = 0; y < mapBlockSize.Y; y++)
        {
            for (int x = 0; x < mapBlockSize.X; x++) blocks[y, x]?.Draw(spriteBatch);
        }
    }
    public void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
    {
        foreach (var block in blocks)
        {
            if (block != null)
                block.Update(gameTime);
        }
    }
    private void CreateBlocks(Dictionary<BlockType, List<Texture2D>> textures, Texture2D text = null)
    {

        var scale = 1f;
        var random = new Random();

        BlockSize = new Point(textures[BlockType.SIMPLE][0].Width, textures[BlockType.SIMPLE][0].Height);
        MapSize = new Point(BlockSize.X * (int)scale * mapBlockSize.X, BlockSize.Y * (int)scale * mapBlockSize.Y);

        for (int y = 0; y < mapBlockSize.Y; y++)
        {
            for (int x = 0; x < mapBlockSize.X; x++)
            {
                if (gameboard[y, x] > 0)
                {

                    var type = (BlockType)gameboard[y, x];
                    int textureIndex = random.Next(0, textures[type].Count);
                    int yDrawingsCount = 1;
                    int xDrawingsCount = 1;

                    var xPos = x * BlockSize.X;
                    var yPos = y * BlockSize.Y;

                    var block = BlockFactory.CreateBlock(
                        type,
                        textures[type][textureIndex],
                        initialPos: new Vector2(xPos * scale, yPos * scale),
                        scale: scale, colliderTexture2d: null,
                        width: BlockSize.X, height: BlockSize.Y,
                        xDrawingsCount: xDrawingsCount, yDrawingsCount: yDrawingsCount
                    );
                    if (type == BlockType.TRAP)
                    {
                        var col = block.Collider;
                        block.SetColliderSize(col.Width - (int)(col.Width * .4), col.Height - (int)(col.Height * .4));
                    }
                    block.AddAnimation(AnimationType.IDLE, 1);

                    blocks[y, x] = block;

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
                if (blocks[y, x] == null)
                    continue;
                gameObject.AddColliderTriggers(blocks[y, x]);
            }
        }
        mapBoundaries.ForEach(b => gameObject.AddColliderTriggers(b));
    }
    private void CreateMapBoundaries(GraphicsDevice graphicsDevice)
    {
        var mapSize = MapSize;
        var thickness = 1;
        // left wall
        mapBoundaries.Add(BlockFactory.CreateBlock(BlockType.SIMPLE, emptyText, width: thickness, height: mapSize.Y, colliderTexture2d: emptyText));
        // Ceiling
        mapBoundaries.Add(BlockFactory.CreateBlock(BlockType.SIMPLE, emptyText, width: mapSize.X, height: thickness, colliderTexture2d: emptyText));
        // right wall
        mapBoundaries.Add(BlockFactory.CreateBlock(BlockType.SIMPLE, emptyText, initialPos: new Vector2(mapSize.X, 0), width: thickness, height: mapSize.Y, colliderTexture2d: emptyText));
        // Floor
        mapBoundaries.Add(BlockFactory.CreateBlock(BlockType.TRAP, emptyText, initialPos: new Vector2(0, mapSize.Y + 50f), width: mapSize.X, height: thickness, colliderTexture2d: emptyText));
    }
}