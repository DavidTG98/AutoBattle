using System;

namespace AutoBattle
{
    public static class Helper
    {
        private static readonly Random random = new Random();

        public static int GetRandomInt(int min = 0, int max = int.MaxValue) => random.Next(min, max);

        public static CharacterClass GetRandomClass()
        {
            return (CharacterClass)Enum.GetValues(typeof(CharacterClass)).GetValue(random.Next(Enum.GetValues(typeof(CharacterClass)).Length));
        }

        public static int GetValidPlayerClassChoice()
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
    }
}