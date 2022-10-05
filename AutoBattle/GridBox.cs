namespace AutoBattle
{
    public struct GridBox
    {
        public int xIndex;
        public int yIndex;
        public bool ocupied;
        public int Index;

        public GridBox(int x, int y, bool ocupied, int index)
        {
            xIndex = x;
            yIndex = y;
            this.ocupied = ocupied;
            Index = index;
        }

    }
}