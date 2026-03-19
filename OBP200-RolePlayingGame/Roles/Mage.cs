using System;

namespace OBP200_RolePlayingGame;

public class Mage : IPlayerRolePresets
{
    public string RolePresetName => "Mage";

    public int StartingMaxHeath => 28;
    public int StartingAttack => 10;
    public int StartingDefence => 2;
    public int StartingPotions => 2;
    public int StartingGold => 15;
    public double RunAwayFactor => 0.35;

    public int HeathLevelUpModifer => 4;
    public int AttackLevelUpModifer => 4;
    public int DefenceLevelUpModifer => 1;
    
    public int BaseDamageModifer()
    {
        return 2;
    }
}