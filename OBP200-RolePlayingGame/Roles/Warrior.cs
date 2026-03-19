using System;

namespace OBP200_RolePlayingGame;


public class Warrior : IPlayerRolePresets
{
    public string RolePresetName => "Warrior";

    public int StartingMaxHeath => 40;
    public int StartingAttack => 7;
    public int StartingDefence => 5;
    public int StartingPotions => 2;
    public int StartingGold => 15;
    public double RunAwayFactor => 0.25;

    public int HeathLevelUpModifer => 6;
    public int AttackLevelUpModifer => 2;
    public int DefenceLevelUpModifer => 2;

    public int BaseDamageModifer()
    {
        return 1;
    }
    
    
    
}