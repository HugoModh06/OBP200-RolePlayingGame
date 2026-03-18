namespace OBP200_RolePlayingGame;


public class Warrior : IPlayerRolePreset
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
    
    
    public int SpecialAttack(Player player, Enemy enemy)
    {
        //specialattack som gör mer skada men spelaren tar 2 skada när den används
        Console.WriteLine("Warrior använder Heavy Strike!");
        int damage = Math.Max(2, player.Attack + 3 - enemy.Defence);
        player.TakeDamage(2); // självskada
        //bossar tar 20% mindre skada av specialattaker. 
        if (enemy.IsBoss)
        {
            damage = (int)Math.Round(damage * 0.8);
        }
        return damage;
    }
}