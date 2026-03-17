namespace OBP200_RolePlayingGame;

public class Mage : IPlayerClassPreset
{
    public string ClassName => "Mage";
    public int[] GenerateClass()
    {
        return [28, 10, 2, 2, 15]; 
    }

    public int StartingMaxHeath => 28;
    public int Attack => 10;
    public int Defense => 2;
    public int Potions => 2;
    public int Gold => 15;
    public double RunAwayFactor => 0.35;

    public int HeathLevelUpModifer => 4;
    public int AttackLevelUpModifer => 4;
    public int DefenseLevelUpModifer => 1;
    
    public int BaseDamage()
    {
        return 2;
    }
    
    public int SpecialAttack(Player player, Enemy enemy)
    {
        //Specialattak som gör mycket skada men kostar guld att använda
        int damage = 0;
        if (player.Gold >= 3)
        {
            Console.WriteLine("Mage kastar Fireball!");
            player.RemoveGold(3);
            damage = Math.Max(3, player.Attack + 5 - (enemy.Defence / 2));
        }
        else
        {
            Console.WriteLine("Inte tillräckligt med guld för att kasta Fireball (kostar 3).");
            damage = 0;
            return damage;
        }
        //bossar tar 20% mindre skada av specialattaker
        if (enemy.IsBoss)
        {
            damage = (int)Math.Round(damage * 0.8);
        }
        Console.WriteLine($"Special! {enemy.Name} tar {damage} skada.");
        return damage;
    }
}