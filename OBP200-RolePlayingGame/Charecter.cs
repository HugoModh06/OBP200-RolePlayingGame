namespace OBP200_RolePlayingGame;

public abstract class Character
{
    protected int CurrentHealth;
    protected int MaxHealth;
    public int Attack { get; protected set; }
    public int Defence{get; protected set;}

    public abstract void ShowStatus();
    public abstract int AttackCalculation(Character target);
    public abstract void TakeDamage(int damage);
    public abstract bool CheckIfDead();
}