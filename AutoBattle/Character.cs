using System;
using System.Collections.Generic;

namespace AutoBattle
{
    public class Character
    {
        //Variables
        private int _baseDamage;
        private int _damageMultiplier;

        private readonly List<Character> _targets = new List<Character>();
        private Character _closestTarget;
        public event Action<Character> OnDeath = delegate { };
        private bool isDead;

        //Properties
        public GridBox CurrentBox { get; private set; }
        public string Name { get; private set; }
        public int Health { get; private set; }
        public char Simbol { get; private set; }
        public Team Team { get; private set; }
        public CharacterClass Class { get; private set; }

        public void AddTarget(Character target) => _targets.Add(target);

        public Character(string name, char simbol, int health, int baseDamage, int damageMultiplier, CharacterClass characterClass, Team team)
        {
            Name = name;
            Health = health;
            Simbol = simbol;
            Team = team;
            Class = characterClass;

            _baseDamage = baseDamage;
            _damageMultiplier = damageMultiplier;

            HandleClassChoice();
        }

        public bool MoveTo(Grid grid, (int, int) coordenate, bool setPos = false)
        {
            if (setPos == false && grid.dicGrids[coordenate].IsOcupied == false)
                grid.SetGridOcupation(CurrentBox.Coordinates, false);


            if (grid.Exists(coordenate) == false)
            {
                Console.WriteLine($"({coordenate.Item1},{coordenate.Item2}) doesnt exist.");
                return false;
            }
            else if (grid.dicGrids[coordenate].IsOcupied)
            {
                Console.WriteLine($"({coordenate.Item1},{coordenate.Item2}) is already ocupied");
                return false;
            }
            else
            {
                if (setPos == false)
                    Console.WriteLine($"{Name} move to ({coordenate.Item1},{coordenate.Item2})");

                grid.SetGridOcupation(coordenate, true, Simbol);
                CurrentBox = grid.dicGrids[coordenate];
                return true;
            }
        }

        public void TakeDamage(int amount)
        {
            Health -= amount;

            if (Health <= 0)
                Die();
        }

        private void Die()
        {
            isDead = true;
            Health = 0;

            Console.WriteLine($"{Name}({Simbol}) has died. Bravely");
            OnDeath?.Invoke(this);
        }

        public void DoAction(Grid grid)
        {
            if (isDead)
                return;

            Console.WriteLine("-------------------------");
            Console.WriteLine($"{Name}({Simbol}) phase!");

            GetClosestTarget();

            //If the distance between this character and his target is less or equal one, he is able to attack
            if (CurrentBox.GetDistanceToOtherBox(_closestTarget.CurrentBox) <= 1)
                Attack(_closestTarget);
            else
                TryMoveTowardsPosition(grid, _closestTarget.CurrentBox);

            grid.DrawBattlefield();
        }

        private void TryMoveTowardsPosition(Grid grid, GridBox gridBox)
        {
            var trgC = gridBox.Coordinates;
            var desiredC = CurrentBox.Coordinates;

            if (desiredC.Item1 != trgC.Item1)
                desiredC.Item1 += (desiredC.Item1 > trgC.Item1) ? -1 : 1;
            else
                desiredC.Item2 += (desiredC.Item2 > trgC.Item2) ? -1 : 1;

            MoveTo(grid, desiredC);
        }

        private void GetClosestTarget()
        {
            int distance = int.MaxValue;

            for (int i = 0; i < _targets.Count; i++)
            {
                if (_targets[i].isDead)
                    continue;

                int localDist = CurrentBox.GetDistanceToOtherBox(_targets[i].CurrentBox);

                if (localDist < distance)
                {
                    _closestTarget = _targets[i];
                    distance = localDist;
                }
            }
        }

        public void Attack(Character target)
        {
            int damage = Helper.GetRandomInt(1, _baseDamage * _damageMultiplier);
            Console.WriteLine($"{Name}({Simbol}) attacked {target.Name}({target.Simbol}) and did {damage} damage\n");
            target.TakeDamage(damage);
        }

        public void ShowStats()
        {
            if (isDead)
            {
                Console.WriteLine($"{Name} is Dead");
                return;
            }

            Console.WriteLine($"{Name}({Simbol}) / HP:{Health}");
        }

        public void HandleClassChoice()
        {
            switch (Class)
            {
                case CharacterClass.Warrior:
                    Health += 20;
                    break;
                case CharacterClass.Paladin:
                    _damageMultiplier += 1;
                    break;
                case CharacterClass.Cleric:
                    Health += 5;
                    _baseDamage += 10;
                    break;
                case CharacterClass.Archer:
                    _baseDamage += 20;
                    break;

                default:
                    break;
            }
        }
    }
}