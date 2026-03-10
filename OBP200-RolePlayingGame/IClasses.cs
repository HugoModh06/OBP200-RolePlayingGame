namespace OBP200_RolePlayingGame;

public interface IClasses
{
    int[] GenerateClass();
}

public class Warrior : IClasses
{
    public int[] GenerateClass()
    {
        return [40, 7, 5, 2, 15];
    }
}