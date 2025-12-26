using System;

namespace project;

public interface IAttacker
{
    public float Damage { get; set; }
    public AttackType FutureAttack { get; set; }
}
