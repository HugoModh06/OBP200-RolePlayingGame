using System;

namespace OBP200_RolePlayingGame;

public class Enemy : GameCharacter
{
    static readonly Random Rng = new Random();
    private IEnemyTypePreset _enemyType;
    public int GoldReward{ get; private set;}
    public int ExperienceReward { get; private set;}
    public bool IsBoss { get; private set; }
    
    //skapar en fiende baserad på en mall
    public void GenerateEnemy(IEnemyTypePreset enemyType)
    {
        _enemyType = enemyType;
        Name=_enemyType.Name;
        IsBoss=_enemyType.IsBoss;
        
        
        if (IsBoss == false)
        {
            //om en fiende inte är boss sker en liten slumpmässig justering av statsen,
            MaxHealth = enemyType.MaxHealth+ Rng.Next(-1, 3);
            Attack = _enemyType.Attack+ Rng.Next(0, 2);
            Defence = _enemyType.Defence + Rng.Next(0, 2);
            GoldReward = _enemyType.GoldReward+ Rng.Next(0, 3);
            ExperienceReward = _enemyType.ExperienceReward+ Rng.Next(0, 3);
        }
        else
        {
            //boss fiender har alltid samma värden
            MaxHealth = enemyType.MaxHealth;
            Attack = _enemyType.Attack;
            Defence = _enemyType.Defence;
            GoldReward = _enemyType.GoldReward;
            ExperienceReward = _enemyType.ExperienceReward;
        }
        CurrentHealth = MaxHealth;
        
        Console.WriteLine($"En {Name} dyker upp! (HP {CurrentHealth}, ATK {Attack}, DEF {Defence})");
    }
    
    //Visar status av fienden
    public override void ShowStatus()
    {
        Console.WriteLine($"Fiende: {Name} HP={CurrentHealth}");
        if (IsBoss)
        {
            Console.WriteLine("(Du kan inte fly från en boss!)");
        }
    }
    
    
    public override int CalculateDamage(int targetDefence)
    {
        int damage = Math.Max(1, Attack-(targetDefence/2));
        int extraDamageRoll = Rng.Next(0, 3);
        damage += extraDamageRoll;
        
        // Liten chans att spelaren får ett "glancing blow" och tar mindre skada
        if (Rng.NextDouble() < 0.1)
        {
            damage = Math.Max(1, damage - 2);
        }
        Console.WriteLine($"{Name} anfaller och gör {damage} skada!");
        return damage;
    }
    
    
    
    public Loot MaybeDropLoot()
    {
        // Enkel loot-regel, fiende har ca 35% chans att ge loot
        if (Rng.NextDouble() < 0.35)
        {
            string itemName = "Minor Gem";
            int itemValue = 5;
            //om fienden är en drake får man en bättre bit loot
            if (Name.Contains("Urdraken"))
            {
                itemName = "Dragon Scale";
                itemValue = 25;
            }
            
            
            Console.WriteLine($"Föremål hittat: {itemName} (lagt i din väska)");
            return new Loot(itemName, itemValue);
        }
        return new Loot("Worthless Rock", 0);
    }
}