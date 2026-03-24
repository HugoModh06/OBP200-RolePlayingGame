namespace OBP200_RolePlayingGame;

public class Beast :IEnemyTypePreset
{
    public string Name => "Vildsvin";
    public int MaxHealth => 18;
    public int Attack => 4;
    public int Defence => 1;
    public int GoldReward => 4;
    public bool IsBoss => false;
    public int ExperienceReward => 6;
} 