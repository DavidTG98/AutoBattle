using System.Collections.Generic;
using System;
using System.Linq;

namespace AutoBattle
{
    static class TeamManager
    {
        private static readonly Dictionary<Team, List<Character>> _teams = new Dictionary<Team, List<Character>>();
        public static event Action OnBattleIsOver = delegate { };

        public static void AddCharacterToTeam(Character character)
        {
            if (_teams.ContainsKey(character.Team) == false)
                _teams.Add(character.Team, new List<Character>());

            _teams[character.Team].Add(character);
        }
        public static bool CheckWOWin() => _teams.Keys.Count <= 1;
        public static Team GetRemainingTeam() => _teams.Keys.First();

        public static void SetTargets()
        {
            foreach (Team type in Enum.GetValues(typeof(Team)))
            {
                if (_teams.ContainsKey(type) == false)
                    continue;

                foreach (KeyValuePair<Team, List<Character>> team in _teams)
                {
                    if (team.Key != type)
                    {
                        foreach (Character c in _teams[type])
                        {
                            foreach (Character enemy in team.Value)
                            {
                                c.AddTarget(enemy);
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
                OnBattleIsOver?.Invoke();
            }
        }
    }
}