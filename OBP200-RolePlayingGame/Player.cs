namespace OBP200_RolePlayingGame;

public class Player : Character
{
    static readonly Random Rng = new Random();
    private string Name;
    private IPlayerClassPreset _playerClassPreset;
    public int Gold { get; set; }
    public int Potions { get; private set; }
    private int Level;
    private int Xp;
    private List<Loot> Inventory;
    
    
    public void GeneratePlayer(IPlayerClassPreset playerClassPresetPreset, string name)
    {
        Name = name;
        _playerClassPreset = playerClassPresetPreset;
        MaxHealth = _playerClassPreset.StartingMaxHeath;
        CurrentHealth = MaxHealth;
        Attack = _playerClassPreset.Attack;
        Defence = _playerClassPreset.Defense;
        Potions = _playerClassPreset.Potions;
        Gold = _playerClassPreset.Gold;
        Level = 1;
        Xp = 0;
        Inventory = new List<Loot>();
        Inventory.Add(new Loot("Wooden Sword", 0 ));
        Inventory.Add(new Loot("Cloth Armor", 0 ));
        
        Console.WriteLine($"Välkommen, {Name} the {_playerClassPreset.ClassName}!");

    }
    

    public void LevelUp()
    {
        Level++; 
        MaxHealth += _playerClassPreset.HeathLevelUpModifer;
        CurrentHealth = MaxHealth; //Läks helt vid level up
        Attack += _playerClassPreset.AtkModifer;
        Defence += _playerClassPreset.DefModifer;
    }

    public bool TryRunAway()
    {
        return Rng.NextDouble() < _playerClassPreset.RunAwayFactor;
    }
    
    public void AddPlayerXp(int amount)
    {
        Xp+=amount;
        MaybeLevelUp();
    }
    
    private void MaybeLevelUp()
    {
        // Nivåtrösklar
        int nextThreshold;
        if (Level == 1)
        {
            nextThreshold = 10;
        }
        else if (Level == 2)
        {
            nextThreshold = 25;
        }
        else if (Level == 3)
        {
            nextThreshold = 45;
        }
        else
        {
            nextThreshold = Level * 20;
        }
        
        if (Xp >= nextThreshold)
        {
            LevelUp();
            Console.WriteLine($"Du når nivå {Level}! Värden ökade och HP återställd.");
        }
    }
    
    public override void ShowStatus()
    {
        Console.WriteLine($"[{Name} | {_playerClassPreset.ClassName}]  HP {CurrentHealth}/{MaxHealth}  ATK {Attack}  DEF {Defence}  LVL {Level}  XP {Xp}  Guld {Gold}  Drycker {Potions}");
        Console.Write("Väska:");
        foreach (Loot loot in  Inventory)
        {
            Console.Write($"{loot.Name}; ");
        }
        Console.WriteLine();
    }

    public void AddPlayerGold(int goldLoot)
    {
        Gold += goldLoot;
    }
    
    public void AddLoot(string name, int value)
    {
        Inventory.Add(new Loot(name, value));
    }

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
            Console.WriteLine($"Köp lyckats.");
        }
        else
        {
            Console.WriteLine("Köp misslyckat. Inte tillräckligt guld.");
        }
    }
    
    public void SellLoot(string lootName)
    {
        int valueOfSoldLoot = 0;
        foreach (var loot in Inventory)
        {
            if (loot.Name == lootName)
            {
                valueOfSoldLoot+=loot.Value;
            }
        }
        int amountSold = Inventory.RemoveAll(x => x.Name == lootName);

        Gold += valueOfSoldLoot;
        if (amountSold > 0)
        {
            Console.WriteLine($"Sålde {amountSold} {lootName} för {valueOfSoldLoot}. Ny total guld: {Gold}");
        }
        else
        {
            Console.WriteLine($"Du har ingen {lootName} att sälja.");
        }
    }

    public override int AttackCalculation(Character target)
    {
        int damage = Math.Max(1, Attack-(target.Defence/2));
        damage += _playerClassPreset.BaseDamage;
        int extraDamageRoll = Rng.Next(0, 3);
        damage += extraDamageRoll;
        return damage;
    }

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
    
    public override void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
    }
    
    public void UseSpecialAttack(Player player, Enemy target)
    {
        target.TakeDamage(_playerClassPreset.SpecialAttack(player, target));
    }
}