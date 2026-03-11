namespace OBP200_RolePlayingGame;

public interface IClasses
{
    int[] GenerateClass(); //Ska returnera värden för klassen utifrån ett preset:[MaxHP, Atk, Def, potions, Gold]
    string ClassName { get; }
    int MaxHp { get; }
    int Attack { get; }
    int Defense { get; }
    int Potions  { get; }
    int Gold { get; }
    
    //Hur mycket varje stat ska öka vid level up
    int HeathLevelUpModifer { get; }
    int AtkModifer { get; }
    int DefModifer { get; }

    int baseDamage { get; }
    int SpecialAttack(Player player, Enemy enemy);
}

public class Warrior : IClasses
{
    public string ClassName => "Warrior";
    public int[] GenerateClass()
    {
        return [40, 7, 5, 2, 15]; 
    }

    public int MaxHp => 40;
    public int Attack => 7;
    public int Defense => 5;
    public int Potions => 2;
    public int Gold => 15;

    public int HeathLevelUpModifer => 6;
    public int AtkModifer => 2;
    public int DefModifer => 2;

    public int baseDamage => 1;

    public int SpecialAttack(Player player, Enemy enemy)
    {
        Console.WriteLine("Warrior använder Heavy Strike!");
        int damage = Math.Max(2, player.Attack + 3 - enemy.Defence);
        player.TakeDamage(2); // självskada
        return damage;
    }
}

public class Mage : IClasses
{
    public string ClassName => "Mage";
    public int[] GenerateClass()
    {
        return [40, 7, 5, 2, 15]; 
    }

    public int MaxHp => 28;
    public int Attack => 10;
    public int Defense => 2;
    public int Potions => 2;
    public int Gold => 15;

    public int HeathLevelUpModifer => 4;
    public int AtkModifer => 4;
    public int DefModifer => 1;

    public int baseDamage => 2;

    public int SpecialAttack(Player player, Enemy enemy)
    {
        //Add gold removal
        int damage = 0;
        if (player.Gold >= 3)
        {
            Console.WriteLine("Mage kastar Fireball!");
            
            damage = Math.Max(3, player.Attack + 5 - (enemy.Defence / 2));
        }
        else
        {
            Console.WriteLine("Inte tillräckligt med guld för att kasta Fireball (kostar 3).");
            damage = 0;
        }
        return damage;
    }
}