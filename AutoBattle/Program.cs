using System;
using System.Collections.Generic;

namespace AutoBattle
{
    sealed class Program
    {
        private static void Main(string[] args)
        {
            Grid grid = new Grid(5, 7);
            List<Character> AllPlayers = new List<Character>();
            Action ShowAllPlayersStats = delegate { };

            bool isOver = false;
            int currentTurn = 0;
            int playerClassChoice = Helper.GetValidPlayerClassChoice();

            TeamManager.OnBattleIsOver += () => { isOver = true; };

            //CREATE CHARACTERS!
            Console.WriteLine();
            CreatePlayers();

            if (TeamManager.CheckWOWin())
            {
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine($"TEAM {TeamManager.GetRemainingTeam()} WIN FOR W.O.");
                return;
            }

            TeamManager.SetTargets();

            Console.WriteLine();
            grid.DrawBattlefield();

            UpdateTurn();

            void UpdateTurn()
            {
                Console.WriteLine("\nClick on any key to start the next turn...\n");

                ConsoleKeyInfo key = Console.ReadKey();
                currentTurn++;

                foreach (Character c in AllPlayers)
                {
                    c.DoAction(grid);

                    if (isOver)
                        break;
                }


                if (isOver)
                {
                    Console.WriteLine("-------------------------------------------");
                    Console.WriteLine($"TEAM {TeamManager.GetRemainingTeam()} WIN THE GAME!!!");
                    return;
                }

                Console.WriteLine($"\nSTATS ====== TURN: {currentTurn}\n");
                ShowAllPlayersStats?.Invoke();
                UpdateTurn();
            }

            void CreatePlayers()
            {
                CreateCharacter(new Character("Player_01", 'P', health: 100, baseDamage: 22, damageMultiplier: 1, (CharacterClass)playerClassChoice, Team.Letter));
                CreateCharacter(new Character("Player_02", 'J', health: 100, baseDamage: 22, damageMultiplier: 1, Helper.GetRandomClass(), Team.Letter));
                CreateCharacter(new Character("Player_03", 'K', health: 100, baseDamage: 22, damageMultiplier: 1, Helper.GetRandomClass(), Team.Letter));

                CreateCharacter(new Character("Enemy_01", '5', health: 125, baseDamage: 15, damageMultiplier: 1, Helper.GetRandomClass(), Team.Number));
                CreateCharacter(new Character("Enemy_02", '6', health: 125, baseDamage: 15, damageMultiplier: 1, Helper.GetRandomClass(), Team.Number));

                CreateCharacter(new Character("Minion_01", '$', health: 25, baseDamage: 10, damageMultiplier: 2, Helper.GetRandomClass(), Team.Simbol));
                CreateCharacter(new Character("Minion_02", '#', health: 25, baseDamage: 10, damageMultiplier: 2, Helper.GetRandomClass(), Team.Simbol));


                void CreateCharacter(Character character)
                {
                    if (AllPlayers.Count == grid.xLenght * grid.yLength)
                    {
                        Console.WriteLine("THE BATTLEFIELD IS ALREADY FULL");
                        return;
                    }

                    AllPlayers.Add(character);
                    ShowAllPlayersStats += character.ShowStats;
                    character.OnDeath += OnCharacterDie;

                    TeamManager.AddCharacterToTeam(character);

                    //Set Character to a random position
                    while (TrySetCharacterPosition(character, grid.GetRandomCoordenate()) == false) { }

                    Console.WriteLine($"{character.Name}({character.Simbol}) / Class Choice: {character.Class}");
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

                character.MoveTo(grid, position, true);
                return true;
            }

            void OnCharacterDie(Character character)
            {
                grid.SetGridOcupation(character.CurrentBox.Coordinates, false);
                TeamManager.RemovePlayerFromTeam(character);
            }
        }
    }
}