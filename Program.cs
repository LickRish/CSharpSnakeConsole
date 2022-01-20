using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeV2
{
    class SnakePart
    {
        int posX;
        int posY;

        public SnakePart(int x, int y)
        {
            posX = x;
            posY = y;
        }
    }
    class Program
    {
        static string[,] mapArray;
        static int mapColumns = 50;
        static int mapRows = 25;

        static void Main(string[] args)
        {
            CreateMap();
            Console.ReadLine();

        }

        static void CreateMap()
        {
            mapArray = new string[mapRows, mapColumns];
            for (int row = 0; row < mapArray.GetLength(0); row++)
            {
                Console.Write(" ");
                for (int col = 0; col < mapArray.GetLength(1); col++)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    mapArray[row, col] = GetMapPiece(row,col);
                    Console.Write(mapArray[row, col]);
                }

                Console.Write("\n");
            }
        }

        static string GetMapPiece(int thisRow, int thisCol)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            if (thisRow == 0 && thisCol == 0)
            {
                return "╔";
            }
            else if (thisRow == 0 && thisCol == mapArray.GetLength(1) - 1)
            {
                return "╗";
            }
            else if (thisRow == 0 && thisCol != 0)
            {
                return "═";
            }
            else if (thisRow == mapArray.GetLength(0) - 1 && thisCol == 0)
            {
                return "╚";
            }
            else if (thisRow == mapArray.GetLength(0) - 1 && thisCol == mapArray.GetLength(1) - 1)
            {       
                return "╝";
            }
            else if (thisRow == mapArray.GetLength(0) - 1 && thisCol != 0)
            {
                return "═";
            }
            else if (thisRow != 0 && thisCol == 0)
            {
                return "║";
            }
            else if (thisRow != 0 && thisCol == mapArray.GetLength(1) - 1)
            {
                return "║";
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.White;
                return " ";
            }
        }

    }
}
