using System;
using Microsoft.Xna.Framework;

namespace project;

public class VisionReader : IInputReader
{
    public bool IsDestinationInput => true;

    public AttackType ReadAttack()
    {
        throw new NotImplementedException();
    }

    public Vector2 ReadInput()
    {
        throw new NotImplementedException();
    }
}
