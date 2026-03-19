namespace OBP200_RolePlayingGame;

public class Bandit :IEnemyTypePresets
{
    public string Name => "Bandit";
    public int MaxHealth => 16;
    public int Attack => 6;
    public int Defence => 1;
    public int GoldReward => 6;
    public bool IsBoss => false;
    public int ExperienceReward => 8;
}