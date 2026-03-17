namespace OBP200_RolePlayingGame;

public interface IEnemyTypePresets
{
    //Standardvärden för fiender, olika för olika typer av fiender.
    string Name { get; }
    int Attack { get; }
    int Defence { get; }
    int MaxHealth { get; }
    bool IsBoss { get; }
    int XpReward { get; }
    int GoldReward { get; }
}