using System.Security.Cryptography.X509Certificates;
using Interfaces;
using Microsoft.Xna.Framework;

namespace project;

public enum Direction { LEFT, UP, RIGHT, DOWN, LEFT_TOP, RIGHT_TOP, RIGHT_DOWN, LEFT_DOWN, NONE }
public static class Utils
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="direction"></param>
    /// <returns>8 types of <see cref="Direction"/></returns>
    public static Direction GetDetailedDirection(Vector2 direction)
    {
        var x = direction.X;
        var y = direction.Y;

        return (x, y) switch
        {
            (0, 0) => Direction.NONE,

            ( < 0, 0) => Direction.LEFT,
            ( > 0, 0) => Direction.RIGHT,
            (0, < 0) => Direction.UP,
            (0, > 0) => Direction.DOWN,

            ( < 0, < 0) => Direction.LEFT_TOP,
            ( > 0, < 0) => Direction.RIGHT_TOP,
            ( > 0, > 0) => Direction.RIGHT_DOWN,
            ( < 0, > 0) => Direction.LEFT_DOWN,

            _ => Direction.NONE
        };
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="direction"></param>
    /// <returns>4 types of Directions or Direction.None<see cref="Direction"/></returns>
    public static Direction GetDirection(Vector2 direction)
    {
        var x = direction.X;
        var y = direction.Y;

        return (x, y) switch
        {
            (0, 0) => Direction.NONE,

            ( < 0, 0) => Direction.LEFT,
            ( > 0, 0) => Direction.RIGHT,
            (0, < 0) => Direction.UP,
            (0, > 0) => Direction.DOWN,

            ( < 0, < 0) => Direction.UP,
            ( > 0, < 0) => Direction.UP,
            ( > 0, > 0) => Direction.DOWN,
            ( < 0, > 0) => Direction.DOWN,

            _ => Direction.NONE
        };
    }
    public static Vector2 GetCenteredColliderPosition<T>(T collider, Rectangle ofRectangle) where T : IGameObject, ICollidable
    {
        var gameObjectPos = collider.GetGameObjectPos();
        var newX = (int)(gameObjectPos.X + ofRectangle.Width * collider.Scale * 0.5 - collider.Collider.Width * 0.5);
        var newY = (int)(gameObjectPos.Y + ofRectangle.Height * collider.Scale * 0.5 - collider.Collider.Height * 0.5);

        return new Vector2(newX, newY);
    }
    public static Vector2 GetCenteredColliderPosition(Rectangle collider, Rectangle ofRectangle)
    {
        var newX = ofRectangle.X + ofRectangle.Width*.5f - collider.Width*.5f;
        var newY = ofRectangle.Y + ofRectangle.Height*.5f - collider.Height*.5f;

        return new Vector2(newX, newY);
    }
}

