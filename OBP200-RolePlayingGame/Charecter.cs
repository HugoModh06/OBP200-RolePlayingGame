namespace OBP200_RolePlayingGame;
//Abstrakt klass som både player och enemy ärver från då båda har vissa liknande egenskaper
public abstract class GameCharacter
{
    protected int CurrentHealth;
    protected int MaxHealth;
    protected string Name;
    protected int Attack;
    public int Defence {get; protected set;}

    public virtual void ShowStatus()
    {
        
    }
    public virtual int CalculateDamage(int targetDefence)
    {
        return 0;
    }
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
