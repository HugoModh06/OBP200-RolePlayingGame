namespace OBP200_RolePlayingGame;

public class Player : Character
{
    private string Name;
    private IClasses Class;
    private int Gold;
    private int Potions;
    private int Level;
    private int Xp;
    
    
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
    public void ShowStats()
    {
        Console.WriteLine($"Name: {Name}");
    }

}