namespace OBP200_RolePlayingGame;

public class Enemy : Character
{
    static readonly Random Rng = new Random();
    private IEnemyType _type;
    public string Name { get; private set; }
    public int _goldReward{ get; private set;}
    public int _xpReward { get; private set;}
    public bool IsBoss { get; private set; }

    public void GenerateEnemy(IEnemyType enemyType)
    {
        _type = enemyType;
        Name=_type.Name;
        IsBoss=_type.IsBoss;
        
        if (IsBoss == false)
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
        
        Console.WriteLine($"En {Name} dyker upp! (HP {CurrentHealth}, ATK {Attack}, DEF {Defence})");
    }

    public override void ShowStatus()
    {
        Console.WriteLine($"Fiende: {Name} HP={CurrentHealth}");
        if (IsBoss)
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
        Console.WriteLine($"{Name} anfaller och gör {damage} skada!");
        return damage;
    }
    
    public override void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        Console.WriteLine($"Du slog {Name} för {damage} skada.");
    }

    

    public void TestPrint()
    {
        Console.WriteLine($"Name: {Name}");
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

public class Beast :IEnemyType
{
    public string Name => "Vildsvin";
    public int MaxHealth => 18;
    public int Attack => 4;
    public int Defence => 1;
    public int GoldReward => 4;
    public bool IsBoss => false;
    public int XpReward => 6;
}
public class Slime :IEnemyType
{
    public string Name => "Geléslem";
    public int MaxHealth => 14;
    public int Attack => 3;
    public int Defence => 0;
    public int GoldReward => 3;
    public bool IsBoss => false;
    public int XpReward => 5;
}

public class Skeleton :IEnemyType
{
    public string Name => "Skelett";
    public int MaxHealth => 20;
    public int Attack => 5;
    public int Defence => 2;
    public int XpReward => 7;
    public int GoldReward => 5;
    public bool IsBoss => false;
}

public class Dragon :IEnemyType
{
    public string Name => "Urdraken";
    public int MaxHealth => 20;
    public int Attack => 5;
    public int Defence => 2;
    public int XpReward => 7;
    public int GoldReward => 5;
    public bool IsBoss => true;
}