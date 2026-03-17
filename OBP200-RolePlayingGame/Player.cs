namespace OBP200_RolePlayingGame;

public class Player : Character
{
    static readonly Random Rng = new Random();
    //private string Name;
    private IPlayerClassPreset _playerClassPreset;
    public int Gold { get; private set; }
    public int Potions { get; private set; }
    private int _level;
    private int _xp;
    
    //Spelarens förråd/väska där loot sparas
    private List<Loot> _inventory =new();
    
    
    //metod som sätter alla värden utifrån ett preset
    public void GeneratePlayer(IPlayerClassPreset playerClassPresetPreset, string name)
    {
        Name = name;
        _playerClassPreset = playerClassPresetPreset;
        MaxHealth = _playerClassPreset.StartingMaxHeath;
        CurrentHealth = MaxHealth;
        Attack = _playerClassPreset.StartingAttack;
        Defence = _playerClassPreset.StartingDefence;
        Potions = _playerClassPreset.StartingPotions;
        Gold = _playerClassPreset.StartingGold;
        _level = 1;
        _xp = 0;
        _inventory.Clear();
        _inventory.Add(new Loot("Wooden Sword", 0 ));
        _inventory.Add(new Loot("Cloth Armor", 0 ));
        
        Console.WriteLine($"Välkommen, {Name} the {_playerClassPreset.ClassPresetName}!");
    }
    
    //Ökar level och stats utifrån klassens mall
    public void LevelUp()
    {
        _level++; 
        MaxHealth += _playerClassPreset.HeathLevelUpModifer;
        CurrentHealth = MaxHealth; //Läks helt vid level up
        Attack += _playerClassPreset.AttackLevelUpModifer;
        Defence += _playerClassPreset.DefenceLevelUpModifer;
    }
    
    //försöka fly från striden, chansen beror på klassmallen
    public bool TryRunAway()
    {
        return Rng.NextDouble() < _playerClassPreset.RunAwayFactor;
    }
    
    public void AddPlayerXp(int amount)
    {
        _xp+=amount;
        MaybeLevelUp();
    }
    
    private void MaybeLevelUp()
    {
        // Nivåtrösklar
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
        
        if (_xp >= nextThreshold)
        {
            LevelUp();
            Console.WriteLine($"Du når nivå {_level}! Värden ökade och HP återställd.");
        }
    }
    
    public override void ShowStatus()
    {
        Console.WriteLine($"[{Name} | {_playerClassPreset.ClassPresetName}]  HP {CurrentHealth}/{MaxHealth}  ATK {Attack}  DEF {Defence}  LVL {_level}  XP {_xp}  Guld {Gold}  Drycker {Potions}");
        Console.Write("Väska:");
        foreach (Loot loot in  _inventory)
        {
            Console.Write($"{loot.Name}; ");
        }
        Console.WriteLine();
    }
    
    
    public void AddPlayerGold(int goldLoot)
    {
        Gold += goldLoot;
    }
    
    public void RemoveGold(int amount)
    {
        Gold -= amount;
    }
    
    public void AddLoot(string name, int value)
    {
        _inventory.Add(new Loot(name, value));
    }
    
    //försöka köpa ur butiken. buffType är vad man ska köpa (1 är potions, 2 attak och 3 defense) och strength är hur mycket de ökar
    public void AttemptToBuy(int cost, int buffType, int buffStrength)
    {
        if (Gold >= cost)
        {
            switch (buffType)
            {
                case 1:
                {
                    Potions+=buffStrength;
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
            Console.WriteLine($"Sålde {amountSold} {lootName} för {valueOfSoldLoot}. Ny total guld: {Gold}");
        }
        else
        {
            Console.WriteLine($"Du har ingen {lootName} att sälja.");
        }
    }
    
    //Räknar ut hur mycket skada man gör
    public override int CalculateDamafe(Character target)
    {
        int damage = Math.Max(1, Attack-(target.Defence/2));
        damage += _playerClassPreset.BaseDamageModifer();
        int extraDamageRoll = Rng.Next(0, 3);
        damage += extraDamageRoll;
        Console.WriteLine($"{Name} anfaller och gör {damage} skada!");
        return damage;
    }
    
    //heala genom att vila i ett vila rum
    public void HealThroughRest()
    {
        CurrentHealth=MaxHealth;
    }

    public void DrinkPotion()
    {
        if (Potions <= 0)
        {
            Console.WriteLine("Du har inga drycker kvar.");
            return;
        }

        // Helning av spelaren
        const int healAmmount = 12;
        int newHealth = Math.Min(MaxHealth, CurrentHealth + healAmmount);
        CurrentHealth = newHealth;
        Console.WriteLine($"Du dricker en dryck och återfår {newHealth - CurrentHealth} HP.");
        Potions--;
    }
    
    public void UseSpecialAttack(Player player, Enemy target)
    {
        target.TakeDamage(_playerClassPreset.SpecialAttack(player, target));
    }
}