using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
/// <summary>
/// File:           Program.cs
/// Description:    A console application that allows the user to play one or more games of "Random Racer"
/// Author:         Peter Maxwell
/// References:     http://ascii.co.uk/art/hangman used for hangman ascii art
/// Date:           2019/09/20
/// Version:        1.0
/// </summary>
namespace Activity05
{
    class Program
    {
        static void Main(string[] args)
        {
            //used to access hangman methods
            Hangman hangman = new Hangman();

            //executes the main hangman game method
            hangman.Run();
        }
    }

    class Hangman
    {
        /// <summary>
        /// Main method to run the Hangman Game
        /// </summary>
        public void Run()
        {
            #region Variable Setup
            //used to loop through the game, console will close if this returns false
            bool activeGame = true;
            //used to loop through the main game (inputting a guess, outputting a result)
            bool mainGameLoop = true;
            //used to determine if the player has won the game
            bool mysteryWordCorrectlyGuessed = false;

            //used to select a mystery word for the active game
            Random random = new Random();
            //holds mystery words
            string[] mysteryWordPool = { "heracles", "odysseus", "orpheus", "achilles", "theseus", "cadmus", "perseus", "jason" };
            //holds mystery words split into string arrays of each of their characters e.g. (h, e, r, a, c, l, e, s)
            string[][] mysteryWordPoolArray = new string[mysteryWordPool.Length][];
            //transfers mystery words from pool into array pool
            for (int i = 0; i < mysteryWordPool.Length; i++)
            {
                mysteryWordPoolArray[i] = new string[mysteryWordPool[i].Length];

                for (int j = 0; j < mysteryWordPool[i].Length; j++)
                {
                    mysteryWordPoolArray[i][j] = mysteryWordPool[i].Substring(j, 1);
                }
            }

            //holds all letters the player has guessed that are in the mystery word
            string[] lettersGuessed = new string[26];
            //holds player input
            string userInput = "";
            //holds the active games mystery word
            string[] mysteryWord = new string[0];
            //holds the hidden word (e.g. "______") with all the correct player guesses (e.g "A_es_me")
            string[] hiddenWordWithPlayerGuesses = new string[0];
            //used to determine if player guess is not found in mystery word
            bool guessedLetterNotInMysteryWord = true;
            //counts how many incorrect guesses the player has made
            int incorrectGuesses = 0;
            //counts total number of guesses player has made
            int guesses = 0;
            #endregion

            //resizes console window to properly fit in the hangman ascii art
            Console.SetWindowSize(100, 75);

            //while the user is playing the game
            while (activeGame)
            {
                //clears any previous game
                Console.Clear();
                //resets variables to inital values
                mainGameLoop = true;
                activeGame = true;
                mysteryWordCorrectlyGuessed = false;
                incorrectGuesses = 0;
                guesses = 0;
                lettersGuessed = new string[26];
                userInput = "";

                //randomly selects a mystery word from the word pool
                mysteryWord = mysteryWordPoolArray[random.Next(0, mysteryWordPoolArray.Length)];
                //resizes display string to correctly fit the mystery word
                hiddenWordWithPlayerGuesses = new string[mysteryWord.Length];
                for (int i = 0; i < hiddenWordWithPlayerGuesses.Length; i++)
                {
                    hiddenWordWithPlayerGuesses[i] = "_";
                }

                Console.WriteLine("\t\t\t\tHANGMAN\n\nINSTRUCTIONS: Win by correctly guessing all the letters in the mystery word.\nAfter making six incorrect guesses you'll get a hangman and lose the game\n\nPress any key to begin");

                Console.ReadKey();

                //main game loop
                while (mainGameLoop)
                {
                    //draws initial hangman frame
                    DrawHangman(hiddenWordWithPlayerGuesses, incorrectGuesses, mysteryWordCorrectlyGuessed);

                    //if player hasn't run out of guesses and hasn't guessed the full mystery word
                    if (incorrectGuesses < 6 && !mysteryWordCorrectlyGuessed)
                    {
                        //resets variable to initial value
                        guessedLetterNotInMysteryWord = true;
                        //validates user input
                        userInput = ValidatePlayerInput(Console.ReadLine());
                        //stores validated user input in lettersGuessed array
                        lettersGuessed[guesses] = userInput;

                        //cycle through each letter in mystery word
                        for (int i = 0; i < mysteryWord.Length; i++)
                        {
                            //if letter in mysteryword matches the guessed letter
                            if (mysteryWord[i] == lettersGuessed[guesses])
                            {
                                for (int j = 0; j < hiddenWordWithPlayerGuesses.Length; j++)
                                {
                                    //if character is currently hidden
                                    if (hiddenWordWithPlayerGuesses[i] == "_" && mysteryWord[j].Equals(lettersGuessed[guesses]))
                                    {
                                        //changes _ to correctly guessed letter
                                        hiddenWordWithPlayerGuesses[i] = lettersGuessed[guesses];
                                        //capitalizes the first letter
                                        hiddenWordWithPlayerGuesses[0] = hiddenWordWithPlayerGuesses[0].ToUpper();
                                    }
                                }

                                //used to determine if an incorrect guess was made
                                guessedLetterNotInMysteryWord = false;
                            }
                        }

                        if (guessedLetterNotInMysteryWord)
                        {
                            //add 1 to incorrect guesses
                            incorrectGuesses++;
                        }

                        //add 1 to guesses
                        guesses++;

                        //if current display string doesn't contain any _
                        if (!hiddenWordWithPlayerGuesses.Concat(hiddenWordWithPlayerGuesses).Contains("_"))
                        {
                            //the mystery word has been fully revealed
                            mysteryWordCorrectlyGuessed = true;
                        }
                    }
                    else if (Console.ReadKey().KeyChar.ToString().Equals("n", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //quits the application
                        mainGameLoop = false;
                        activeGame = false;
                    }
                    else
                    {
                        //ends the active game
                        mainGameLoop = false;
                    }
                }
            }
        }

        /// <summary>
        /// Clears and draws a new hangman scene after every player input
        /// </summary>
        public void DrawHangman(string[] wordToWrite, int incorrectGuessCount, bool playerWon)
        {
            //holds all the different ascii art segments of the hangman
            string[] hangmanSegments = { @" ___________.._______
| .__________))______|
| | / /      ||
| |/ /       ||
| | /        ||
| |/         ||
| |          || 
| |           \\-\\
| |            \\_\\
| |           
| |          
| |           
| |             
| |          
| |           
| |           
| |           
| |             
''''''''''|_        |''''|
|'|'''''''\ \       '''|'|
| |        \ \        | |
: :         \ \       : : 
. .          `'       . .", @" ___________.._______
| .__________))______|
| | / /      ||
| |/ /       ||
| | /        ||.-''.
| |/         |/  _  \
| |          ||  `/,|
| |          (\\`_.'
| |           `--'
| |           
| |          
| |           
| |             
| |          
| |           
| |           
| |           
| |             
''''''''''|_        |''''|
|'|'''''''\ \       '''|'|
| |        \ \        | |
: :         \ \       : : 
. .          `'       . .", @" ___________.._______
| .__________))______|
| | / /      ||
| |/ /       ||
| | /        ||.-''.
| |/         |/  _  \
| |          ||  `/,|
| |          (\\`_.'
| |         .-`--'.
| |           . . 
| |          |   | 
| |          | . |  
| |          |   |   
| |          
| |           
| |           
| |           
| |             
''''''''''|_        |''''|
|'|'''''''\ \       '''|'|
| |        \ \        | |
: :         \ \       : : 
. .          `'       . .", @" ___________.._______
| .__________))______|
| | / /      ||
| |/ /       ||
| | /        ||.-''.
| |/         |/  _  \
| |          ||  `/,|
| |          (\\`_.'
| |         .-`--'.
| |        /Y . . 
| |       // |   | 
| |      //  | . |  
| |     ')   |   |   
| |          
| |           
| |           
| |           
| |             
''''''''''|_        |''''|
|'|'''''''\ \       '''|'|
| |        \ \        | |
: :         \ \       : : 
. .          `'       . .", @" ___________.._______
| .__________))______|
| | / /      ||
| |/ /       ||
| | /        ||.-''.
| |/         |/  _  \
| |          ||  `/,|
| |          (\\`_.'
| |         .-`--'.
| |        /Y . . Y\
| |       // |   | \\
| |      //  | . |  \\
| |     ')   |   |   (`
| |          
| |           
| |           
| |           
| |             
''''''''''|_        |''''|
|'|'''''''\ \       '''|'|
| |        \ \        | |
: :         \ \       : : 
. .          `'       . .", @" ___________.._______
| .__________))______|
| | / /      ||
| |/ /       ||
| | /        ||.-''.
| |/         |/  _  \
| |          ||  `/,|
| |          (\\`_.'
| |         .-`--'.
| |        /Y . . Y\
| |       // |   | \\
| |      //  | . |  \\
| |     ')   |   |   (`
| |          ||'
| |          || 
| |          || 
| |          || 
| |         / |    
''''''''''|_`-'     |''''|
|'|'''''''\ \       '''|'|
| |        \ \        | |
: :         \ \       : : 
. .          `'       . .", @" ___________.._______
| .__________))______|
| | / /      ||
| |/ /       ||
| | /        ||.-''.
| |/         |/  _  \
| |          ||  `/,|
| |          (\\`_.'
| |         .-`--'.
| |        /Y . . Y\
| |       // |   | \\
| |      //  | . |  \\
| |     ')   |   |   (`
| |          ||'||
| |          || ||
| |          || ||
| |          || ||
| |         / | | \
''''''''''|_`-' `-' |''''|
|'|'''''''\ \       '''|'|
| |        \ \        | |
: :         \ \       : : 
. .          `'       . ." };
            
            //clears the console window
            Console.Clear();
            //draws the active hangman segment
            Console.WriteLine(hangmanSegments[incorrectGuessCount]);
            //active game in progress
            if (incorrectGuessCount < 6 && !playerWon)
            {
                //outputs how how many incorrect guesses the player has made
                Console.Write("\n\nYou've made {0} incorrect guesses\n\n\t\tMystery Word: ", incorrectGuessCount);

                //outputs what the player has guessed so far for the mystery word e.g. "A_es_me"
                for (int i = 0; i < wordToWrite.Length; i++)
                {
                    Console.Write(wordToWrite[i]);
                }
                Console.WriteLine();
                //outputs a prompt for the player to enter in their next guess
                Console.Write("\n\nGuess a letter:");
            }
            //player lost
            else if (!playerWon)
            {
                //outputs lose message
                Console.Write("\n\nYou've made 6 incorrect guesses\n\n\t\tYou lose, game over!\n\n");
                Console.WriteLine("\t\t\t\tWould you like to play again?");
            }
            //player won
            else
            {
                //outputs win message
                Console.Write("\n\nYou've guessed the mystery word\n\n\t\tMystery Word: ");
                //outputs fully guessed mystery word
                for (int i = 0; i < wordToWrite.Length; i++)
                {
                    Console.Write(wordToWrite[i]);
                }
                Console.WriteLine("\n\n\t\t\tYou win, game over!\n\n\t\t\t\tWould you like to play again?");
            }
        }

        /// <summary>
        /// If player input is correct it is returned as a string.
        /// If player input is incorrect the player is prompted to input again
        /// </summary>
        public string ValidatePlayerInput(string input)
        {
            //stores player input in lowercase
            input = input.ToLower();

            //if player input is invalid
            while (!Regex.IsMatch(input, @"^[a-zA-Z]") || input.Length != 1)
            {
                //prompt player to input again
                Console.WriteLine("You didn't enter a valid input. Please enter one letter ==> ");
                //store player input in lowercase
                input = Console.ReadLine().ToLower();
            }

            //return validated input
            return input;
        }
    }
}
