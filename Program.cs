﻿using System;
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
        static Timer gameLoopTimer = null;
        //player variables
        static List<SnakePart> allParts = new List<SnakePart>();
        static int xSpeed = 1;
        static int ySpeed = 0;
        static bool addPart = false;
        //score variables
        static bool gameOver = false;
        static int score = 0;
        //map variables
        static string[,] mapArray;
        static int mapColumns = 50;
        static int mapRows = 25;

        static void Main(string[] args)
        {
            //Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
            CreatePlayer();
            CreateMap();
            DisplayScore();
            gameLoopTimer = new Timer(GameLoop, null, 0, 100);
            GetInput();


            Console.ReadLine();
        }

        static void GameLoop(Object o)
        {
            MovePlayer();
            CheckForGameOver();
        }

        public static void GetInput()
        {
            while (gameOver == false)
            {
                ConsoleKeyInfo key = new ConsoleKeyInfo();
                key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.W && ySpeed == 0)
                {
                    xSpeed = 0;
                    ySpeed = -1;
                    gameLoopTimer.Change(0, 200);
                }
                else if (key.Key == ConsoleKey.S && ySpeed == 0)
                {
                    xSpeed = 0;
                    ySpeed = 1;
                    gameLoopTimer.Change(0, 200);
                }
                else if (key.Key == ConsoleKey.D && xSpeed == 0)
                {
                    xSpeed = 1;
                    ySpeed = 0;
                 
                    gameLoopTimer.Change(0, 100);
                }
                else if (key.Key == ConsoleKey.A && xSpeed == 0)
                {
                    xSpeed = -1;
                    ySpeed = 0;
                    gameLoopTimer.Change(0, 100);
                }
                if(key.Key == ConsoleKey.Spacebar)
                {
                    score += 1;
                    DisplayScore();
                    addPart = true;
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

            int tempNewX = 0;
            int tempNewY = 0;
            if (!addPart)
            {
                Console.SetCursorPosition(allParts[allParts.Count - 1].posX, allParts[allParts.Count - 1].posY);
                Console.Write(" ");
            }
            else
            {
                tempNewX = allParts[allParts.Count - 1].posX;
                tempNewY = allParts[allParts.Count - 1].posY;
            }


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

            if (addPart)
            {
                allParts.Add(new SnakePart(tempNewX, tempNewY));
                addPart = false;
            }

        }

        //call this at start, and only when collecting food
        static void DisplayScore()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(mapColumns + 3, mapRows / 2);
            Console.Write($"Score : {score}");
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