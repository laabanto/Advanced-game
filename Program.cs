using System;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;
using System.Linq;
using System.ComponentModel;
using System.IO.Pipes;

namespace Advance
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string name = "Lubito"; // Bot's name

            bool nameRequestMode = false;
            int expectedArgsCount = 3; // Default
            char[] friendPieces = new char[9]; // To hold codes for friendly pieces. So program knows which characters are the firendly pieces
            //char[] friendPieces;

            // Dertermine expected number of arguments
            if (args[0] == "name") 
            { 
                expectedArgsCount = 1;
                nameRequestMode = true;
            }

            if (nameRequestMode) 
            {
                if (args.Length == expectedArgsCount)
                {
                    if (args[0] != "name") // The first argument is invalid
                    {
                        Console.WriteLine("First argument is invalid. Expected 'name', 'white', or 'black'.");
                        // Terminate program
                        Environment.Exit(0);
                    }
                    else // Name of bot requested
                    {
                        Console.WriteLine("{0}", name); // dispaly bot's name
                        Environment.Exit(0); // terminate program
                    }
                }
                else // Not the expected number of arguments (1)
                {
                    if (args.Length == 1) // Error messages.
                    {
                        Console.WriteLine($"Error: Only {args.Length} argument was provided. Expected number of arguments is {expectedArgsCount}.");
                    }

                    else
                    {
                        Console.WriteLine($"Error: Only {args.Length} arguments were provided. Expected number of arguments is {expectedArgsCount}.");
                    }

                    // Terminate the program
                    Environment.Exit(0);
                }               
            }

            else // Game Mode
            {
                ///////////////////////////////////////////////////////////////READ COMMAND LINE ARGS/////////////////////////////////////////////////////////////////////////////////////
                // Check if the expected number of arguments is provided
                if (args.Length == expectedArgsCount)
                {
                    string boardStateFile = args[1];  // Second command line argument - READ FROM FILE
                    if (!File.Exists(boardStateFile)) // Check if file exists

                    {
                        // File does not exist
                        Console.WriteLine("Error: Read from file provided does not exist.");
                        // Terminate program
                        Environment.Exit(0);
                    }

                    else
                    {
                        if (args[0] != "white" && args[0] != "black") // The first argument is invalid
                        {
                            Console.WriteLine("First argument is invalid. Expected 'name', 'white', or 'black'.");
                            // Terminate program
                            Environment.Exit(0);
                        }

                        else // Read command lines args. If this point was reached. NO ISSUES with command line args
                        {
                            
                            char[] whiteArmy = { 'Z', 'B', 'M', 'J', 'S', 'C', 'D', 'G'};
                            char[] blackArmy = { 'z', 'b', 'm', 'j', 's', 'c', 'd', 'g' };
                            char[] otherChras = { '.', '#' };
                            char[] allteams = whiteArmy.Concat(blackArmy).ToArray();
                            char[] allowedChars = allteams.Concat(otherChras).ToArray();

                            string teamColor = args[0];  // First command line argument

                                switch (teamColor)
                                {
                                    case "white": // Bot's team is white
                                    friendPieces = whiteArmy;

                                        //friendPieces[0] = 'Z';
                                        //friendPieces[1] = 'B';
                                        //friendPieces[2] = 'M';
                                        //friendPieces[3] = 'J';
                                        //friendPieces[4] = 'S';
                                        //friendPieces[5] = 'C';
                                        //friendPieces[6] = 'D';
                                        //friendPieces[7] = 'G';
                                    break;

                                    case "black": // Bot's team is black
                                    friendPieces = blackArmy;

                                        //friendPieces[0] = 'z';
                                        //friendPieces[1] = 'b';
                                        //friendPieces[2] = 'm';
                                        //friendPieces[3] = 'j';
                                        //friendPieces[4] = 's';
                                        //friendPieces[5] = 'c';
                                        //friendPieces[6] = 'd';
                                        //friendPieces[7] = 'g';
                                break;
                                }


                                // PLAY MOVE
                                AdvanceBot advanceBot = new AdvanceBot(teamColor);

                                // GET BOARD STATE AFTER EXECUTING MOVE
                                char[,] advanceBoard = advanceBot.ExecuteMove(boardStateFile, friendPieces, allowedChars);

                                ////////////////////////////////////////////////////////// WRITE TO FILE ///////////////////////////////////////////////////////////////

                                string write2File = args[2];  // Third command line argument - WRITE TO FILE

                                using FileStream fileStream = new(write2File, FileMode.OpenOrCreate); // create the file stream object

                                try
                                {
                                    using (StreamWriter writer = new StreamWriter(fileStream))
                                    {
                                        for (int i = 0; i < advanceBoard.GetLength(0); i++) // for number of rows
                                        {
                                            for (int j = 0; j < advanceBoard.GetLength(1); j++) // for number of columns
                                            {
                                                writer.Write(advanceBoard[i, j]);
                                            }

                                            writer.WriteLine();
                                            //fileStream.Close(); // Close file stream
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error: {ex.Message}");
                                }
                                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////                          
                        }
                    }
                }

                else // Not the expected number of arguments (3)
                {

                    if (args.Length == 1) // Error messages.
                    {
                        Console.WriteLine($"Error: Only {args.Length} argument was provided. Expected number of arguments is {expectedArgsCount}.");
                    }

                    else
                    {
                        Console.WriteLine($"Error: Only {args.Length} arguments were provided. Expected number of arguments is {expectedArgsCount}.");
                    }

                    // Terminate the program
                    Environment.Exit(0);
                }
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            }
        }
    } 
}

