using System;

namespace AutoBattle
{
    public struct GridBox
    {
        public int X_Index { get; private set; }
        public int Y_Index { get; private set; }
        public int Index { get; private set; }

        public bool IsOcupied { get; private set; }
        public (int, int) GetCoordinates() => (X_Index, Y_Index);

        public GridBox(int x, int y, bool isOcupied, int index)
        {
            X_Index = x;
            Y_Index = y;
            Index = index;
            IsOcupied = isOcupied;
        }

        public void SetOcupied(bool isOcupied)
        {
            //Console.WriteLine($"{X_Index},{Y_Index} ({Index}) has set occupation to {isOcupied}");
            IsOcupied = isOcupied;
        }
    }
}