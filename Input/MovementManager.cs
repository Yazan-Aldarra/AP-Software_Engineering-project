using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project;

public class MovementManager
{
    public void Move(IMovable movable)
    {
        var direction = movable.FutureDirection;

        if (movable.InputReader.IsDestinationInput)
        {
            direction -= movable.Position;
            direction.Normalize();
        }
        var distance = direction * movable.Speed;
        var futurePosition = movable.Position + distance;

        movable.Position = futurePosition;

        ResetBlockedSide(movable);
    }
    // returns true if the point is reached, and does not jump
    public bool JumpTill(IMovable movable, float point)
    {

        var isReached = false;
        var futureY = movable.Position.Y - movable.JumpingSpeed;
        var futureX = movable.Position.X;

        if (futureY <= point) { futureY = point; isReached = true; }

        movable.Position = new Vector2(futureX, futureY);

        ResetBlockedSide(movable);
        return isReached;
    }
    public void MoveInAir(IMovable movable)
    {

        var futureX = movable.Position.X + movable.FutureDirection.X * movable.AirMoveSpeed;
        var futureY = movable.Position.Y;

        movable.Position = new Vector2(futureX, futureY);
        ResetBlockedSide(movable);
    }
    private void ResetBlockedSide(IMovable movable)
    {
        movable.BlockedSide = new List<Direction>();
    }
    private void ApplyBlockedSideToFutureDirection(IMovable movable, List<Collision> collisions)
    {
        var newX = movable.FutureDirection.X;
        if (movable.BlockedSide != null && movable.BlockedSide.Contains(Direction.LEFT))
        {
            newX = (int)movable.FutureDirection.X == 0 ? 0 : 1;
        }
        else if (movable.BlockedSide != null && movable.BlockedSide.Contains(Direction.RIGHT))
        {
            newX = (int)movable.FutureDirection.X == 0 ? 0 : -1;
        }
        movable.FutureDirection = new Vector2(newX, movable.FutureDirection.Y);
    }
    public void SetOverlappedObjectBack<T>(T movable, List<Collision> collisions) where T : IMovable, ICollidable
    {
        var horCol = collisions
               .Where(c => c.Direction == Direction.LEFT || c.Direction == Direction.RIGHT)
               .OrderBy(c => c.OverlapAmount)
               .FirstOrDefault();

        var verCol = collisions
            .Where(c => c.Direction == Direction.UP || c.Direction == Direction.DOWN)
            .OrderBy(c => c.OverlapAmount)
            .FirstOrDefault();

        float dx = 0f;
        float dy = 0f;

        if (horCol != null)
            dx = (horCol.Direction == Direction.LEFT) ? +horCol.OverlapAmount : -horCol.OverlapAmount;

        if (verCol != null)
            // dy = (verCol.Direction == Direction.UP) ? +verCol.OverlapAmount : -verCol.OverlapAmount;
            dy = (verCol.Direction == Direction.UP) ? +verCol.OverlapAmount : -verCol.OverlapAmount + 1;

        movable.Position = new Vector2(movable.Position.X + dx, movable.Position.Y + dy);
    }
}
