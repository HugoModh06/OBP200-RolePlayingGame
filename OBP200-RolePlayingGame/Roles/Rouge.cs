using System;

namespace OBP200_RolePlayingGame;

public class Rouge : IPlayerRolePresets
{
    public string RolePresetName => "Rouge";
    

    public int StartingMaxHeath => 40;
    public int StartingAttack => 7;
    public int StartingDefence => 5;
    public int StartingPotions => 2;
    public int StartingGold => 15;
    public double RunAwayFactor => 0.5;

    public int HeathLevelUpModifer => 6;
    public int AttackLevelUpModifer => 2;
    public int DefenceLevelUpModifer => 2;

    public int BaseDamageModifer()
    {
        //crit chans
        Random Rng = new Random();
        if (Rng.NextDouble() < 0.2)
        {
            return 4;
        }

        return 0;
    }
}