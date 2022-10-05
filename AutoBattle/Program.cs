using System;
using static AutoBattle.Character;
using static AutoBattle.Grid;
using System.Collections.Generic;
using System.Linq;

namespace AutoBattle
{
    class Program
    {
        private static void Main(string[] args)
        {
            Grid grid = new Grid(5, 5);
            GridBox EnemyCurrentLocation;
            List<Character> AllPlayers = new List<Character>();

            int currentTurn = 0;
            int numberOfPossibleTiles = grid.grids.Count;

            //CREATE CHARACTERS!
            int playerClassChoice = GetValidPlayerClassChoice();

            Character PlayerCharacter = CreateCharacter("Player", 100, 20, 1, (CharacterClass)playerClassChoice);
            Character EnemyCharacter = CreateCharacter("Enemy", 100, 20, 1, Helper.GetRandomClass());

            StartGame();

            int GetValidPlayerClassChoice()
            {
                //asks for the player to choose between for possible classes via console.
                Console.WriteLine("Choose Between One of this Classes:\n");

                //Write all classes
                string[] classes = Enum.GetNames(typeof(CharacterClass));
                for (int i = 0; i < classes.Length; i++)
                {
                    Console.Write($"[{i + 1}]{classes[i]} ");
                }
                Console.WriteLine();

                int choice = int.Parse(Console.ReadLine());
                while (choice <= 0 || choice > Enum.GetNames(typeof(CharacterClass)).Length)
                {
                    Console.WriteLine();
                    choice = GetValidPlayerClassChoice();
                }

                return choice;
            }
            Character CreateCharacter(string name, float health, float baseDamage, float damageMultiplier, CharacterClass characterClass)
            {
                Console.WriteLine($"{name} Class Choice: {characterClass}");
                Character character = new Character(name, AllPlayers.Count, health, baseDamage, damageMultiplier, characterClass);
                AllPlayers.Add(character);
                return character;
            }

            void StartGame()
            {
                //populates the character variables and targets
                EnemyCharacter.SetCharacterTarget(PlayerCharacter);
                PlayerCharacter.SetCharacterTarget(EnemyCharacter);

                AlocatePlayerCharacter();
                StartTurn();
            }

            void StartTurn()
            {
                if (currentTurn == 0)
                {
                    //AllPlayers.Sort();  
                }

                foreach (Character character in AllPlayers)
                {
                    character.StartTurn(grid);
                }

                currentTurn++;
                HandleTurn();
            }

            void HandleTurn()
            {
                if (PlayerCharacter.IsDead)
                {
                    return;
                }
                else if (EnemyCharacter.IsDead)
                {
                    Console.Write(Environment.NewLine + Environment.NewLine);

                    // endgame?

                    Console.Write(Environment.NewLine + Environment.NewLine);

                    return;
                }
                else
                {
                    Console.Write(Environment.NewLine + Environment.NewLine);
                    Console.WriteLine("Click on any key to start the next turn...\n");
                    Console.Write(Environment.NewLine + Environment.NewLine);

                    ConsoleKeyInfo key = Console.ReadKey();
                    StartTurn();
                }
            }

            void AlocatePlayerCharacter()
            {
                int random = 0;
                GridBox RandomLocation = (grid.grids.ElementAt(random));
                Console.Write($"{random}\n");
                if (!RandomLocation.ocupied)
                {
                    GridBox PlayerCurrentLocation = RandomLocation;
                    RandomLocation.ocupied = true;
                    grid.grids[random] = RandomLocation;
                    PlayerCharacter._currentBox = grid.grids[random];
                    AlocateEnemyCharacter();
                }
                else
                {
                    AlocatePlayerCharacter();
                }
            }

            void AlocateEnemyCharacter()
            {
                int random = Helper.GetRandomInt(0, grid.grids.Count - 1);
                GridBox RandomLocation = grid.grids.ElementAt(random);
                Console.Write($"{random}\n");
                if (!RandomLocation.ocupied)
                {
                    EnemyCurrentLocation = RandomLocation;
                    RandomLocation.ocupied = true;
                    grid.grids[random] = RandomLocation;
                    EnemyCharacter._currentBox = grid.grids[random];
                    grid.DrawBattlefield();
                }
                else
                {
                    AlocateEnemyCharacter();
                }
            }
        }
    }
}