using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace SnakeV2
{
    class SnakePart
    {
        public int posX;
        public int posY;

        public SnakePart(int x, int y)
        {
            posX = x;
            posY = y;
        }
    }
    class Program
    {
        static Thread getInputThread;
        static Timer gameLoopTimer = null;
        //player variables
        static List<SnakePart> allParts = new List<SnakePart>();
        static int xSpeed = 1;
        static int ySpeed = 0;
        //score variables
        static bool gameOver = false;
        static int score = 0;
        static float time = 0;
        static float addTime = 0.1f;
        //map variables
        static string[,] mapArray;
        static int mapColumns = 50;
        static int mapRows = 25;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
            CreatePlayer();
            CreateMap();



            getInputThread = new Thread(new ThreadStart(GetInput));
            getInputThread.Start();

            gameLoopTimer = new Timer(GameLoop, null, 0, 100);

            Console.ReadLine();
        }

        static void GameLoop(Object o)
        {
            MovePlayer();
            DisplayScoreTime();
            CheckForGameOver();
        }

        public static void GetInput()
        {
            while (gameOver == false)
            {
                ConsoleKeyInfo key = new ConsoleKeyInfo();
                key = Console.ReadKey(true);

                if (key.KeyChar == 'w')
                {
                    score = 1;
                }
                if (key.KeyChar == 's')
                {
                    score = 2;
                }
                if (key.KeyChar == 'd')
                {
                    score = 3;
                }
                if (key.KeyChar == 'a')
                {
                    score = 4;
                }
            }

        }


        static void CheckForGameOver()
        {
            //checks if snake hit edge of map
            if (allParts[0].posX == mapColumns - 1 || allParts[0].posX == 0 || allParts[0].posY == mapRows - 1 || allParts[0].posY == 0)
            {
                gameOver = true;
                gameLoopTimer.Dispose();
            }
        }

        static void MovePlayer()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.BackgroundColor = ConsoleColor.White;
            Console.SetCursorPosition(allParts[allParts.Count - 1].posX, allParts[allParts.Count - 1].posY);
            Console.Write(" ");

            //move snake
            for (int i = allParts.Count - 1; i > -1; i--)
            {
                if(i != 0)
                {
                    allParts[i].posX = allParts[i - 1].posX;
                    allParts[i].posY = allParts[i - 1].posY;
                }
                else
                {
                    allParts[0].posX += xSpeed;
                    allParts[0].posY += ySpeed;
                }
            }


            //draw snake
            for (int i = allParts.Count - 1; i > -1; i--)
            {
                Console.SetCursorPosition(allParts[i].posX, allParts[i].posY);
                Console.Write("■");
            }
            ////swap last and first
            //SnakePart tmp = allParts[0];
            //allParts[0] = allParts[allParts.Count - 1];
            //allParts[allParts.Count - 1] = tmp;


            //Console.ForegroundColor = ConsoleColor.Blue;
            //Console.BackgroundColor = ConsoleColor.White;
            ////grab part at end of list and set its pos to next pos
            //Console.SetCursorPosition(allParts[allParts.Count - 1].posX, allParts[allParts.Count - 1].posY);
            //Console.Write(" ");
            //allParts[allParts.Count - 1].posX = allParts[0].posX + xSpeed;
            //allParts[allParts.Count - 1].posY = allParts[0].posY + ySpeed;
            
            //Console.SetCursorPosition(allParts[allParts.Count - 1].posX, allParts[allParts.Count - 1].posY);
            //Console.Write("O");
        }
        static void DisplayScoreTime()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(mapColumns + 3, mapRows / 2);
            Console.Write($"Score : {score}");
            Console.SetCursorPosition(mapColumns + 3, (mapRows / 2) - 1);
            time += addTime;
            string timeString = time.ToString("0.0");
            Console.Write($"Time : {timeString}");
        }

        static void CreatePlayer()
        {
            allParts.Add(new SnakePart(7, 12));

        }
        static void CreateMap()
        {
            mapArray = new string[mapRows, mapColumns];
            for (int row = 0; row < mapArray.GetLength(0); row++)
            {
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
