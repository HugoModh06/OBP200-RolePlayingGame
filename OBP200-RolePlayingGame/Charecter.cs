namespace OBP200_RolePlayingGame;
//Abstrakt klass som både player och enemy ärver från då båda har liknande egenskaper
public abstract class Character
{
    protected int CurrentHealth;
    protected int MaxHealth;
    public int Attack { get; protected set; }
    public int Defence{get; protected set;}

    public abstract void ShowStatus();
    public abstract int AttackCalculation(Character target);
    public abstract void TakeDamage(int damage);
    
    
    public bool CheckIfDead()
    {
        if (CurrentHealth <= 0)
        {
            return true;
        }
        
        return false;
    }
}