namespace OBP200_RolePlayingGame;

public interface IPlayerRolePreset
{
    //Standardvärden för varje klass som sätts vid start
    string RolePresetName { get; }
    int StartingMaxHeath { get; }
    int StartingAttack { get; }
    int StartingDefence { get; }
    int StartingPotions  { get; }
    int StartingGold { get; }
    
    //hur stor chans att man kan fly från fight
    double RunAwayFactor { get; }
    
    //Hur mycket varje stat ska öka vid level up
    int HeathLevelUpModifer { get; }
    int AttackLevelUpModifer { get; }
    int DefenceLevelUpModifer { get; }
    
    //base damage tillläg för klasser, samt hanterar critical hits för rouge
    int BaseDamageModifer();
    //speciella attaker som är unika för varje klass
    int SpecialAttack(Player player, Enemy enemy);
}
