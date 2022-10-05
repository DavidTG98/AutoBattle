using System.Collections.Generic;
using System;

namespace AutoBattle
{
    public sealed class Grid
    {
        public Dictionary<(int, int), GridBox> dicGrids;
        public int xLenght;
        public int yLength;

        public (int, int) GetRandomCoordenate() => (Helper.GetRandomInt(0, xLenght), Helper.GetRandomInt(0, yLength));

        public Grid(int Lines, int Columns)
        {
            dicGrids = new Dictionary<(int, int), GridBox>();

            xLenght = Lines;
            yLength = Columns;

            for (int i = 0; i < Lines; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    GridBox newBox = new GridBox(j, i, false, Columns * i + j);
                    (int, int) coordenate = (j, i);
                    dicGrids.Add(coordenate, newBox);
                }
            }

            Console.WriteLine("The battle field has been created\n");
        }

        public void SetGridOcupation((int, int) coordenate, bool isOcupied)
        {
            GridBox c = dicGrids[coordenate];
            c.SetOcupied(isOcupied);
            dicGrids[coordenate] = c;
        }

        public void DrawBattlefield()
        {
            for (int i = 0; i < xLenght; i++)
            {
                for (int j = 0; j < yLength; j++)
                {
                    //Console.Write($"[({i},{j}){GetGridChar()}]\t");
                    Console.Write($"[{GetGridChar()}]\t");

                    string GetGridChar() => dicGrids[(i, j)].IsOcupied ? "X" : " ";
                }

                Console.Write(Environment.NewLine + Environment.NewLine);
            }

            Console.Write(Environment.NewLine + Environment.NewLine);
        }
    }
}