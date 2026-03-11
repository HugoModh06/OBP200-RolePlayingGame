namespace OBP200_RolePlayingGame;

public abstract class Character
{
    protected int CurrentHealth;
    protected int MaxHealth;
    protected int Attack;
    public int Defence{get; protected set;}

    public virtual void ShowStatus()
    {
        
    }
}