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
            List<Character> AllPlayers = new List<Character>();

            int currentTurn = 0;
            //int numberOfPossibleTiles = grid.grids.Count;

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
                    Console.Write($"[{i + 1}]{classes[i]} ");

                Console.WriteLine();

                if (int.TryParse(Console.ReadLine(), out int choice) == false)
                    Redo();

                while (choice <= 0 || choice > Enum.GetNames(typeof(CharacterClass)).Length)
                    Redo();

                void Redo()
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
            bool TrySetCharacterPosition(Character character, (int, int) position)
            {
                if (grid.dicGrids[position].IsOcupied)
                    return false;

                character.MoveTo(grid, position, true);
                return true;
            }

            void StartGame()
            {
                EnemyCharacter.SetCharacterTarget(PlayerCharacter);
                PlayerCharacter.SetCharacterTarget(EnemyCharacter);

                //Set Characters position
                TrySetCharacterPosition(PlayerCharacter, (0, 0));
                while (TrySetCharacterPosition(EnemyCharacter, grid.GetRandomCoordenate()) == false) { }

                grid.DrawBattlefield();

                StartTurn();
            }

            void StartTurn()
            {
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
                    Console.WriteLine("Player Character is dead");
                    Console.Write(Environment.NewLine + Environment.NewLine);
                    return;
                }
                else if (EnemyCharacter.IsDead)
                {
                    Console.WriteLine("Enemy Character is dead");
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
        }
    }
}