namespace OBP200_RolePlayingGame;

public class Enemy : Character
{
    static readonly Random Rng = new Random();
    private IEnemyType _type;
    private string _name;
    private int _goldReward;
    private int _xpReward;
    private bool _isBoss;

    public void GenerateEnemy(IEnemyType enemyType)
    {
        _type = enemyType;
        _name=_type.Name;
        _isBoss=_type.IsBoss;
        
        if (_isBoss == false)
        {
            MaxHealth = enemyType.MaxHealth+ Rng.Next(-1, 3);
            Attack = _type.Attack+ Rng.Next(0, 2);
            Defence = _type.Defence + Rng.Next(0, 2);
            _goldReward = _type.GoldReward+ Rng.Next(0, 3);
            _xpReward = _type.XpReward+ Rng.Next(0, 3);
        }
        else
        {
            MaxHealth = enemyType.MaxHealth;
            Attack = _type.Attack;
            Defence = _type.Defence;
            _goldReward = _type.GoldReward;
            _xpReward = _type.XpReward;
        }
        CurrentHealth = MaxHealth;
    }

    public override void ShowStatus()
    {
        Console.WriteLine($"Fiende: {_name} HP={CurrentHealth}");
        if (_isBoss)
        {
            Console.WriteLine("(Du kan inte fly från en boss!)");
        }
    }
    
    public override int AttackCalculation(Character target)
    {
        int damage = Math.Max(1, Attack-(target.Defence/2));
        int extraDamageRoll = Rng.Next(0, 3);
        damage += extraDamageRoll;
        
        // Liten chans till "glancing blow" (minskad skada)
        if (Rng.NextDouble() < 0.1)
        {
            damage = Math.Max(1, damage - 2);
        }
        return damage;
    }
    
    public void TestPrint()
    {
        Console.WriteLine($"Name: {_name}");
        Console.WriteLine($"Gold: {_goldReward}");
        Console.WriteLine($"Max HP: {MaxHealth}");
        Console.WriteLine($"Current HP: {CurrentHealth}");
        Console.WriteLine($"Atk: {Attack}");
        Console.WriteLine($"Def: {Defence}");
        Console.WriteLine($"Xp: {_xpReward}");
    }
}



public interface IEnemyType
{
    string Name { get; }
    int Attack { get; }
    int Defence { get; }
    int MaxHealth { get; }
    bool IsBoss { get; }
    int XpReward { get; }
    int GoldReward { get; }
}

public class Bandit :IEnemyType
{
    public string Name => "Bandit";
    public int MaxHealth => 16;
    public int Attack => 6;
    public int Defence => 1;
    public int GoldReward => 6;
    public bool IsBoss => false;
    public int XpReward => 8;
}