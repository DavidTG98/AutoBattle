using System;

namespace AutoBattle
{
    public struct GridBox
    {
        public bool IsOcupied { get; private set; }
        public (int, int) Coordinates { get; private set; }
        public char Simbol { get; private set; }

        public GridBox(int x, int y)
        {
            Coordinates = (x, y);
            IsOcupied = false;
            Simbol = ' ';
        }

        public void SetOcupied(bool isOcupied, char simbol = ' ')
        {
            IsOcupied = isOcupied;
            Simbol = IsOcupied ? simbol : ' ';
        }

        public int GetDistanceToOtherBox(GridBox otherPos)
        {
            return Math.Abs(Coordinates.Item1 - otherPos.Coordinates.Item1) + Math.Abs(Coordinates.Item2 - otherPos.Coordinates.Item2);
        }
    }
}