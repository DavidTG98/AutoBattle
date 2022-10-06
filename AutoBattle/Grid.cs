using System.Collections.Generic;
using System;

namespace AutoBattle
{
    public sealed class Grid
    {
        public readonly Dictionary<(int, int), GridBox> dicGrids = new Dictionary<(int, int), GridBox>();
        public int xLenght;
        public int yLength;

        public (int, int) GetRandomCoordenate() => (Helper.GetRandomInt(0, xLenght), Helper.GetRandomInt(0, yLength));

        public Grid(int Lines, int Columns)
        {
            xLenght = Math.Max(1, Lines);
            yLength = Math.Max(1, Columns);

            for (int x = 0; x < Lines; x++)
            {
                for (int y = 0; y < Columns; y++)
                {
                    GridBox newBox = new GridBox(x, y);
                    (int, int) coordenate = (x, y);
                    dicGrids.Add(coordenate, newBox);
                }
            }

            Console.WriteLine("The battle field has been created\n");
        }

        public void SetGridOcupation((int, int) coordenate, bool isOcupied, char simbol = ' ')
        {
            GridBox c = dicGrids[coordenate];
            c.SetOcupied(isOcupied, simbol);
            dicGrids[coordenate] = c;
        }

        //Retorna falso caso a coordenada esteja fora dos limites do grid
        public bool Exists((int, int) coordenate)
        {
            if (coordenate.Item1 > xLenght || coordenate.Item2 > yLength || coordenate.Item1 < 0 || coordenate.Item2 < 0)
                return false;

            return true;
        }

        public void DrawBattlefield()
        {
            for (int i = 0; i < xLenght; i++)
            {
                for (int j = 0; j < yLength; j++)
                {
                    //Console.Write($"[({i},{j})]\t");
                    Console.Write($"[{dicGrids[(i, j)].Simbol}]");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}