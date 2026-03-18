namespace OBP200_RolePlayingGame;


public class Slime : IEnemyTypePresets
{
    public string Name => "Geléslem";
    public int MaxHealth => 14;
    public int Attack => 3;
    public int Defence => 0;
    public int GoldReward => 3;
    public bool IsBoss => false;
    public int XpReward => 5;
}