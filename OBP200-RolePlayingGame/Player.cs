namespace OBP200_RolePlayingGame;

public class Player : Character
{
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
        MaxHp = Class.MaxHp;
        CurrentHp = MaxHp;
        Atk = Class.Atk;
        Def = Class.Def;
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
        MaxHp += Class.HpModifer;
        CurrentHp = MaxHp; //Läks helt vid level up
        Atk += Class.AtkModifer;
        Def += Class.DefModifer;
    }
    
    public void TestPrint()
    {
        Console.WriteLine($"Name: {Name}");
        Console.WriteLine($"Class: {Class.ClassName}");
        Console.WriteLine($"Gold: {Gold}");
        Console.WriteLine($"Potions: {Potions}");
        Console.WriteLine($"Max HP: {MaxHp}");
        Console.WriteLine($"Current HP: {CurrentHp}");
        Console.WriteLine($"Atk: {Atk}");
        Console.WriteLine($"Def: {Def}");
        Console.WriteLine($"Xp: {Xp}");
        Console.WriteLine($"Level: {Level}");
    }
    public void ShowStatus()
    {
        Console.WriteLine($"[{Name} | {Class.ClassName}]  HP {CurrentHp}/{MaxHp}  ATK {Atk}  DEF {Def}  LVL {Level}  XP {Xp}  Guld {Gold}  Drycker {Potions}");
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
}