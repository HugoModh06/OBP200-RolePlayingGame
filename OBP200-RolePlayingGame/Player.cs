namespace OBP200_RolePlayingGame;

public class Player : Character, IAttackCapable
{
    static readonly Random Rng = new Random();
    //private string Name;
    private IPlayerRolePreset _playerRolePreset;
    public int Gold { get; private set; }
    private int _potions;
    private int _level;
    private int _experience;
    
    //Spelarens förråd/väska där loot sparas
    private List<Loot> _inventory =new();
    
    
    //metod som sätter alla värden utifrån en mall
    public void GeneratePlayer(IPlayerRolePreset playerClassPresetPreset, string name)
    {
        Name = name;
        _playerRolePreset = playerClassPresetPreset;
        MaxHealth = _playerRolePreset.StartingMaxHeath;
        CurrentHealth = MaxHealth;
        Attack = _playerRolePreset.StartingAttack;
        Defence = _playerRolePreset.StartingDefence;
        _potions = _playerRolePreset.StartingPotions;
        Gold = _playerRolePreset.StartingGold;
        _level = 1;
        _experience = 0;
        _inventory.Clear();
        _inventory.Add(new Loot("Wooden Sword", 0 ));
        _inventory.Add(new Loot("Cloth Armor", 0 ));
        
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
    public void AddPlayerXp(int amount)
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
        Console.WriteLine($"[{Name} | {_playerRolePreset.RolePresetName}]  HP {CurrentHealth}/{MaxHealth}  ATK {Attack}  DEF {Defence}  LVL {_level}  XP {_experience}  Guld {Gold}  Drycker {_potions}");
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
        if (Gold < 0)
        {
            Gold = 0;
        }
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
            Console.WriteLine($"Sålde {amountSold} {lootName} för {valueOfSoldLoot}. Ny total guld: {Gold}");
        }
        else
        {
            Console.WriteLine($"Du har ingen {lootName} att sälja.");
        }
    }
    
    //Räknar ut hur mycket skada man gör
    public override int CalculateDamage(Character target)
    {
        int damage = Math.Max(1, Attack-(target.Defence/2));
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
        Console.WriteLine($"Guld: {Gold} | Drycker: {_potions}");
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
    
    public void UseSpecialAttack(Player player, Enemy target)
    {
        int damage = _playerRolePreset.SpecialAttack(player, target);
        target.TakeDamage(damage);
        Console.WriteLine($"Special! {Name} anfaller och gör {damage} skada.");
    }
}