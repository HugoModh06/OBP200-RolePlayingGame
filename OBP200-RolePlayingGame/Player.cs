namespace OBP200_RolePlayingGame;

public class Player : Character
{
    private string Name;
    private string Class;
    private int Gold;
    private int Potions;
    private int Level;
    private int Xp;
    
    public void GeneratePlayer(IClasses classPreset, string name)
    {
        Name = name;
        Class = classPreset.ClassName;
        int[] preset = classPreset.GenerateClass();
        MaxHp = preset[0];
        CurrentHp = MaxHp;
        Atk = preset[1];
        Def = preset[2];
        Potions = preset[3];
        Gold = preset[4];
        Level = 1;
        Xp = 0;
    }

    public void LevelUp()
    {
        Level++;
        Xp = 0;
        
    }
    
    public void TestPrint()
    {
        Console.WriteLine($"Name: {Name}");
        Console.WriteLine($"Class: {Class}");
        Console.WriteLine($"Gold: {Gold}");
        Console.WriteLine($"Potions: {Potions}");
        Console.WriteLine($"Max HP: {MaxHp}");
        Console.WriteLine($"Current HP: {CurrentHp}");
        Console.WriteLine($"Atk: {Atk}");
        Console.WriteLine($"Def: {Def}");
        Console.WriteLine($"Xp: {Xp}");
        Console.WriteLine($"Level: {Level}");
    }

}