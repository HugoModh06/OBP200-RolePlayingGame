using System;
using System.Collections.Generic;
using System.Text;

namespace OBP200_RolePlayingGame;


class Program
{
    // ======= Globalt tillstånd  =======
    
    //player objekt, hanterar all/det mesta data om spelaren
    static readonly Player Player = new Player();
    
    //en lista med olika typer av loot man kan få från kistor
    static readonly List<Loot> ChestLootTable = new List<Loot>();
    
    //enemy objekt, hanterar all/det mesta data om fiender
    static readonly Enemy Enemy = new Enemy();
    
    // Rum: [type, label]
    // types: battle, treasure, shop, rest, boss
    static readonly List<string[]> Rooms = new List<string[]>();

    // Fiendemallar, 
    static readonly List<IEnemyTypePreset> EnemyTemplates = new();
    
    //en lista med mallar för olika spelarklasser. 
    private static readonly List<IPlayerRolePreset> PlayerTemplates = new();
    
    // Status för kartan
    static int _currentRoomIndex;

    // Random
    static readonly Random Rng = new Random();

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
    
    static void InitalisePlayerTemplates()
    {
        PlayerTemplates.Add(new Warrior());
        PlayerTemplates.Add(new Mage());
        PlayerTemplates.Add(new Rouge());
    }
    
    //skapar ett "loot table" med olika typer av loot man kan få från en kista och ger dem värde
    static void CreateChestLootTable()
    {
        ChestLootTable.Add(new Loot("Iron Dagger", 5));
        ChestLootTable.Add(new Loot("Oak Staff", 3));
        ChestLootTable.Add(new Loot("Leather Vest", 4));
        ChestLootTable.Add(new Loot("Healing Herb", 2));
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
        int playerTemplateListLength = PlayerTemplates.Count - 1;
        if (classChoice > playerTemplateListLength || classChoice<0)  //deafultar till en klass om instoppade värdet inte finns som ett index
        {
            Player.GeneratePlayer(PlayerTemplates[0], name);
        }
        else
        {
            Player.GeneratePlayer(PlayerTemplates[classChoice], name);
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

        _currentRoomIndex = 0;
    }

    static void RunGameLoop()
    {
        while (true)
        {
            var room = Rooms[_currentRoomIndex];
            Console.WriteLine($"--- Rum {_currentRoomIndex + 1}/{Rooms.Count}: {room[1]} ({room[0]}) ---");

            bool continueAdventure = EnterRoom(room[0]);
            
            if (Player.CheckIfDead())
            {
                Console.WriteLine("Du har stupat... Spelet över.");
                break;
            }
            
            if (!continueAdventure)
            {
                Console.WriteLine("Du lämnar äventyret för nu.");
                break;
            }

            _currentRoomIndex++;
            
            if (_currentRoomIndex >= Rooms.Count)
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

        while (!Enemy.CheckIfDead() && !Player.CheckIfDead())
        {
            Console.WriteLine();
            Player.ShowStatus();
            Enemy.ShowStatus();
            Console.WriteLine("[A] Attack   [X] Special   [P] Dryck   [R] Fly");
            Console.Write("Val: ");
            
            var cmd = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();

            if (cmd == "A")
            {
                Enemy.TakeDamage(Player.CalculateDamage(Enemy.Defence));
            }
            else if (cmd == "X")
            { 
                int specialDamage=Player.UseSpecialAttack(Enemy.Defence, Enemy.IsBoss);
                Enemy.TakeDamage(specialDamage);
            }
            else if (cmd == "P")
            {
                Player.DrinkPotion();
            }
            else if (cmd == "R" && !isBoss)
            {
                if (Player.TryRunAway())
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

            if (Enemy.CheckIfDead()) break;

            // Fiendens tur
            Player.TakeDamage(Enemy.CalculateDamage(Player.Defence));
        }

        if (Player.CheckIfDead())
        {
            return false;
        }

        // Vinstrapporter, XP, guld, loot
        Player.AddPlayerExperience(Enemy.ExperienceReward);
        Player.AddPlayerGold(Enemy.GoldReward);

        Console.WriteLine($"Seger! +{Enemy.ExperienceReward} XP, +{Enemy.GoldReward} guld.");
        Player.AddLoot(Enemy.MaybeDropLoot());
        return true;
    }
    
    //skapar en fiende utifrån ett template
    static void CreateEnemy(bool isBoss)
    {
        if (isBoss)
        {
            // Genererar fienden och sätter värden utifrån en boss-mall istället för vanlig fiende
            Enemy.GenerateEnemy(new Dragon());
        }
        else
        {
            // Slumpa bland templates
            var template = EnemyTemplates[Rng.Next(EnemyTemplates.Count)];
            
            // Genererar fienden och sätter värden
            Enemy.GenerateEnemy(template);
        }
    }
    
    //Lägger till templates för fiender i listan för att senare slumpa fram en av dem vid strid
    static void InitaliseEnemyTemplates()
    {
        EnemyTemplates.Add(new Skeleton());
        EnemyTemplates.Add(new Bandit());
        EnemyTemplates.Add(new Beast());
        EnemyTemplates.Add(new Slime());
    }
    

    // ======= Rumshändelser =======

    static bool DoTreasure()
    {
        Console.WriteLine("Du hittar en gammal kista...");
        if (Rng.NextDouble() < 0.5)
        {
            int gold = Rng.Next(8, 15);
            Player.AddPlayerGold(gold);
            Console.WriteLine($"Kistan innehåller {gold} guld!");
        }
        else
        {
            
            Loot found = ChestLootTable[Rng.Next(ChestLootTable.Count)];
            Player.AddLoot(found);
            Console.WriteLine($"Du plockar upp: {found.Name}");
        }
        return true;
    }
    
    static bool DoShop()
    {
        Console.WriteLine("En vandrande köpman erbjuder sina varor:");
        while (true)
        {
            Player.PrintShopRelevantStats();
            Console.WriteLine("1) Köp dryck (10 guld)");
            Console.WriteLine("2) Köp vapen (+2 ATK) (25 guld)");
            Console.WriteLine("3) Köp rustning (+2 DEF) (25 guld)");
            Console.WriteLine("4) Sälj alla 'Minor Gem' (+5 guld/st)");
            Console.WriteLine("5) Lämna butiken");
            Console.Write("Val: ");
            var val = (Console.ReadLine() ?? "").Trim();

            if (val == "1")
            {
                Player.AttemptToBuy(10, 1, 1);
            }
            else if (val == "2")
            {
                Player.AttemptToBuy(25, 3, 2);
            }
            else if (val == "3")
            {
                Player.AttemptToBuy(25, 3, 2);
            }
            else if (val == "4")
            {
                Player.SellLoot("Minor Gem");
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
        Player.HealThroughRest();
        Console.WriteLine("HP återställt till max.");
        return true;
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
    
    
}
