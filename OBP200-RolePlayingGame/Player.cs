namespace OBP200_RolePlayingGame;

public class Player : Character
{
    private string Name;
    private string Class;
    private int Gold;
    private int Potions;
        
    public void GeneratePlayer(IClasses classPreset)
    {
        int[] preset = classPreset.GenerateClass();
        MaxHp = preset[0];
        CurrentHp = MaxHp;
        Atk = preset[1];
        Def = preset[2];
        Potions = preset[3];
        Gold = preset[4];
    }

}