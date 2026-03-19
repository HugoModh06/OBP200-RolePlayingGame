namespace OBP200_RolePlayingGame;

public class Dragon :IEnemyTypePresets
{
    public string Name => "Urdraken";
    public int MaxHealth => 20;
    public int Attack => 5;
    public int Defence => 2;
    public int ExperienceReward => 7;
    public int GoldReward => 5;
    public bool IsBoss => true;
}