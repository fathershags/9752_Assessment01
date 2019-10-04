using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// File:           Program.cs
/// Description:    A console application that allows the user to play one or more games of "Random Racer"
/// Author:         Peter Maxwell
/// Date:           2019/09/16
/// Version:        1.0
/// </summary>
namespace Activity04
{
    class Program
    {
        static void Main(string[] args)
        {            
            //used to access methods class
            RandomRacer methods = new RandomRacer();

            //strings used to display racetracks in the console window
            string[] computerTrack = new string[50];
            string[] playerTrack = new string[50];
            //strings used to determine the current location of racers (0=computer, 1=player)
            int[] racerPositions = new int[2];
            //used to generate random numbers
            Random random = new Random();
            //used to track of games won by computer and human racer (0=computer win, 1=human win)
            int[] gamesWonAndLost = new int[2];
            //used to hold user input
            string userInput = "";
            //used to determine if another round should be played
            bool playAgain = true;

            while (playAgain)
            {
                //clears console for a new round
                Console.Clear();
                //set variables to default values
                racerPositions = new int[2];
                userInput = "";

                //prints welcome message
                Console.WriteLine("\t\t\t\tRANDOM RACER\n\nINSTRUCTIONS: Random Racer is a 1v1 race\nagainst the computer to the finish line.\n\nTo commence and continue the race press any key.\nWhen either or both racers reach the finish line the game is over!");

                //wait for player to press a key before continuing
                Console.ReadKey();
                //draw initial racetrack
                racerPositions = methods.DrawRacerTrack(racerPositions, computerTrack, playerTrack, random, true);

                //main game loop
                do
                {
                    //wait for player to press a key before repeating the loop again
                    Console.ReadKey();
                    //calculate and update racerPosition strings
                    racerPositions = methods.DrawRacerTrack(racerPositions, computerTrack, playerTrack, random, false);
                    Console.WriteLine("\n\nC = Computer Racer\nH = Human Racer");
                } while (racerPositions[0] < 50 && racerPositions[1] < 50);

                //if racers tied
                if (racerPositions[0] >= 50 && racerPositions[1] >= 50)
                {
                    Console.WriteLine("\nGame Over! It's a tie!\n\nComputer wins: {0}\nHuman wins: {1}", gamesWonAndLost[0], gamesWonAndLost[1]);
                }
                //if computer racer won
                else if (racerPositions[0] >= 50)
                {
                    gamesWonAndLost[0]++;
                    Console.WriteLine("\nGame Over! Computer Racer won!\n\nComputer wins: {0}\nHuman wins: {1}", gamesWonAndLost[0], gamesWonAndLost[1]);
                }
                //if human racer won
                else if (racerPositions[1] >= 50)
                {
                    gamesWonAndLost[1]++;
                    Console.WriteLine("\nGame Over! Human Racer won!\n\nComputer wins: {0}\nHuman wins: {1}", gamesWonAndLost[0], gamesWonAndLost[1]);
                }

                Console.WriteLine("Would you like to play again? y/n");
                userInput = Console.ReadKey().KeyChar.ToString();
                //if user entered n
                if (userInput.Equals("n", StringComparison.InvariantCultureIgnoreCase))
                {
                    //ends Main() loop, closing application
                    playAgain = false;
                }
                else
                {
                    playAgain = true;
                }
            }          
        }
    }

    class RandomRacer
    {
        /// <summary>
        /// Calculates racer positions and draws the racers on a racetrack
        /// </summary>
        public int[] DrawRacerTrack(int[] racerPositions, string[] racer1Track, string[] racer2Track, Random randomNumberGenerator, bool firstDraw)
        {
            //if this isn't the first time drawing the racetrack
            if (!firstDraw)
            {
                //update racer positions
                racerPositions[0] += randomNumberGenerator.Next(0, 6);
                racerPositions[1] += randomNumberGenerator.Next(0, 6);
            }

            //populate racetrack strings usings current racer positions
            for (int i = 0; i < racer1Track.Length; i++)
            {
                //if current array element is the racer position
                if (i == racerPositions[0])
                {
                    //array element equals racer symbol
                    racer1Track[i] = "C";
                }
                else
                {
                    //array element equals piece of racetrack
                    racer1Track[i] = "-";
                }

                if (i == racerPositions[1])
                {
                    racer2Track[i] = "H";
                }
                else
                {
                    racer2Track[i] = "-";
                }
            }

            //clear the screen for drawing the racetrack
            Console.Clear();

            //draw the first racetrack
            for (int i = 0; i < racer1Track.Length; i++)
            {
                Console.Write(racer1Track[i]);
            }

            //create a space between the racetracks
            Console.WriteLine();
            Console.WriteLine("|    |    |    |    |    |    |    |    |    |    |");

            //draw the second racetrack
            for (int i = 0; i < racer2Track.Length; i++)
            {
                Console.Write(racer2Track[i]);
            }

            //return the updated racer positions
            return racerPositions;
        }
    }
}
