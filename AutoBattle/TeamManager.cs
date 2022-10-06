using System.Collections.Generic;
using System;
using System.Linq;

namespace AutoBattle
{
    static class TeamManager
    {
        public static readonly Dictionary<Team, List<Character>> _teams = new Dictionary<Team, List<Character>>();
        public static event Action<Team> OnBattleIsOver = delegate { };

        public static void Init()
        {
            foreach (Team t in Enum.GetValues(typeof(Team)))
            {
                _teams.Add(t, new List<Character>());
            }
        }

        public static void AddCharacterToTeam(Character character, Team team)
        {
            _teams[team].Add(character);
        }

        public static void SetTargets()
        {
            foreach (Team type in Enum.GetValues(typeof(Team)))
            {
                foreach (KeyValuePair<Team, List<Character>> team in _teams)
                {
                    if (team.Key != type)
                    {
                        foreach (Character c in _teams[type])
                        {
                            foreach (Character enemy in team.Value)
                            {
                                c.AddTarget(enemy);
                                //Console.WriteLine($"{c.Name} Add {enemy.Name} as Target");
                            }
                        }
                    }
                }
            }
        }

        public static void RemovePlayerFromTeam(Character character)
        {
            _teams[character.Team].Remove(character);

            if (_teams[character.Team].Count == 0)
            {
                EliminateTeam(character.Team);
            }
        }

        private static void EliminateTeam(Team team)
        {
            Console.WriteLine($"TEAM {team} WAS ELIMINATED!!!");

            _teams.Remove(team);

            if (_teams.Count == 1)
            {
                OnBattleIsOver?.Invoke(_teams.Keys.First());
            }
        }
    }
}