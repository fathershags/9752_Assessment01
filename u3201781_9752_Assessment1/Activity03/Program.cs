using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
/// <summary>
/// File:           Program.cs
/// Description:    A console application that allows the user to play one or more games of "Smileys and Grumpys"
/// Author:         Peter Maxwell
///	References:		https://ozh.github.io/ascii-tables/ used to generate ascii table art
/// Date:           2019/09/06
/// Version:        1.0
/// Notes:          Changed game symbols "smileys and grumpys" to "hearts and spades"
/// </summary>
namespace Activity03
{
    class Program
    {
        static void Main(string[] args)
        {

            #region Variable Setup
            //used to call custom methods
            SmileyAndGrumpy methods = new SmileyAndGrumpy();

            //how many rows/columns the grid will have
            int gridSize = 0;

            //how many turns the player gets to find all smileys
            int playerTurns = 6;

            //how many heart locations the player has correctly guessed
            int heartLocationsCorrectlyGuessed = 0;

            //2, 3 or 6 array size
            string[][] hiddenAnswers = new string[0][];
            string[][] currentlyDisplayedAnswers = new string[0][];

            //holds user input for use in IF statements
            string userInput = "";

            //used to control the main game loop
            bool activeGame = true;

            //tracks how many games the player has won/lost (0=games won, 1=games lost)
            int[] gamesWonAndLost = new int[2];
            #endregion

            while (activeGame)
            {
                //clears previous game field
                Console.Clear();
                //sets variables to default values
                userInput = "";
                gridSize = 0;

                //print welcome message and prompt to console
                Console.WriteLine("Welcome to Hearts and Spades!\n\nINSTRUCTIONS: Correctly guess the location of all the hearts before you run out of turns to win\n\nPlease choose a difficulty (easy/medium/hard) ==> ");

                Console.SetWindowSize(100, 50);

                while (gridSize == 0)
                {
                    //using user input, select a game difficulty
                    gridSize = int.Parse(methods.SelectGameDifficulty(Console.ReadLine()));
                }

                //set up player turn limit
                playerTurns = gridSize * 2;
                //generate the hidden smileys and grumpys array
                hiddenAnswers = methods.SmileyAndGrumpyGenerator(gridSize);
                //resizes array to correct size
                currentlyDisplayedAnswers = new string[hiddenAnswers.Length][];

                for (int i = 0; i < hiddenAnswers.Length; i++)
                {
                    //resizes array to correct size
                    currentlyDisplayedAnswers[i] = new string[gridSize];

                    for (int j = 0; j < hiddenAnswers[i].Length; j++)
                    {
                        //fills array with x's
                        currentlyDisplayedAnswers[i][j] = "x";
                    }
                }

                //draws full grid
                methods.DrawGrid(gridSize, currentlyDisplayedAnswers);

                while (playerTurns > 0)
                {
                    while (userInput == "")
                    {
                        userInput = methods.ValidatePlayerGuess(userInput, gridSize);
                    }

                    //if user has selected a hidden square
                    if (currentlyDisplayedAnswers[int.Parse(userInput[0].ToString())][int.Parse(userInput[2].ToString())] == "x")
                    {
                        //change value of square to show hidden value (heart/spade)
                        currentlyDisplayedAnswers[int.Parse(userInput[0].ToString())][int.Parse(userInput[2].ToString())] = hiddenAnswers[int.Parse(userInput[0].ToString())][int.Parse(userInput[2].ToString())];

                        if (hiddenAnswers[int.Parse(userInput[0].ToString())][int.Parse(userInput[2].ToString())] == @"♥")
                        {
                            heartLocationsCorrectlyGuessed++;
                        }

                        Console.Clear();

                        //draws full grid
                        methods.DrawGrid(gridSize, currentlyDisplayedAnswers);
                        //remove 1 turn from player turns left
                        playerTurns--;
                        //reset user input
                        userInput = "";
                    }
                    //user has selected a square that's already showing
                    else
                    {
                        Console.WriteLine("You've selected a square that's already been revealed, please choose a different square!");
                    }
                }

                if (heartLocationsCorrectlyGuessed >= gridSize)
                {
                    gamesWonAndLost[0]++;
                }
                else
                {
                    gamesWonAndLost[1]++;
                }

                Console.WriteLine("\nYou've run out of turns!\nYou guessed {0} of the {1} heart locations correctly\n\nGames won: {2}\nGames lost: {3}\n\nWould you like to play again?", heartLocationsCorrectlyGuessed, gridSize, gamesWonAndLost[0], gamesWonAndLost[1]);

                userInput = Console.ReadKey().KeyChar.ToString();

                if (userInput.Equals("n", StringComparison.InvariantCultureIgnoreCase))
                {
                    //exits game
                    activeGame = false;
                }
                else
                {
                    //continue playing
                    activeGame = true;
                }
            }
        }
    }

    class SmileyAndGrumpy
    {
        /// <summary>
        /// Verifies player has inputted a correctly formatted and valid guess
        /// </summary>
        public string ValidatePlayerGuess(string input, int squareGridSize)
        {
            Console.Write("\n\nPlease enter a row and column location to check for a heart or spade (e.g. 1,2) ==> ");
            input = Console.ReadLine();

            string pattern = @"[0-" + squareGridSize.ToString() + "],[0-" + squareGridSize.ToString() + "]";

            //if player input is digit,digit no greater than the grid size
            if (Regex.IsMatch(input, pattern))
            {
                return input;
            }
            else
            {
                Console.WriteLine("That's not a valid input.");
                return "";
            }
        }

        /// <summary>
        /// Prompts user to select difficulty and sets up game difficulty
        /// </summary>
        public string SelectGameDifficulty(string userInput)
        {
            //if user input equals "easy"
            if (userInput.Equals("easy", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.Clear();
                //set grid size
                return "3";
            }
            //if user input equals "medium"
            else if (userInput.Equals("medium", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.Clear();
                //set grid size
                return "6";
            }
            //if user input equals "hard"
            else if (userInput.Equals("hard", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.Clear();
                //set grid size
                return "9";
            }
            else
            {
                Console.WriteLine("That's not a valid difficulty.\n\nPlease choose easy, medium or hard difficulty ==> ");
                return "0";
            }
        }

        /// <summary>
        /// Creates a string-based table populated with hearts and spades
        /// </summary>
        public string[][] SmileyAndGrumpyGenerator(int squareGridSize)
        {
            //holds the grid array to be returned
            string[][] smileysAndGrumpys = new string[squareGridSize][];
            Random randomNumberGenerator = new Random();

            //correctly sizes grid array to hold all smileys and grumpys
            for (int i = 0; i < smileysAndGrumpys.Length; i++)
            {
                smileysAndGrumpys[i] = new string[squareGridSize];

                //fills entire grid array with grumpys
                for (int j = 0; j < smileysAndGrumpys[i].Length; j++)
                {
                    smileysAndGrumpys[i][j] = @"♠";
                }
            }

            //calculates how many smileys need to be included
            int smileysLeftToInput = (squareGridSize * squareGridSize) / 3;
            //holds all smiley grid positions
            string[] smileyPositions = new string[smileysLeftToInput];

            for (int i = 0; i < smileyPositions.Length; i++)
            {
                //generates a double-digit value used for smileys column and row positions
                smileyPositions[i] = randomNumberGenerator.Next(0, squareGridSize).ToString() + randomNumberGenerator.Next(0, squareGridSize).ToString();

                for (int j = 0; j < smileyPositions.Length; j++)
                {
                    //generates new smiley position until it doesn't match any existing smiley positions
                    while (smileyPositions[i] == smileyPositions[j] && i != j)
                    {
                        smileyPositions[i] = randomNumberGenerator.Next(0, squareGridSize).ToString() + randomNumberGenerator.Next(0, squareGridSize).ToString();
                    }
                }

                //adds smiley to the grid array at specified column/row position
                smileysAndGrumpys[smileyPositions[i][0] - '0'][smileyPositions[i][1] - '0'] = @"♥";
            }

            return smileysAndGrumpys;
        }

        /// <summary>
        /// Draws a horizontal table border using ascii art. Input a left, middle and right border piece and how many columns to draw
        /// </summary>
        public void DrawHorizontalGridBorder(string leftBorder, string middleBorder, string rightBorder, int numberOfGridSquares)
        {
            Console.Write(leftBorder);

            for (int i = 0; i < numberOfGridSquares; i++)
            {
                Console.Write(middleBorder);
            }

            Console.Write(rightBorder);
        }

        /// <summary>
        /// Draws a grid table with column and row headers, with gridToDraw as the grid square contents
        /// </summary>
        public void DrawGrid(int squareGridSize, string[][] gridToDraw)
        {
            #region Variables
            //holds pieces used for building the top of the game field (left border, middle border, right border)
            string[] gameFieldTop = { @"//===", @"[]=====", @"\\",
@"||   ", @"||  {0}  ", @"||",
@"|]===[]=====[]=====[]=====[|" };
            //holds pieces used for building bottom of the game field (left border, middle border, right border
            string[] gameFieldBottom = { @"\\===", @"[]=====", @"//" };

            string gameFieldRowHeader = "|| {0} ||  ";
            string gameFieldBodyRow = "{0}  ||  ";
            #endregion

            //draw top grid border ascii art
            DrawHorizontalGridBorder(gameFieldTop[0], gameFieldTop[1], gameFieldTop[2], squareGridSize);
            Console.WriteLine();
            for (int i = 0; i < 1; i++)
            {
                //writes column header with appropriate number
                Console.Write(gameFieldRowHeader, " ");

                for (int j = 0; j < squareGridSize; j++)
                {
                    //draws column headers with correct number labels
                    Console.Write(gameFieldBodyRow, j);
                }

                Console.WriteLine();
            }

            //for every row
            for (int i = 0; i < squareGridSize; i++)
            {
                //writes row header with appropriate number
                Console.Write(gameFieldRowHeader, i);

                //for every column
                for (int j = 0; j < squareGridSize; j++)
                {
                    //draws row filled with x's
                    Console.Write(gameFieldBodyRow, gridToDraw[i][j]);
                }
                //goes to new line
                Console.WriteLine();
            }

            //draw bottom grid border ascii art
            DrawHorizontalGridBorder(gameFieldBottom[0], gameFieldBottom[1], gameFieldBottom[2], squareGridSize);
        }
    }
}
