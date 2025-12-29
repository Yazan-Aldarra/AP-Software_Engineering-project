using System;
using Microsoft.Xna.Framework.Graphics;

namespace project;

public enum BlockType{ SIMPLE, TRAP }
public static class BlockFactory
{
    public static Block CreateBlock(
     BlockType type, float x, float y, GraphicsDevice graphics)
    {
        Block newBlock = null;
        
        return type switch
        {
            // BlockType.SIMPLE => new SimpleBlock(),
            // BlockType.TRAP => new TrapBlock(),

        };
    }
}
