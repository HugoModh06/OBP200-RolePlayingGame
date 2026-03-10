namespace OBP200_RolePlayingGame;

public interface IClasses
{
    int[] GenerateClass();
    string ClassName { get; }
}

public class Warrior : IClasses
{
    public string ClassName => "Warrior";
    public int[] GenerateClass()
    {
        return [40, 7, 5, 2, 15]; //returns what to set values to:[MaxHP, Atk, Def, potions, Gold]
    }
}