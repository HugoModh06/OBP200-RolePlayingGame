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
    int HpModifer { get; }
    int AtkModifer { get; }
    int DefModifer { get; }

    int baseDamage { get; }
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

    public int HpModifer => 6;
    public int AtkModifer => 2;
    public int DefModifer => 2;

    public int baseDamage => 1;
}