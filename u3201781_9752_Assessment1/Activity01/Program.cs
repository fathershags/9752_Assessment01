using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// File:           Program.cs
/// Description:    Creates a "Mad Libs" word game that prompts the player 
///                 for a list of words to substitute blanks in a templated 
///                 story
/// Author:         Peter Maxwell
/// Date:           2019/08/16
/// Version:        1.0
/// </summary>
namespace Activity01
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Variable Initialization
            //used to call methods from the method class
            MadLibs methods = new MadLibs();

            //holds game title
            string gameTitle = @"      ___           ___           ___     
     /\__\         /\  \         /\  \    
    /::|  |       /::\  \       /::\  \   
   /:|:|  |      /:/\:\  \     /:/\:\  \  
  /:/|:|__|__   /::\~\:\  \   /:/  \:\__\ 
 /:/ |::::\__\ /:/\:\ \:\__\ /:/__/ \:|__|
 \/__/~~/:/  / \/__\:\/:/  / \:\  \ /:/  /
       /:/  /       \::/  /   \:\  /:/  / 
      /:/  /        /:/  /     \:\/:/  /  
     /:/  /        /:/  /       \::/__/   
     \/__/         \/__/         ~~       
      ___                   ___           ___     
     /\__\      ___        /\  \         /\  \    
    /:/  /     /\  \      /::\  \       /::\  \   
   /:/  /      \:\  \    /:/\:\  \     /:/\ \  \  
  /:/  /       /::\__\  /::\~\:\__\   _\:\~\ \  \ 
 /:/__/     __/:/\/__/ /:/\:\ \:|__| /\ \:\ \ \__\
 \:\  \    /\/:/  /    \:\~\:\/:/  / \:\ \:\ \/__/
  \:\  \   \::/__/      \:\ \::/  /   \:\ \:\__\  
   \:\  \   \:\__\       \:\/:/  /     \:\/:/  /  
    \:\__\   \/__/        \::/__/       \::/  /   
     \/__/                 ~~            \/__/    ";
            //holds welcome message
            string welcomeMessage = "Welcome to the word game Mad Libs,\nwhere YOU get to construct epic and funny stories by writing a handful of words\n\nNavigation\n<1> to look at saved Mad Libs\n<2> or <Any Key> to play a new Mad Lib";

            //holds story templates
            string[] storyTemplates = { "The {0} {1} {2} jumps over the {3} {4}.",
                "A {0} gets caught by a {1} and pleas for {2} because he is such a small bit of a {3}. Too bad. Yum yum.",
                "Weep, you {0}. My {1} has given you up. Now it {2}s {3}. Goodbye, wondrous {4}!" };
            //holds story descriptions
            string[] storyDescriptions = { "The Fox and Dog", "The Circle of Life", "Ancient Poetry" };

            //sets up storage for each stories prompts
            //contained in storyPrompts var below
            #region Individual Story Prompts
            //holds prompts for storyTemplates[0]
            string[] story1Prompts = { "an <Adjective>", "an <Adjective>", "a <Common Noun (A thing)>", "an <Adjective>", "a <Noun>" };
            //holds prompts for storyTemplates[1]
            string[] story2Prompts = { "a <Noun>", "a <Noun>", "a <Noun>", "a <Noun>" };
            //holds prompts for storyTemplates[2]
            string[] story3Prompts = { "a <Noun>", "a <Noun - body part>", "a <Verb>", "a <Noun - body part>", "a <Noun>" };
            #endregion

            //holds all prompts for all storyTemplates
            string[][] storyPrompts = { story1Prompts, story2Prompts, story3Prompts };
            //used to determine which story is active
            int storyNumber = 0;
            //holds the user inputted answers for each prompt
            string[] userAnswers = new string[0];
            //holds user stories loaded from "Activity01_Save_Data.txt"
            string[] savedStories = new string[0];

            //holds bool to check if game should continue running
            bool keepPlaying = true;
            //used to determine if a valid user input has been made
            bool validSelection = false;
            //used to determine if user is viewing saved stories
            bool viewSavedStories = false;
            #endregion

            //resizes console window
            Console.SetWindowSize(100, 50);
            //prints game title and welcome message to screen
            Console.WriteLine(gameTitle);
            Console.WriteLine("\n\n\n\n");
            Console.WriteLine(welcomeMessage);

            //main game loop
            while (keepPlaying)
            {
                //holds user input
                string keyContent = Console.ReadKey().KeyChar.ToString();
                int keyContentInt = 0;

                //resets var to false for next game loop
                validSelection = false;
                //clears the console window
                Console.Clear();

                #region View Saved Stories
                //attempt to convert keyContent from string to int, output to keyContentInt
                int.TryParse(keyContent, out keyContentInt);
                //match keyContentInt to a case
                switch (keyContentInt)
                {
                    case 1:
                        //enables viewSavedStories while loop
                        viewSavedStories = true;
                        break;
                    case 2:
                        //disables viewSavedStories while loop
                        viewSavedStories = false;
                        break;
                    default:
                        //will play a game of libs if no valid case is selected
                        break;
                }

                //user is viewing their saved stories
                while (viewSavedStories)
                {
                    //loads any saved stories from previous play sessions
                    savedStories = methods.LoadTextFileFromBaseDirectory("Activity01_Save_Data");
                    //tells the user how many stories they have saved and their options
                    Console.WriteLine("You have {0} saved stories.\n", savedStories.Length);
                    for (int i = 0; i < savedStories.Length; i++)
                    {
                        Console.WriteLine("\n{0}. {1}\n", i + 1, savedStories[i]);
                    }

                    //loops until a valid user input is registered
                    while (!validSelection)
                    {
                        //prompts user to select a saved story
                        Console.WriteLine("\nTo delete a saved story enter the story number.\nDelete another entry, type quit or play to continue\n");
                        //hold user input
                        keyContent = Console.ReadLine();

                        //if user input is numerical
                        if (int.TryParse(keyContent, out keyContentInt))
                        {
                            //clear console window
                            Console.Clear();

                            //store saved stories in list type variable
                            List<string> tempList = savedStories.ToList();
                            //remove item from list at keyContent index
                            tempList.RemoveAt(int.Parse(keyContent));
                            //write list back to savedStories array
                            savedStories = tempList.ToArray();

                            //saves updated savedStories to Activity01_Save_Data.txt
                            methods.SaveTextFileToBaseDirectory("Activity01_Save_Data", savedStories, false);
                        }
                        else if (keyContent == "quit")
                        {
                            //terminate console application
                            Environment.Exit(1);
                        }
                        else if (keyContent == "play")
                        {
                            //play game
                            viewSavedStories = false;
                            validSelection = true;
                            Console.Clear();
                        }
                        else
                        {
                            //clears console window
                            Console.Clear();
                            //tells the user how many stories they have saved and their options
                            Console.WriteLine("You have {0} saved stories.\n", savedStories.Length);
                            for (int i = 0; i < savedStories.Length; i++)
                            {
                                Console.WriteLine("\n{0}. {1}\n", i + 1, savedStories[i]);
                            }

                            //prompts user to select a saved story or exit
                            Console.WriteLine("{0}\t...Invalid selection.\n", keyContent);
                        }
                    }

                    validSelection = false;
                }
                #endregion

                //user selects a story
                storyNumber = methods.StorySelection(storyDescriptions);

                //user answers story prompts
                userAnswers = methods.AnswerThePrompts(storyPrompts[storyNumber - 1]);

                //outputs completed story
                Console.WriteLine(storyTemplates[storyNumber - 1], userAnswers);
                Console.WriteLine();
                Console.WriteLine("What an interesting story! Would you like to save it (1), play again (2) or quit (3)?");

                //game over loop
                while (!validSelection)
                {
                    string lineContents = Console.ReadKey().KeyChar.ToString();

                    //if user wants to save their story
                    if (lineContents == "1")
                    {
                        //filename that story will be saved to
                        string fileName = "Activity01_Save_Data";
                        //saves story to fileName and keeps previous contents of fileName intact
                        methods.SaveTextFileToBaseDirectory(fileName, (string.Format(storyTemplates[storyNumber - 1], userAnswers)), true);
                        //valid selection has been made
                        validSelection = true;
                    }
                    //if user wants to play again
                    else if (lineContents == "2")
                    {
                        //valid selection has been made
                        validSelection = true;
                    }
                    //if user wants to quit
                    else if (lineContents == "3")
                    {
                        //ends while loop, closing the game
                        keepPlaying = false;
                        //valid selection has been made
                        validSelection = true;
                    }
                    //valid selection has not been made
                    else
                    {
                        Console.WriteLine("\t...That's not a valid selection.\n\nWould you like to save it (1), play again (2) or quit (3)?\n");
                    }
                }
            }
        }
    }

    class MadLibs
    {
        /// <summary>
        /// 1. Promps user to select a story
        /// 2. Reads user input, stores in storyNumber
        /// </summary>
        public int StorySelection(string[] storyDescriptions)
        {
            //resets storyNumber
            int storyNumber = 0;

            Console.WriteLine("Which story would you like to use?");
            Console.WriteLine("\n\tStory 1: {0}\n\tStory 2: {1}\n\tStory 3: {2}\n", storyDescriptions);
            Console.WriteLine("Please enter a selection (1-3): ");

            int.TryParse(Console.ReadKey().KeyChar.ToString(), out storyNumber);
            //checks a valid integer was input
            while (storyNumber == 0)
            {
                Console.WriteLine(" is not a valid selection\n");
                int.TryParse(Console.ReadKey().KeyChar.ToString(), out storyNumber);
            }

            //clears the console window for story prompts!
            Console.Clear();

            return storyNumber;
        }

        /// <summary>
        /// 1. Initializes userAnswers array
        /// 2. Prints story prompts
        /// 3. Reads user input, stores in userAnswers
        /// </summary>
        public string[] AnswerThePrompts(string[] prompts)
        {
            //resizes userAnswers so it can hold all the following user inputs
            string[] userAnswers = new string[prompts.Length];
            //cycles through each prompt, printing it and prompting the user for input
            for (int i = 0; i < prompts.Length; i++)
            {
                Console.Write("Give me {0}: ", prompts[i]);
                userAnswers[i] = Console.ReadLine();
                Console.WriteLine();
            }

            return userAnswers;
        }

        /// <summary>
        /// Saves text file "fileName" to the base directory
        /// </summary>
        public void SaveTextFileToBaseDirectory(string fileName, string inputText)
        {
            //initialize variables
            string previousTextData = "";

            //checks if fileName already exists
            if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + fileName + ".txt"))
            {
                //holds text from existing file
                previousTextData = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + fileName + ".txt");
            }

            //writes inputText to the specified file path/name
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + fileName + ".txt", string.Concat(previousTextData, inputText, Environment.NewLine));
        }
        /// <summary>
        /// Saves text file "fileName" to the base directory. Merges with existing text file or overwrites
        /// </summary>
        public void SaveTextFileToBaseDirectory(string fileName, string inputText, bool mergeWithPreviousFile)
        {
            //initialize variables
            string previousTextData = "";

            //checks if inputText should be merged with existing text in fileName
            if (mergeWithPreviousFile)
            {
                //checks if fileName already exists
                if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + fileName + ".txt"))
                {
                    //holds text from existing file
                    previousTextData = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + fileName + ".txt");
                }
            }

            //writes inputText to the specified file path/name
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + fileName + ".txt", string.Concat(previousTextData, inputText, Environment.NewLine));
        }
        /// <summary>
        /// Saves text file "fileName" to the base directory. Merges with existing text file or overwrites
        /// </summary>
        public void SaveTextFileToBaseDirectory(string fileName, string[] inputText, bool mergeWithPreviousFile)
        {
            //initialize variables
            string previousTextData = "";
            string inputTextToWrite = "";

            //checks if inputText should be merged with existing text in fileName
            if (mergeWithPreviousFile)
            {
                //checks if fileName already exists
                if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + fileName + ".txt"))
                {
                    //holds text from existing file
                    previousTextData = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + fileName + ".txt");
                }
            }

            //condenses inputText array into string, with each array element on a new line
            for (int i = 0; i < inputText.Length; i++)
            {
                inputTextToWrite = string.Concat(inputTextToWrite, inputText[i], Environment.NewLine);
            }

            //writes inputText to the specified file path/name
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + fileName + ".txt", string.Concat(previousTextData, inputTextToWrite, Environment.NewLine));
        }
        
        /// <summary>
        /// Loads text file "fileName" from the base directory, searches for textToLoad and stores it in outputText
        /// </summary>
        public string[] LoadTextFileFromBaseDirectory(string fileName)
        {
            string[] outputText = new string[System.IO.File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + fileName + ".txt").Length];

            //attemps to load fileName text file into outputText
            try
            {
                for (int i = 0; i < outputText.Length; i++)
                {
                    outputText[i] = (System.IO.File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + fileName + ".txt")[i]);
                }
            }
            //fileName doesn't exist, creates fileName
            catch (Exception) { System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + fileName + ".txt"); }

            return outputText;
        }
    }
}
