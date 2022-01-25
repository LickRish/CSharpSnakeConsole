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
        public string symbol;
        public ConsoleColor color;

        public SnakePart(int x, int y, string tsymbol,ConsoleColor thisColor)
        {
            posX = x;
            posY = y;
            symbol = tsymbol;
            color = thisColor;
        }
    }

    class Food
    {
        public int posX;
        public int posY;
        Random randX = new Random();
        public ConsoleColor color;
        public void SetRandomPosition(int maxCols, int maxRows)
        {

            posX = randX.Next(1, maxCols-1);
            posY = randX.Next(1, maxRows-1);

            for (int i = Program.allParts.Count - 1; i > -1; i--)
            {
                if(posX == Program.allParts[i].posX && posY == Program.allParts[i].posY)
                {
                    SetRandomPosition(maxCols, maxRows);
                }
            }

            Console.BackgroundColor = ConsoleColor.Black;
            color = (ConsoleColor)randX.Next(0, 16);
            Console.ForegroundColor = color;
            Console.SetCursorPosition(posX, posY);
            Console.Write("■");
        }
    }
    class Program
    {
        static Timer gameLoopTimer = null;
        static bool paused = false;
        static bool changedDirection = false;
        //player variables
        public static List<SnakePart> allParts = new List<SnakePart>();
        static int xSpeed = 1;
        static int ySpeed = 0;
        static bool addPart = false;
        static bool waitForMove = false;
        static ConsoleColor addThisColor;
        //score variables
        static bool gameOver = false;
        static int score = 0;
        static int length = 0;
        static Food foodInstance;
        //map variables
        static string[,] mapArray;
        static int mapColumns = 100;
        static int mapRows = 25;

        static void Main(string[] args)
        {
            //SetUpGame();
            //Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
            foodInstance = new Food();
            CreatePlayer();
            CreateMap();
            DisplayScore();
            gameLoopTimer = new Timer(GameLoop, null, 0, 75);
            GetInput();


            Console.ReadLine();
        }


        static void SetUpGame()
        {
            Console.WriteLine("Enter Amount of Coloumns ");
            Console.ReadLine();
            Console.WriteLine("Enter Amount of Rows ");
            Console.ReadLine();
            Console.Clear();

        }

        static void GameLoop(Object o)
        {
            if (paused) { return; }
            if (CheckToChangeGameSpeed()) { return; }
            MovePlayer();
            CheckForGameOver();
        }

        public static bool CheckToChangeGameSpeed()
        {
            if(changedDirection == true)
            {
                if(xSpeed != 0)
                {
                    changedDirection = false;
                    gameLoopTimer.Change(0, 75);
                    return true;
                }
                if(ySpeed != 0)
                {
                    changedDirection = false;
                    gameLoopTimer.Change(0, 100);
                    return true;
                }
            }
            return false;
        }

        public static void GetInput()
        {
            while (gameOver == false)
            {
                if (waitForMove) { continue; }
                ConsoleKeyInfo key = new ConsoleKeyInfo();
                key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Spacebar)
                {
                    paused = !paused;
                    DisplayScore();
                }
                if (!paused)
                {
                    if (key.Key == ConsoleKey.W && ySpeed == 0)
                    {
                        xSpeed = 0;
                        ySpeed = -1;
                        allParts[0].symbol = "▀";
                        changedDirection = true;
                        waitForMove = true;
                    }
                    else if (key.Key == ConsoleKey.S && ySpeed == 0)
                    {
                        xSpeed = 0;
                        ySpeed = 1;
                        allParts[0].symbol = "▀";
                        changedDirection = true;
                        waitForMove = true;
                    }
                    else if (key.Key == ConsoleKey.D && xSpeed == 0)
                    {
                        xSpeed = 1;
                        ySpeed = 0;
                        allParts[0].symbol = "▐";
                        changedDirection = true;
                        waitForMove = true;
                    }
                    else if (key.Key == ConsoleKey.A && xSpeed == 0)
                    {
                        xSpeed = -1;
                        ySpeed = 0;
                        allParts[0].symbol = "▐";
                        changedDirection = true;
                        waitForMove = true;
                    }
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
                DisplayScore();
            }            
            if(allParts[0].posX == foodInstance.posX && allParts[0].posY == foodInstance.posY)
            {
                addThisColor = foodInstance.color;
                //Console.Beep(100, 200);
                foodInstance.SetRandomPosition(mapColumns, mapRows);
                score += 5;
                length++;
                addPart = true;
            }
            if(allParts.Count > 1)
            {
                for (int i = allParts.Count - 1; i > 0; i--)
                {
                    if(allParts[0].posX == allParts[i].posX && allParts[0].posY == allParts[i].posY)
                    {
                        gameOver = true;
                        gameLoopTimer.Dispose();
                        DisplayScore();
                        break;
                    }
                }
            }

            waitForMove = false;


        }

        static void MovePlayer()
        {
            Console.ForegroundColor = ConsoleColor.Black;


            int tempNewX = 0;
            int tempNewY = 0;
            if (!addPart)
            {
                Console.SetCursorPosition(allParts[allParts.Count - 1].posX, allParts[allParts.Count - 1].posY);
                if (allParts[allParts.Count- 1].posX % 2 == 0)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                }
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
                    allParts[i].symbol = allParts[i - 1].symbol;
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
                //▀ ▐ 
                Console.BackgroundColor = allParts[i].color;
                Console.Write(allParts[i].symbol);
            }

            if (addPart)
            {
                allParts.Add(new SnakePart(tempNewX, tempNewY,"o",addThisColor));
                DisplayScore();
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
            Console.SetCursorPosition(mapColumns + 3, (mapRows / 2) + 1);
            Console.Write($"Length : {length}");
            Console.SetCursorPosition(mapColumns + 3, (mapRows / 2) - 1);
            Console.Write($"      ");

            if (gameOver)
            {
                Console.SetCursorPosition(mapColumns + 3, (mapRows / 2) - 1);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"Game Over!");
            }
            if (paused)
            {
                Console.SetCursorPosition(mapColumns + 3, (mapRows / 2) - 1);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"Paused");
            }
        }

        static void CreatePlayer()
        {
            allParts.Add(new SnakePart(mapColumns/2, mapRows / 2, "▐",ConsoleColor.Red));

        }
        static void CreateMap()
        {
            mapArray = new string[mapRows, mapColumns];
            for (int row = 0; row < mapArray.GetLength(0); row++)
            {
                for (int col = 0; col < mapArray.GetLength(1); col++)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    mapArray[row, col] = GetMapPiece(row,col);
                    Console.Write(mapArray[row, col]);
                }

                Console.Write("\n");
            }
            Console.SetCursorPosition(mapColumns + 10, mapRows + 20);
            Console.Write("You found my secret!");
            Console.SetCursorPosition(mapColumns + 10, mapRows + 60);
            Console.Write("Stop going down..");
            Console.SetCursorPosition(0, 0);
            foodInstance.SetRandomPosition(mapColumns, mapRows);
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
                if(thisCol % 2 == 0)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                }
                return " ";
            }
        }

    }
}
