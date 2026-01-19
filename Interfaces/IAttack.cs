
namespace project;

public interface IAttack
{
    public float Damage { get; set; }
    public bool isActive { get; set; }
    public void UseAttack(IHasHealth hasHealth);
}
