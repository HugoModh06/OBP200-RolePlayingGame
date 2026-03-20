using System;
using System.Collections.Generic;

namespace OBP200_RolePlayingGame;

public class Player : Character
{
    static readonly Random Rng = new Random();
    private IPlayerRolePresets? _playerRolePreset;
    private int _gold;
    private int _potions;
    private int _level;
    private int _experience;
    
    //Spelarens förråd/väska där loot sparas
    private readonly List<Loot> _inventory =new();
    
    
    //metod som sätter alla värden utifrån en mall
    public void GeneratePlayer(IPlayerRolePresets playerClassPresetPreset, string name)
    {
        Name = name;
        _playerRolePreset = playerClassPresetPreset;
        MaxHealth = _playerRolePreset.StartingMaxHeath;
        CurrentHealth = MaxHealth;
        Attack = _playerRolePreset.StartingAttack;
        Defence = _playerRolePreset.StartingDefence;
        _potions = _playerRolePreset.StartingPotions;
        _gold = _playerRolePreset.StartingGold;
        _level = 1;
        _experience = 0;
        _inventory.Clear();
        AddLoot(new Loot("Wooden Sword", 0 ));
        AddLoot(new Loot("Cloth Armor", 0 ));
        
        Console.WriteLine($"Välkommen, {Name} the {_playerRolePreset.RolePresetName}!");
    }
    
    //Ökar level och stats utifrån klassens mall
    private void LevelUp()
    {
        _level++; 
        MaxHealth += _playerRolePreset.HeathLevelUpModifer;
        CurrentHealth = MaxHealth; //Läks helt vid level up
        Attack += _playerRolePreset.AttackLevelUpModifer;
        Defence += _playerRolePreset.DefenceLevelUpModifer;
    }
    
    //försöka fly från striden, chansen beror på klassmallen
    public bool TryRunAway()
    {
        return Rng.NextDouble() < _playerRolePreset.RunAwayFactor;
    }
    
    //ökar hur mycket xp spelaren har och kollar om man levelar upp
    public void AddPlayerExperience(int amount)
    {
        _experience+=amount;
        MaybeLevelUp();
    }
    
    //kolla om spelaren kommer levla upp
    private void MaybeLevelUp()
    {
        // Nivåtrösklar, hur mycket experience som behövs för att nå en vis level
        int nextThreshold;
        if (_level == 1)
        {
            nextThreshold = 10;
        }
        else if (_level == 2)
        {
            nextThreshold = 25;
        }
        else if (_level == 3)
        {
            nextThreshold = 45;
        }
        else
        {
            nextThreshold = _level * 20;
        }
        
        if (_experience >= nextThreshold)
        {
            LevelUp();
            Console.WriteLine($"Du når nivå {_level}! Värden ökade och HP återställd.");
        }
    }
    
    //skriver ut ens stats när man är i combat
    public override void ShowStatus()
    {
        Console.WriteLine($"[{Name} | {_playerRolePreset.RolePresetName}]  HP {CurrentHealth}/{MaxHealth}  ATK {Attack}  DEF {Defence}  LVL {_level}  XP {_experience}  Guld {_gold}  Drycker {_potions}");
        Console.Write("Väska:");
        foreach (Loot loot in  _inventory)
        {
            Console.Write($"{loot.Name}; ");
        }
        Console.WriteLine();
    }
    
    
    public void AddPlayerGold(int goldLoot)
    {
        _gold += goldLoot;
    }

    private void RemoveGold(int amount)
    {
        _gold -= amount;
        if (_gold < 0)
        {
            _gold = 0;
        }
    }
    
    
    public void AddLoot(Loot loot)
    {
        _inventory.Add(loot);
    }
    
    //försöka köpa ur butiken. buffType är vad man ska köpa (1 är potions, 2 attak och 3 defense) och strength är hur mycket de ökar
    public void AttemptToBuy(int cost, int buffType, int buffStrength)
    {
        if (_gold >= cost)
        {
            switch (buffType)
            {
                case 1:
                {
                    _potions+=buffStrength;
                    break;
                }
                case 2:
                {
                    Attack += buffStrength;
                    break;
                }
                case 3:
                {
                    Defence += buffStrength;
                    break;
                }
            }
            RemoveGold(cost);
            Console.WriteLine($"Köp lyckats.");
        }
        else
        {
            Console.WriteLine("Köp misslyckat. Inte tillräckligt guld.");
        }
    }
    
    //försök sälja en typ av loot
    public void SellLoot(string lootName)
    {
        int valueOfSoldLoot = 0;
        //loop som räknar ut sammanlagda värdet av all matchande loot
        foreach (var loot in _inventory)
        {
            if (loot.Name == lootName)
            {
                valueOfSoldLoot+=loot.Value;
            }
        }
        int amountSold = _inventory.RemoveAll(x => x.Name == lootName); //tar bort alla matchande samt räknar ut hur många som såldes

        AddPlayerGold(valueOfSoldLoot);
        //kollar om man sålde något eller inte för att skriva rätt medeleande
        if (amountSold > 0)
        {
            Console.WriteLine($"Sålde {amountSold} {lootName} för {valueOfSoldLoot}. Ny total guld: {_gold}");
        }
        else
        {
            Console.WriteLine($"Du har ingen {lootName} att sälja.");
        }
    }
    
    //Räknar ut hur mycket skada man gör
    public override int CalculateDamage(int targetDefence)
    {
        int damage = Math.Max(1, Attack-(targetDefence/2));
        damage += _playerRolePreset.BaseDamageModifer(); //ökar skada baserad på klass, som rouges chans för kritisk träff
        int extraDamageRoll = Rng.Next(0, 3); //slumpad värde för att ge mindre variation i hur mycket skada man gör
        damage += extraDamageRoll;
        Console.WriteLine($"{Name} anfaller och gör {damage} skada!");
        return damage;
    }
    
    //heala genom att vila i ett vila rum
    public void HealThroughRest()
    {
        CurrentHealth=MaxHealth;
    }
    
    //skriver ut värden som är relevanta för när man är i shop
    public void PrintShopRelevantStats()
    {
        Console.WriteLine($"Guld: {_gold} | Drycker: {_potions}");
    }
    
    public void DrinkPotion()
    {
        if (_potions <= 0)
        {
            Console.WriteLine("Du har inga drycker kvar.");
            return;
        }

        // Helning av spelaren
        const int healAmmount = 12;
        int newHealth = Math.Min(MaxHealth, CurrentHealth + healAmmount);
        CurrentHealth = newHealth;
        Console.WriteLine($"Du dricker en dryck och återfår {newHealth - CurrentHealth} HP.");
        _potions--;
    }
    
    public int UseSpecialAttack(int targetDefence, bool isBoss)
    {
        int damage;
        //väljer vilken specialattack som används. Om den inte har en registrerad klass (vilket borde vara omöjligt) körs Warrior som standard, samma som i ursprungsprogrammet
        switch (_playerRolePreset)
        {
            case Warrior:
            {
                damage = WarriorSpecialAttack(targetDefence);
                break;
            }
            case Mage:
            {
                damage = MageSpecialAttack(targetDefence);
                break;
            }
            case Rouge:
            {
                damage = RougeSpecialAttack();
                break;
            }
            default:
            {
                damage=WarriorSpecialAttack(targetDefence);
                break;
            }
        }
        
        //bossfiender tar 20% mindre damage av en backstab
        if (isBoss)
        {
            damage = (int)Math.Round(damage * 0.8);
        }
        
        
        Console.WriteLine($"Special! {Name} anfaller och gör {damage} skada.");
        return damage;
    }

    private int RougeSpecialAttack()
    {
        int damage;
         
        //specialattak som har chans att ignorera fiendens defence stat, annars gör bara 1 skada
        if (Rng.NextDouble() < 0.5)
        {
            Console.WriteLine("Rogue utför en lyckad Backstab!");
            damage = Math.Max(4, Attack + 6);
        }
        else
        {
            Console.WriteLine("Backstab misslyckades!");
            damage = 1;
        }
        
        return damage;
    }
    
    private int WarriorSpecialAttack(int enemyDefence)
    {
        //specialattack som gör mer skada men spelaren tar 2 skada när den används
        Console.WriteLine("Warrior använder Heavy Strike!");
        int damage = Math.Max(2, Attack + 3 - enemyDefence);
        TakeDamage(2); // självskada
        return damage;
    }

    private int MageSpecialAttack(int enemyDefence)
    {
        //Specialattak som gör mycket skada men kostar guld att använda
        int damage;
        if (_gold >= 3)
        {
            Console.WriteLine("Mage kastar Fireball!");
            RemoveGold(3);
            damage = Math.Max(3, Attack + 5 - (enemyDefence / 2));
        }
        else
        {
            Console.WriteLine("Inte tillräckligt med guld för att kasta Fireball (kostar 3).");
            damage = 0;
        }
        return damage;
    }
}