using System;
using System.Collections.Generic;
using System.Text;

namespace OBP200_RolePlayingGame;


class Program
{
    // ======= Globalt tillstånd  =======
    
    //player objekt, hanterar all/det mesta data om spelaren
    static Player _player = new Player();
    
    //en lista med olika typer av loot man kan få från kistor
    static List<Loot> _chestLootTable = new List<Loot>();
    
    //enemy objekt, hanterar all/det mesta data om fiender
    static Enemy _enemy = new Enemy();
    
    // Rum: [type, label]
    // types: battle, treasure, shop, rest, boss
    static List<string[]> Rooms = new List<string[]>();

    // Fiendemallar, 
    static List<IEnemyTypePresets> _enemyTemplates = new();
    
    //en lista med mallar för olika spelarklasser. 
    private static List<IPlayerRolePresets> _playerTemplates = new();
    
    // Status för kartan
    static int CurrentRoomIndex = 0;

    // Random
    static Random Rng = new Random();

    // ======= Main =======

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        InitaliseEnemyTemplates();
        InitalisePlayerTemplates();
        CreateChestLootTable();
        while (true)
        {
            ShowMainMenu();
            Console.Write("Välj: ");
            var choice = (Console.ReadLine() ?? "").Trim();
            
            
            if (choice == "1")
            {
                StartNewGame();
                RunGameLoop();
            }
            else if (choice == "2")
            {
                Console.WriteLine("Avslutar...");
                return;
            }
            else
            {
                Console.WriteLine("Ogiltigt val.");
            }

            Console.WriteLine();
        }
    }

    // ======= Meny & Init =======

    static void ShowMainMenu()
    {
        Console.WriteLine("=== Text-RPG ===");
        Console.WriteLine("1. Nytt spel");
        Console.WriteLine("2. Avsluta");
    }

    static void StartNewGame()
    {
        Console.Write("Ange namn: ");
        var name = (Console.ReadLine() ?? "").Trim();
        if (string.IsNullOrWhiteSpace(name)) name = "Namnlös";
        
        Console.WriteLine("Välj klass: 0) Warrior  1) Mage  2) Rouge");
        Console.Write("Val: ");
        var k = (Console.ReadLine() ?? "").Trim();
        //förhindrar värden som inte är int
        int classChoice = AtemptToParseInt(k, 0);
        
        //Mall för spelarens klass väljs baserad på värdet av inmatning. 
        int playerTemplateListLength = _playerTemplates.Count - 1;
        if (classChoice > playerTemplateListLength || classChoice<0)  //deafultar till en klass om instoppade värdet inte finns som ett index
        {
            _player.GeneratePlayer(_playerTemplates[0], name);
        }
        else
        {
            _player.GeneratePlayer(_playerTemplates[classChoice], name);
        }
        
        // Initiera karta (linjärt äventyr)
        Rooms.Clear();
        Rooms.Add(new[] { "battle", "Skogsstig" });
        Rooms.Add(new[] { "treasure", "Gammal kista" });
        Rooms.Add(new[] { "shop", "Vandrande köpman" });
        Rooms.Add(new[] { "battle", "Grottans mynning" });
        Rooms.Add(new[] { "rest", "Lägereld" });
        Rooms.Add(new[] { "battle", "Grottans djup" });
        Rooms.Add(new[] { "boss", "Urdraken" });

        CurrentRoomIndex = 0;
    }

    static void RunGameLoop()
    {
        while (true)
        {
            var room = Rooms[CurrentRoomIndex];
            Console.WriteLine($"--- Rum {CurrentRoomIndex + 1}/{Rooms.Count}: {room[1]} ({room[0]}) ---");

            bool continueAdventure = EnterRoom(room[0]);
            
            if (_player.CheckIfDead())
            {
                Console.WriteLine("Du har stupat... Spelet över.");
                break;
            }
            
            if (!continueAdventure)
            {
                Console.WriteLine("Du lämnar äventyret för nu.");
                break;
            }

            CurrentRoomIndex++;
            
            if (CurrentRoomIndex >= Rooms.Count)
            {
                Console.WriteLine();
                Console.WriteLine("Du har klarat äventyret!");
                break;
            }
            
            Console.WriteLine();
            Console.WriteLine("[C] Fortsätt     [Q] Avsluta till huvudmeny");
            Console.Write("Val: ");
            var post = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();

            if (post == "Q")
            {
                Console.WriteLine("Tillbaka till huvudmenyn.");
                break;
            }

            Console.WriteLine();
        }
    }

    // ======= Rumshantering =======

    static bool EnterRoom(string type)
    {
        switch ((type ?? "battle").Trim())
        {
            case "battle":
                return DoBattle(isBoss: false);
            case "boss":
                return DoBattle(isBoss: true);
            case "treasure":
                return DoTreasure();
            case "shop":
                return DoShop();
            case "rest":
                return DoRest();
            default:
                Console.WriteLine("Du vandrar vidare...");
                return true;
        }
    }

    // ======= Strid =======

    static bool DoBattle(bool isBoss)
    {
        CreateEnemy(isBoss);

        while (!_enemy.CheckIfDead() && !_player.CheckIfDead())
        {
            Console.WriteLine();
            _player.ShowStatus();
            _enemy.ShowStatus();
            Console.WriteLine("[A] Attack   [X] Special   [P] Dryck   [R] Fly");
            Console.Write("Val: ");
            
            var cmd = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();

            if (cmd == "A")
            {
                _enemy.TakeDamage(_player.CalculateDamage(_enemy.Defence));
            }
            else if (cmd == "X")
            { 
                int specialDamage=_player.UseSpecialAttack(_enemy.Defence, _enemy.IsBoss);
                _enemy.TakeDamage(specialDamage);
            }
            else if (cmd == "P")
            {
                _player.DrinkPotion();
            }
            else if (cmd == "R" && !isBoss)
            {
                if (_player.TryRunAway())
                {
                    Console.WriteLine("Du flydde!");
                    return true; // fortsätt äventyr
                }
                Console.WriteLine("Misslyckad flykt!");
                
            }
            else
            {
                Console.WriteLine("Du tvekar...");
            }

            if (_enemy.CheckIfDead()) break;

            // Fiendens tur
            _player.TakeDamage(_enemy.CalculateDamage(_player.Defence));
        }

        if (_player.CheckIfDead())
        {
            return false;
        }

        // Vinstrapporter, XP, guld, loot
        _player.AddPlayerExperience(_enemy.ExperienceReward);
        _player.AddPlayerGold(_enemy.GoldReward);

        Console.WriteLine($"Seger! +{_enemy.ExperienceReward} XP, +{_enemy.GoldReward} guld.");
        _player.AddLoot(_enemy.MaybeDropLoot());
        return true;
    }
    
    //skapar en fiende utifrån ett template
    static void CreateEnemy(bool isBoss)
    {
        if (isBoss)
        {
            // Genererar fienden och sätter värden utifrån en boss-mall istället för vanlig fiende
            _enemy.GenerateEnemy(new Dragon());
        }
        else
        {
            // Slumpa bland templates
            var template = _enemyTemplates[Rng.Next(_enemyTemplates.Count)];
            
            // Genererar fienden och sätter värden
            _enemy.GenerateEnemy(template);
        }
    }
    
    //Lägger till templates för fiender i listan för att senare slumpa fram en av dem vid strid
    static void InitaliseEnemyTemplates()
    {
        _enemyTemplates.Add(new Skeleton());
        _enemyTemplates.Add(new Bandit());
        _enemyTemplates.Add(new Beast());
        _enemyTemplates.Add(new Slime());
    }
    
    

    

    // ======= Rumshändelser =======

    static bool DoTreasure()
    {
        Console.WriteLine("Du hittar en gammal kista...");
        if (Rng.NextDouble() < 0.5)
        {
            int gold = Rng.Next(8, 15);
            _player.AddPlayerGold(gold);
            Console.WriteLine($"Kistan innehåller {gold} guld!");
        }
        else
        {
            
            Loot found = _chestLootTable[Rng.Next(_chestLootTable.Count)];
            _player.AddLoot(found);
            Console.WriteLine($"Du plockar upp: {found.Name}");
        }
        return true;
    }
    
    static bool DoShop()
    {
        Console.WriteLine("En vandrande köpman erbjuder sina varor:");
        while (true)
        {
            _player.PrintShopRelevantStats();
            Console.WriteLine("1) Köp dryck (10 guld)");
            Console.WriteLine("2) Köp vapen (+2 ATK) (25 guld)");
            Console.WriteLine("3) Köp rustning (+2 DEF) (25 guld)");
            Console.WriteLine("4) Sälj alla 'Minor Gem' (+5 guld/st)");
            Console.WriteLine("5) Lämna butiken");
            Console.Write("Val: ");
            var val = (Console.ReadLine() ?? "").Trim();

            if (val == "1")
            {
                _player.AttemptToBuy(10, 1, 1);
            }
            else if (val == "2")
            {
                _player.AttemptToBuy(25, 3, 2);
            }
            else if (val == "3")
            {
                _player.AttemptToBuy(25, 3, 2);
            }
            else if (val == "4")
            {
                _player.SellLoot("Minor Gem");
            }
            else if (val == "5")
            {
                Console.WriteLine("Du säger adjö till köpmannen.");
                break;
            }
            else
            {
                Console.WriteLine("Köpmannen förstår inte ditt val.");
            }
        }
        return true;
    }

    
    
    static bool DoRest()
    {
        Console.WriteLine("Du slår läger och vilar.");
        _player.HealThroughRest();
        Console.WriteLine("HP återställt till max.");
        return true;
    }

    
    
    static void InitalisePlayerTemplates()
    {
        _playerTemplates.Add(new Warrior());
        _playerTemplates.Add(new Mage());
        _playerTemplates.Add(new Rouge());
    }
    
    static int AtemptToParseInt(string s, int fallback)
    {
        try
        {
            int value = Convert.ToInt32(s);
            return value;
        }
        catch (Exception e)
        {
            return fallback;
        }
    }
    
    //skapar ett "loot table" med olika typer av loot man kan få från en kista och ger dem värde
    static void CreateChestLootTable()
    {
        _chestLootTable.Add(new Loot("Iron Dagger", 5));
        _chestLootTable.Add(new Loot("Oak Staff", 3));
        _chestLootTable.Add(new Loot("Leather Vest", 4));
        _chestLootTable.Add(new Loot("Healing Herb", 2));
    }
}
