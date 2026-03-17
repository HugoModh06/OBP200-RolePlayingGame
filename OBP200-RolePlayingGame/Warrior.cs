namespace OBP200_RolePlayingGame;


public class Warrior : IPlayerClassPreset
{
    public string ClassName => "Warrior";
    public int[] GenerateClass()
    {
        return [40, 7, 5, 2, 15]; 
    }

    public int StartingMaxHeath => 40;
    public int Attack => 7;
    public int Defense => 5;
    public int Potions => 2;
    public int Gold => 15;
    public double RunAwayFactor => 0.25;

    public int HeathLevelUpModifer => 6;
    public int AttackLevelUpModifer => 2;
    public int DefenseLevelUpModifer => 2;

    public int BaseDamage()
    {
        return 1;
    }
    
    
    public int SpecialAttack(Player player, Enemy enemy)
    {
        //specialattack som gör mer skada men spelaren tar 2 skada när den används
        Console.WriteLine("Warrior använder Heavy Strike!");
        int damage = Math.Max(2, player.Attack + 3 - enemy.Defence);
        player.TakeDamage(2); // självskada
        //bossar tar 20% mindre skada av specialattaker. 
        if (enemy.IsBoss)
        {
            damage = (int)Math.Round(damage * 0.8);
        }
        Console.WriteLine($"Special! {enemy.Name} tar {damage} skada.");
        return damage;
    }
}