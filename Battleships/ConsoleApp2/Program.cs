using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Battleships\n" +
                "Pick a sea size:");
            int seaSize = int.Parse(Console.ReadLine());
            char[,] sea = new char[seaSize, seaSize];

            //Real matrix is filled with " " the "~" is just for looks
            for (int rows = 0; rows < sea.GetLength(0); rows++)
            {
                for (int cols = 0; cols < sea.GetLength(1); cols++)
                {
                    sea[rows, cols] = ' ';
                    Console.Write("~ ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("Pick how many ships you want:");
            int shipCount = int.Parse(Console.ReadLine());
            while (shipCount > seaSize)
            {
                Console.WriteLine("Can't have more ships than the sea, pick again");
                shipCount = int.Parse(Console.ReadLine());
            }
            sea = GenerateShips(sea, shipCount);
           
            int turnCount = 0;
            while (shipCount > 0)
            {
                Console.WriteLine("Choose a coordinate to strike, example: 0 0");
                int[] coords = Console.ReadLine().Split().Select(int.Parse).ToArray();
                
                int rowStrike = coords[0];
                int colStrike = coords[1];
                if (rowStrike >= sea.GetLength(0) || colStrike >= sea.GetLength(1))
                {
                    Console.WriteLine("Invalid coordinates, pick again");
                    continue;
                }
                string result = Fire(sea, rowStrike, colStrike);
                Console.WriteLine(result);
                if (result.Contains("Sank"))
                { shipCount--; }
                turnCount++;
                PrintSea(sea);
            }
            Console.WriteLine($"You won! It took you {turnCount} turns");
        }
        static char[,] GenerateShips(char[,] sea, int shipCount)
        {
            var rand = new Random();

            for (int i = 0; i < shipCount; i++)
            {
                // Picks which direction to generate ship, length and start coordinates
                int direction = rand.Next(0, 2);
                int curShipLen = rand.Next(1, 3);
                int rowCoord = rand.Next(0, sea.GetLength(0) - 1);
                int colCoord = rand.Next(0, sea.GetLength(1) - 1);

                for (int j = 0; j < curShipLen; j++)
                {
                    sea[colCoord, rowCoord] = (char)i;
                    if (direction == 0)
                    {
                        //Checks if the next ship symbol goes outside the array or meets an existing ship
                        //in which case it stops making the ship.
                        if (colCoord + 1 > sea.GetLength(0) - 1 || sea[colCoord + 1, rowCoord] != ' ')
                        { break; }
                        colCoord = colCoord + 1;
                        sea[colCoord, rowCoord] = (char)i; //Makes the ship unique by giving it it's own char.
                    }
                    else
                    {
                        if (rowCoord + 1 > sea.GetLength(0) - 1 || sea[colCoord, rowCoord + 1] != ' ')
                        { break; }
                        rowCoord = rowCoord + 1;
                        sea[colCoord, rowCoord] = (char)i;
                    }

                }
            }
            return sea;
        }
        static string Fire(char[,] sea, int rowStrike, int colStrike)
        {
           
            string result = string.Empty;
            if (sea[rowStrike, colStrike] != '^' && sea[rowStrike, colStrike] != '*' && sea[rowStrike, colStrike] != ' ')
            {
                //Verifies that an unhit ship is at this coord and sets it to hit.               
                char currentShipSymbol = sea[rowStrike, colStrike];
                sea[rowStrike, colStrike] = '*';
                //Checks if there are any of the current ship symbol left in the 2d array, "Hit" if yes,"Sank" if not.
                bool hasSymbol = false;
                for (int row = 0; row < sea.GetLength(0); row++)
                {
                    for (int col = 0; col < sea.GetLength(1); col++)
                    {
                        if (sea[row, col] == currentShipSymbol)
                        {
                            hasSymbol = true;
                        }

                    }
                }
                if (hasSymbol == false)
                { result = $"Sank ship: {currentShipSymbol}!"; }
                else
                { result = "Hit!";}

            }// ^ is bombed water, * is bombed ship.
            else if (sea[rowStrike, colStrike] == '^' || sea[rowStrike, colStrike] == '*')
            {
                result = "You've already bombed this spot."; 
            }
            else
            {               
                result = "Miss!";
                sea[rowStrike, colStrike] = '^';
            }

            return result;
        }
        static void PrintSea(char[,] sea)
        {
            
            for (int rows = 0; rows < sea.GetLength(0); rows++)
            {
                for (int cols = 0; cols < sea.GetLength(1); cols++)
                {
                    // Prints only the coordinates that have been bombed with their symbol
                    // if it's a ship or water symbol prints ~
                    if (sea[rows, cols] == '*' || sea[rows, cols] == '^')
                    { Console.Write(sea[rows, cols] + " ");}
                    else
                    { Console.Write("~ "); }
                }
                Console.WriteLine();
            }
        }       
    }
}
