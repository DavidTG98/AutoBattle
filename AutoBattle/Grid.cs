using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AutoBattle
{
    public partial class Grid
    {
        public List<GridBox> grids = new List<GridBox>();
        public int xLenght;
        public int yLength;

        public Grid(int Lines, int Columns)
        {
            xLenght = Lines;
            yLength = Columns;

            for (int i = 0; i < Lines; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    GridBox newBox = new GridBox(j, i, false, (Columns * i + j));
                    grids.Add(newBox);
                }
            }

            Console.WriteLine("The battle field has been created\n");
        }

        // prints the matrix that indicates the tiles of the battlefield
        public void DrawBattlefield()
        {
            for (int i = 0; i < xLenght; i++)
            {
                for (int j = 0; j < yLength; j++)
                {
                    GridBox currentgrid = new GridBox();
                    Console.Write($"[{GetGridChar()}]\t");
                    string GetGridChar() => currentgrid.ocupied ? "X" : " ";
                }

                Console.Write(Environment.NewLine + Environment.NewLine);
            }

            Console.Write(Environment.NewLine + Environment.NewLine);
        }
    }
}