namespace OBP200_RolePlayingGame;

public class Rouge : IPlayerClassPreset
{
    public string ClassName => "Rouge";

    public int[] GenerateClass()
    {
        return [40, 7, 5, 2, 15];
    }

    public int StartingMaxHeath => 40;
    public int Attack => 7;
    public int Defense => 5;
    public int Potions => 2;
    public int Gold => 15;
    public double RunAwayFactor => 0.5;

    public int HeathLevelUpModifer => 6;
    public int AttackLevelUpModifer => 2;
    public int DefenseLevelUpModifer => 2;

    public int BaseDamage()
    {
        //crit chans
        Random Rng = new Random();
        if (Rng.NextDouble() < 0.2)
        {
            return 4;
        }

        return 0;
    }

    public int SpecialAttack(Player player, Enemy enemy)
    {
        int damage;
        Random rng = new Random();
        //specialattak som har chans att ignorera fiendens defence stat, annars gör bara 1 skada
        if (rng.NextDouble() < 0.5)
        {
            Console.WriteLine("Rogue utför en lyckad Backstab!");
            damage = Math.Max(4, player.Attack + 6);
        }
        else
        {
            Console.WriteLine("Backstab misslyckades!");
            damage = 1;
        }
        
        //bossfiender tar 20% mindre damage av en backstab
        if (enemy.IsBoss)
        {
            damage = (int)Math.Round(damage * 0.8);
        }

        Console.WriteLine($"Special! {enemy.Name} tar {damage} skada.");

        return damage;
    }
}