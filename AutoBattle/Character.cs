using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AutoBattle
{
    public class Character
    {
        private string _name;
        private float _health;
        private float _baseDamage;
        private float _damageMultiplier;
        private Character _target;
        private CharacterClass _characterClass;

        public GridBox _currentBox;

        public int PlayerIndex { get; private set; }

        public Character(string name, int index, float health, float baseDamage, float damageMultiplier, CharacterClass characterClass)
        {
            _name = name;
            _health = health;
            _baseDamage = baseDamage;
            _damageMultiplier = damageMultiplier;
            _characterClass = characterClass;

            PlayerIndex = index;
        }

        public void MoveTo(Grid grid, (int, int) coordenate, bool setPos = false)
        {
            if (setPos == false)
                grid.SetGridOcupation(_currentBox.GetCoordinates(), false);

            grid.SetGridOcupation(coordenate, true);
            _currentBox = grid.dicGrids[coordenate];
        }

        public void TakeDamage(float amount)
        {
            _health -= amount;

            if (_health <= 0)
                Die();
        }

        public bool IsDead => _health <= 0;
        public void SetCharacterTarget(Character target) => _target = target;
        public void Die()
        {
            Console.WriteLine($"{_name} has died");
        }

        public void StartTurn(Grid battlefield)
        {
            if (CheckCloseTargets(battlefield))
            {
                Attack(_target);
                return;
            }
            else
            {   // if there is no target close enough, calculates in wich direction this character should move to be closer to a possible target
                if (_currentBox.X_Index > _target._currentBox.X_Index)
                {
                    if (battlefield.grids.Exists(x => x.Index == _currentBox.Index - 1))
                    {
                        _currentBox.SetOcupied(false);
                        battlefield.grids[_currentBox.Index] = _currentBox;
                        _currentBox = battlefield.grids.Find(x => x.Index == _currentBox.Index - 1);
                        _currentBox.SetOcupied(true);
                        battlefield.grids[_currentBox.Index] = _currentBox;
                        Console.WriteLine($"{_name} walked left\n");
                        battlefield.DrawBattlefield();

                        return;
                    }
                }
                else if (_currentBox.X_Index < _target._currentBox.X_Index)
                {
                    _currentBox.SetOcupied(false);
                    battlefield.grids[_currentBox.Index] = _currentBox;
                    _currentBox = battlefield.grids.Find(x => x.Index == _currentBox.Index + 1);
                    _currentBox.SetOcupied(true);
                    battlefield.grids[_currentBox.Index] = _currentBox;
                    Console.WriteLine($"{_name} walked right\n");
                    battlefield.DrawBattlefield();
                    return;
                }

                if (_currentBox.Y_Index > _target._currentBox.Y_Index)
                {
                    battlefield.DrawBattlefield();
                    _currentBox.SetOcupied(false);
                    battlefield.grids[_currentBox.Index] = _currentBox;
                    _currentBox = battlefield.grids.Find(x => x.Index == _currentBox.Index - battlefield.xLenght);
                    _currentBox.SetOcupied(true);
                    battlefield.grids[_currentBox.Index] = _currentBox;
                    Console.WriteLine($"{_name} walked up\n");
                    return;
                }
                else if (_currentBox.Y_Index < _target._currentBox.Y_Index)
                {
                    _currentBox.SetOcupied(true);
                    battlefield.grids[_currentBox.Index] = _currentBox;
                    _currentBox = battlefield.grids.Find(x => x.Index == _currentBox.Index + battlefield.xLenght);
                    _currentBox.SetOcupied(false);
                    battlefield.grids[_currentBox.Index] = _currentBox;
                    Console.WriteLine($"{_name} walked down\n");
                    battlefield.DrawBattlefield();

                    return;
                }
            }
        }

        // Check in x and y directions if there is any character close enough to be a target.
        bool CheckCloseTargets(Grid battlefield)
        {
            bool left = battlefield.grids.Find(x => x.Index == _currentBox.Index - 1).IsOcupied;
            bool right = battlefield.grids.Find(x => x.Index == _currentBox.Index + 1).IsOcupied;
            bool up = battlefield.grids.Find(x => x.Index == _currentBox.Index + battlefield.xLenght).IsOcupied;
            bool down = battlefield.grids.Find(x => x.Index == _currentBox.Index - battlefield.xLenght).IsOcupied;

            return left & right & up & down;
        }

        public void Attack(Character target)
        {
            float damage = Helper.GetRandomInt(0, (int)(_baseDamage * _damageMultiplier));
            target.TakeDamage(damage);
            Console.WriteLine($"{_name} is attacking the player {_target.PlayerIndex} and did {damage} damage\n");
        }
    }
}
