namespace OBP200_RolePlayingGame;

public interface IClasses
{
    int[] GenerateClass(); //Ska returnera värden för klassen utifrån ett preset:[MaxHP, Atk, Def, potions, Gold]
    string ClassName { get; }
}

public class Warrior : IClasses
{
    public string ClassName => "Warrior";
    public int[] GenerateClass()
    {
        return [40, 7, 5, 2, 15]; 
    }
}