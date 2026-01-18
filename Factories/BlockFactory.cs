using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project;

public enum BlockType { NONE, SIMPLE, TRAP }
public static class BlockFactory
{

    private static readonly Dictionary<BlockType, Type> BlockTypes = new()
    {
        [BlockType.SIMPLE] = typeof(SimpleBlock),
        [BlockType.TRAP] = typeof(TrapBlock),
    };

    static BlockFactory()
    {
    }
    public static Block CreateBlock(BlockType type,
            Texture2D texture2D,
			Vector2? initialPos = null,
			int width = 10,
			int height = 10,
			float scale = 1f,
            Animation animation = null,
			int xDrawingsCount = 1,
			int yDrawingsCount = 1,
			Texture2D colliderTexture2d = null)
    {
        if (!BlockTypes.TryGetValue(type, out var blockType))
            throw new ArgumentException($"Unknown block type {type}");

        var block = (Block) Activator.CreateInstance( 
                blockType,
                texture2D,
                initialPos,
                width,
                height,
                scale,
                animation,
                xDrawingsCount,
                yDrawingsCount,
                colliderTexture2d
        );

        if (block is TrapBlock trapBlock)
        {
            // stuff
        }
        
        return block;
    }
}
