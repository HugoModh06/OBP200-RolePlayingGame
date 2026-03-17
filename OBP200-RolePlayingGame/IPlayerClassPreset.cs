namespace OBP200_RolePlayingGame;

public interface IPlayerClassPreset
{
    int[] GenerateClass(); //Ska returnera värden för klassen utifrån ett preset:[MaxHP, Atk, Def, potions, Gold]
    
    //Standardvärden för varje klass som sätts vid start
    string ClassName { get; }
    int StartingMaxHeath { get; }
    int Attack { get; }
    int Defense { get; }
    int Potions  { get; }
    int Gold { get; }
    
    //hur stor chans att man kan fly från fight
    double RunAwayFactor { get; }
    
    //Hur mycket varje stat ska öka vid level up
    int HeathLevelUpModifer { get; }
    int AttackLevelUpModifer { get; }
    int DefenseLevelUpModifer { get; }
    
    //base damage tillläg för klasser, samt hanterar critical hits för rouge
    int BaseDamage();
    //speciella attaker som är unika för varje klass
    int SpecialAttack(Player player, Enemy enemy);
}
