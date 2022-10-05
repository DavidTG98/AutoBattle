using System;
using static AutoBattle.Character;
using static AutoBattle.Grid;
using System.Collections.Generic;
using System.Linq;
using static AutoBattle.Types;

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
    }
}