using System;
using System.Security.Cryptography.X509Certificates;

namespace project;

public interface IHasHealth
{
    public float Health { get; }
    public void DecreaseHealth(float value);
}
