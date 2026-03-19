using System;

namespace OBP200_RolePlayingGame;

public class Enemy : Character
{
    static readonly Random Rng = new Random();
    private IEnemyTypePresets _type;
    public int GoldReward{ get; private set;}
    public int ExperienceReward { get; private set;}
    public bool IsBoss { get; private set; }
    
    //skapar en fiende baserad på en mall
    public void GenerateEnemy(IEnemyTypePresets enemyType)
    {
        _type = enemyType;
        Name=_type.Name;
        IsBoss=_type.IsBoss;
        
        
        if (IsBoss == false)
        {
            //om en fiende inte är boss sker en liten slumpmässig justering av statsen,
            MaxHealth = enemyType.MaxHealth+ Rng.Next(-1, 3);
            Attack = _type.Attack+ Rng.Next(0, 2);
            Defence = _type.Defence + Rng.Next(0, 2);
            GoldReward = _type.GoldReward+ Rng.Next(0, 3);
            ExperienceReward = _type.ExperienceReward+ Rng.Next(0, 3);
        }
        else
        {
            //boss fiender har alltid samma värden
            MaxHealth = enemyType.MaxHealth;
            Attack = _type.Attack;
            Defence = _type.Defence;
            GoldReward = _type.GoldReward;
            ExperienceReward = _type.ExperienceReward;
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
    
    
    public override int CalculateDamage(Character target)
    {
        int damage = Math.Max(1, Attack-(target.Defence/2));
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
    
    public void MaybeDropLoot(Player player)
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
            
            player.AddLoot(itemName, itemValue);
            Console.WriteLine($"Föremål hittat: {itemName} (lagt i din väska)");
        }
    }
}