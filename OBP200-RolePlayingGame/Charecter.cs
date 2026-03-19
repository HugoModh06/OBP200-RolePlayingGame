namespace OBP200_RolePlayingGame;
//Abstrakt klass som både player och enemy ärver från då båda har liknande egenskaper
public abstract class Character
{
    protected int CurrentHealth;
    protected int MaxHealth;
    protected string Name;
    protected int Attack;
    public int Defence {get; protected set;}

    public abstract void ShowStatus();
    public abstract int CalculateDamage(Character target);
    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
    }
    
    public bool CheckIfDead()
    {
        if (CurrentHealth <= 0)
        {
            return true;
        }
        
        return false;
    }
}

public interface IAttackCapable
{
    int CalculateDamage(Character target);
}