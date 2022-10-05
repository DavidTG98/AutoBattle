using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AutoBattle
{
    public class Character
    {
        private int _health;
        private int _baseDamage;
        private int _damageMultiplier;
        private Character _target;
        private CharacterClass _characterClass;

        public GridBox _currentBox;

        public bool IsDead => _health <= 0;
        public void SetCharacterTarget(Character target) => _target = target;

        public string Name { get; private set; }
        public int PlayerIndex { get; private set; }

        public Character(string name, int index, int health, int baseDamage, int damageMultiplier, CharacterClass characterClass)
        {
            Name = name;
            _health = health;
            _baseDamage = baseDamage;
            _damageMultiplier = damageMultiplier;
            _characterClass = characterClass;

            PlayerIndex = index;
        }

        public bool MoveTo(Grid grid, (int, int) coordenate, bool setPos = false)
        {
            if (setPos == false)
                grid.SetGridOcupation(_currentBox.GetCoordinates(), false);


            if (grid.dicGrids[coordenate].IsOcupied || coordenate.Item1 > grid.xLenght || coordenate.Item2 > grid.yLength)
            {
                Console.WriteLine($"{Name} cannot move to ({coordenate.Item1},{coordenate.Item2})");
                return false;
            }
            else
            {
                Console.WriteLine($"{Name} move to ({coordenate.Item1},{coordenate.Item2})");
                grid.SetGridOcupation(coordenate, true);
                _currentBox = grid.dicGrids[coordenate];
                return true;
            }
        }

        public void TakeDamage(int amount)
        {
            _health -= amount;

            if (_health <= 0)
                Die();
        }

        public void Die()
        {
            Console.WriteLine($"{Name} has died");
        }

        public void DoAction(Grid grid)
        {
            if (HasCloseTargets(grid))
            {
                Attack(_target);
            }
            else
            {
                MoveTowardsTarget(grid);
            }

            grid.DrawBattlefield();
        }

        private void MoveTowardsTarget(Grid grid)
        {
            var trgC = _target._currentBox.GetCoordinates();
            var desiredC = _currentBox.GetCoordinates();

            if (desiredC.Item1 != trgC.Item1)
                desiredC.Item1 += (desiredC.Item1 > trgC.Item1) ? -1 : 1;
            else
                desiredC.Item2 += (desiredC.Item2 > trgC.Item2) ? -1 : 1;

            MoveTo(grid, desiredC);
        }

        private bool HasCloseTargets(Grid grid)
        {
            var myCoordenate = _currentBox.GetCoordinates();

            if (grid.dicGrids.TryGetValue((myCoordenate.Item1 + 1, myCoordenate.Item2), out GridBox gridBox))
                if (gridBox.IsOcupied)
                    return true;

            if (grid.dicGrids.TryGetValue((myCoordenate.Item1 - 1, myCoordenate.Item2), out gridBox))
                if (gridBox.IsOcupied)
                    return true;

            if (grid.dicGrids.TryGetValue((myCoordenate.Item1, myCoordenate.Item2 + 1), out gridBox))
                if (gridBox.IsOcupied)
                    return true;

            if (grid.dicGrids.TryGetValue((myCoordenate.Item1, myCoordenate.Item2 - 1), out gridBox))
                if (gridBox.IsOcupied)
                    return true;

            return false;
        }

        public void Attack(Character target)
        {
            int damage = Helper.GetRandomInt(0, _baseDamage * _damageMultiplier);
            target.TakeDamage(damage);
            Console.WriteLine($"{Name} is attacking {_target.Name} and did {damage} damage\n");
        }
    }
}
