using System;
using System.Collections.Generic;

namespace AutoBattle
{
    class Program
    {
        private static void Main(string[] args)
        {
            Grid grid = new Grid(3, 10);
            List<Character> AllPlayers = new List<Character>();
            Action ShowAllPlayersStats = delegate { };
            Action<Grid> DoPlayersAction = delegate { };

            bool isOver = false;
            Team winnerTeam = Team.Letter;
            int currentTurn = 0;
            int playerClassChoice = Helper.GetValidPlayerClassChoice();

            TeamManager.Init();
            TeamManager.OnBattleIsOver += TeamManager_OnBattleIsOver;

            //CREATE CHARACTERS!
            Console.WriteLine();
            CreatePlayers();

            TeamManager.SetTargets();

            Console.WriteLine();
            grid.DrawBattlefield();

            UpdateTurn();

            void UpdateTurn()
            {
                Console.Write("-------------------------------------------");
                Console.WriteLine($"Turn {currentTurn} has started");

                DoPlayersAction?.Invoke(grid);

                Console.WriteLine();
                Console.WriteLine("STATS");
                ShowAllPlayersStats?.Invoke();

                if (isOver)
                {
                    Console.WriteLine("-------------------------------------------");
                    Console.WriteLine($"TEAM {winnerTeam} WIN THE GAME!!!");
                    return;
                }

                currentTurn++;

                Console.WriteLine("\nClick on any key to start the next turn...\n");

                ConsoleKeyInfo key = Console.ReadKey();
                UpdateTurn();
            }


            void CreatePlayers()
            {
                CreateCharacter(new Character("Player_01", 'P', 0, health: 100, baseDamage: 22, damageMultiplier: 1, (CharacterClass)playerClassChoice, Team.Letter));
                CreateCharacter(new Character("Player_02", 'J', 1, health: 100, baseDamage: 22, damageMultiplier: 1, Helper.GetRandomClass(), Team.Letter));
                CreateCharacter(new Character("Player_03", 'K', 1, health: 100, baseDamage: 22, damageMultiplier: 1, Helper.GetRandomClass(), Team.Letter));

                CreateCharacter(new Character("Enemy_01", '5', 1, health: 125, baseDamage: 15, damageMultiplier: 1, Helper.GetRandomClass(), Team.Number));
                CreateCharacter(new Character("Enemy_02", '6', 1, health: 125, baseDamage: 15, damageMultiplier: 1, Helper.GetRandomClass(), Team.Number));

                CreateCharacter(new Character("Minion_01", '$', 1, health: 25, baseDamage: 10, damageMultiplier: 2, Helper.GetRandomClass(), Team.Simbol));
                CreateCharacter(new Character("Minion_02", '#', 1, health: 25, baseDamage: 10, damageMultiplier: 2, Helper.GetRandomClass(), Team.Simbol));


                void CreateCharacter(Character character)
                {
                    AllPlayers.Add(character);
                    ShowAllPlayersStats += character.ShowStats;
                    DoPlayersAction += character.DoAction;
                    character.OnDeath += OnCharacterDie;

                    TeamManager.AddCharacterToTeam(character, character.Team);

                    //Set Character to a random position
                    while (TrySetCharacterPosition(character, grid.GetRandomCoordenate()) == false) { }
                }
            }

            bool TrySetCharacterPosition(Character character, (int, int) position)
            {
                if (grid.Exists(position) == false)
                {
                    Console.WriteLine($"{position} is not a valid position");
                    return false;
                }

                if (grid.dicGrids[position].IsOcupied)
                    return false;

                //Console.WriteLine($"{character.Name} spawn at ({position.Item1},{position.Item2})");
                character.MoveTo(grid, position, true);
                return true;
            }

            void OnCharacterDie(Character character)
            {
                grid.SetGridOcupation(character.CurrentBox.Coordinates, false);
                TeamManager.RemovePlayerFromTeam(character);
            }

            void TeamManager_OnBattleIsOver(Team team)
            {
                winnerTeam = team;
                isOver = true;
            }
        }
    }
}