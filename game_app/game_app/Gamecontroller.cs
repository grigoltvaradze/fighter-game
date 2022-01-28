using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_app
{
    internal static class Gamecontroller
    {

        public static int PathLenght { get; set; }
        public static int MeleeEnemyCount { get; set; }
        public static int RangedEnemyCount { get; set; }
        public static int HealSpotCount { get; set; }
        public static Dictionary<int, Character> Enemies { get; set; }
        public static int[] HealthPoints { get; set; }

        private static readonly List<int> _usedLocatios = new List<int>();

        public static void SetPathLenght()
        {
            Console.WriteLine("Enter Path lenght between 30 - 150 : ");
            while (true)
            {
                string enterednumber = Console.ReadLine();
                if (int.TryParse(enterednumber, out int pathlenght))
                {
                    if (pathlenght >= 30 && pathlenght <= 150)
                    {
                        PathLenght = pathlenght;
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Enter numbers between 30 -150 !!!");
                    }
                }
                else
                {
                    Console.WriteLine("enter information about path lenght!!!");
                }
            }
        }
        public static void CreatePlayer()
        {
            Console.WriteLine("Choose your charachter name: ");
            string playerName = Console.ReadLine() ?? "Warrior";
            player = new Player(playerName);
        }
        public static void SetNumberOfRangedEnemys()
        {
            Console.WriteLine("Enter amount  of  ranged enemys between 1 - 3 ");
            while (true)
            {
                string enteredAmountOfEnemyes = Console.ReadLine();
                if (int.TryParse(enteredAmountOfEnemyes, out int enemyAmount))
                {
                    if (enemyAmount >= 1 && enemyAmount <= 3)
                    {
                        RangedEnemyCount = enemyAmount;
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Enter numbers between 1-3 !!!");
                    }
                }
                else
                {
                    Console.WriteLine("enter information about ranged enemy amount");
                }
            }
        }
        public static void SetNumberOfMeleeEnemys()
        {
            Console.WriteLine("Enter amount  of  melee enemys between 1 - 4 ");
            while (true)
            {
                string enteredAmountOfEnemyes = Console.ReadLine();
                if (int.TryParse(enteredAmountOfEnemyes, out int enemyAmount))
                {
                    if (enemyAmount >= 1 && enemyAmount <= 4)
                    {
                        MeleeEnemyCount = enemyAmount;
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Enter numbers between 1-4 !!!");
                    }
                }
                else
                {
                    Console.WriteLine("enter information about melee enemy amount");
                }
            }
        }
        public static void SetNumberOfHealthPoints()
        {
            Console.WriteLine("Enter amount  of health regeneration points between 1- 4: ");
            while (true)
            {
                string enteredAmountOfhealth = Console.ReadLine();
                if (int.TryParse(enteredAmountOfhealth, out int healthAmount))
                {
                    if (healthAmount >= 1 && healthAmount <= 4)
                    {
                        HealSpotCount = healthAmount;
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Enter numbers between 1-4 !!!");
                    }
                }
                else
                {
                    Console.WriteLine("enter information about health regeneration points");
                }
            }
        }
        public static void CreateEnemies()
        {
            Enemies = new Dictionary<int, Character>(RangedEnemyCount + MeleeEnemyCount);
            int enemyLocation;

            for (int i = 0; i < MeleeEnemyCount; i++)
            {
                enemyLocation = GetLocationForEnemy();
                Enemies.Add(enemyLocation, new Melee(enemyLocation));
            }
            for (int i = MeleeEnemyCount; i < RangedEnemyCount + MeleeEnemyCount; i++)
            {
                enemyLocation = GetLocationForEnemy();
                Enemies.Add(enemyLocation, new Ranged(enemyLocation));
            }
        }

        private static int GetLocationForEnemy()
        {
            Random r = new Random();
            int i;
            do
            {
                i = r.Next(2, PathLenght - 2);
            }
            while (_usedLocatios.Contains(i) || _usedLocatios.Contains(i - 1) || _usedLocatios.Contains(i + 1));
            _usedLocatios.Add(i);
            return i;
        }
        public static void CreateHealthPoints()
        {
            HealthPoints = new int[HealSpotCount];
            Random r = new Random();
            int healthpoint;
            for (int j = 0; j < HealSpotCount; j++)
            {
                do
                {
                    healthpoint = r.Next(2, PathLenght - 2);
                }
                while (_usedLocatios.Contains(healthpoint));
                _usedLocatios.Add(healthpoint);
                HealthPoints[j] = healthpoint;
            }
        }

        public static Player player { get; set; }
        public static void InitializeGame()
        {
            SetPathLenght();
            SetNumberOfRangedEnemys();
            SetNumberOfMeleeEnemys();
            SetNumberOfHealthPoints();
        }
        public static void BuildGame()
        {
            CreatePlayer();
            CreateEnemies();
            CreateHealthPoints();
        }


        public static void StartGame()
        {
            var nearestEnemy = GetNearestEnemy(player.CurrentLocation);

            for (int i = 0; i < PathLenght; i++)
            {
                Console.WriteLine(player.Name + " is on position " + (i+1));

                if (HealthPoints.Any(x => x == player.CurrentLocation))
                    player.Heal();

                if (nearestEnemy == null)
                {
                    player.Move();
                    continue;
                }

                bool enemyCanAttack = nearestEnemy.AttRange >= nearestEnemy.CurrentLocation - player.CurrentLocation;
                bool playerCanAttack = player.AttRange >= nearestEnemy.CurrentLocation - player.CurrentLocation;

                if (enemyCanAttack || playerCanAttack)
                {
                    do
                    {
                        if (playerCanAttack)
                        {
                            nearestEnemy.TakeDamage(player.CountDamage());

                            if (!nearestEnemy.IsAlive)
                            {
                                Console.WriteLine("You killed enemy!");
                                break;
                            }
                        }
                        if (enemyCanAttack)
                        {
                            player.TakeDamage(nearestEnemy);
                            if (!player.IsAlive)
                            {
                                Console.WriteLine("Game over...");
                                return;
                            }
                        }
                    }
                    while (player.CurrentLocation + 1 == nearestEnemy.CurrentLocation);
                }

                player.Move();

                if (!nearestEnemy.IsAlive)
                    nearestEnemy = GetNearestEnemy(player.CurrentLocation);
            }

            Console.WriteLine("Congratulations! You won the game!");
        }

        private static Character GetNearestEnemy(int playersLocation)
        {
            var nextEnemyLocations = Enemies.Keys.Where(x => x > playersLocation);

            if (nextEnemyLocations.Count() > 0)
            {
                var nearestLocation = nextEnemyLocations.Min();

                return Enemies[nearestLocation];
            }
            return null;
        }
    }
}
