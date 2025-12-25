using Microsoft.Xna.Framework;

namespace project;

public enum Direction { LEFT, UP, RIGHT, DOWN, LEFT_TOP, RIGHT_TOP, RIGHT_DOWN, LEFT_DOWN, NONE }
public static class Utils
{
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

}

