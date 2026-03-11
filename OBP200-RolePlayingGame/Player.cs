namespace OBP200_RolePlayingGame;

public class Player : Character
{
    static readonly Random Rng = new Random();
    private string Name;
    private IClasses Class;
    private int Gold;
    private int Potions;
    private int Level;
    private int Xp;
    private List<Loot> Inventory;
    
    
    public void GeneratePlayer(IClasses classPreset, string name)
    {
        Name = name;
        Class = classPreset;
        MaxHealth = Class.MaxHp;
        CurrentHealth = MaxHealth;
        Attack = Class.Attack;
        Defence = Class.Defense;
        Potions = Class.Potions;
        Gold = Class.Gold;
        Level = 1;
        Xp = 0;
        Inventory = new List<Loot>();
        Inventory.Add(new Loot("Wooden Sword", 0 ));
        Inventory.Add(new Loot("Cloth Armor", 0 ));
    }
    

    public void LevelUp()
    {
        Level++;
        Xp = 0;
        MaxHealth += Class.HpModifer;
        CurrentHealth = MaxHealth; //Läks helt vid level up
        Attack += Class.AtkModifer;
        Defence += Class.DefModifer;
    }
    
    //TEST METHOD: REMOVE LATER
    public void TestPrint()
    {
        Console.WriteLine($"Name: {Name}");
        Console.WriteLine($"Class: {Class.ClassName}");
        Console.WriteLine($"Gold: {Gold}");
        Console.WriteLine($"Potions: {Potions}");
        Console.WriteLine($"Max HP: {MaxHealth}");
        Console.WriteLine($"Current HP: {CurrentHealth}");
        Console.WriteLine($"Atk: {Attack}");
        Console.WriteLine($"Def: {Defence}");
        Console.WriteLine($"Xp: {Xp}");
        Console.WriteLine($"Level: {Level}");
    }
    
    public override void ShowStatus()
    {
        Console.WriteLine($"[{Name} | {Class.ClassName}]  HP {CurrentHealth}/{MaxHealth}  ATK {Attack}  DEF {Defence}  LVL {Level}  XP {Xp}  Guld {Gold}  Drycker {Potions}");
        Console.Write("Väska:");
        foreach (Loot loot in  Inventory)
        {
            Console.Write($"{loot.Name}; ");
        }
        Console.WriteLine();
    }
    
    public void AddLoot(string name, int value)
    {
        Inventory.Add(new Loot(name, value));
    }
    
    public void SellLoot(string lootName)
    {
        int valueOfSoldLoot = 0;
        foreach (var loot in Inventory)
        {
            if (loot.Name == lootName)
            {
                valueOfSoldLoot+=loot.Value;
            }
        }
        int amountSold = Inventory.RemoveAll(x => x.Name == lootName);

        Gold += valueOfSoldLoot;
        if (amountSold > 0)
        {
            Console.WriteLine($"Sålde {amountSold} {lootName} för {valueOfSoldLoot}. Ny total guld: {Gold}");
        }
        else
        {
            Console.WriteLine($"Du har ingen {lootName} att sälja.");
        }
    }

    public override int AttackCalculation(Character target)
    {
        int damage = Math.Max(1, Attack-(target.Defence/2));
        damage += Class.baseDamage;
        int extraDamageRoll = Rng.Next(0, 3);
        damage += extraDamageRoll;
        return damage;
    }
    
}