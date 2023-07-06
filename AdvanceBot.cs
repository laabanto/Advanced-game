using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Advance
{
    /// <summary>
    /// When object of class is created it represents a turn in the game. It contains information about the teamcolour palying the turn.
    /// It creates a representation of the current board state and selects the best move in favour of the team playing the trun.
    /// </summary>
    internal class AdvanceBot
    {
        protected string teamColour; // Colour of team palying current turn

        public string TeamColour
        {
            get { return teamColour; } //Read-only outsidet this class
            private set
            { // can be changed in this class 
                teamColour = value;
            }// can be changed in this class
        }
        public AdvanceBot(string teamColour) 
        {
            this.teamColour = teamColour ;
        }

        /// <summary>
        /// Updates the board state by making a single move
        /// </summary>
        /// <param name="bstext"></param>
        /// <param name="friendPieces"></param>
        /// <param name="allowedChars"></param>
        /// <returns>The updated board state</returns>
        public char[,] ExecuteMove(string bstext, char[] friendPieces, char[] allowedChars) 
        {
            //bool genCanBeSaved = false;
            bool lookForPT = false;
            bool checkWallSave = false;
            bool wallSave = false;
            bool threatEG = false;
            int[] threatEGLoc = { -1, -1 };
            int[] primeTarget = {-1, -1};
            int[] generalPos = { -1, -1};
            int[] builderSavior = { -1, -1 };
            int[] enemyGPos = { -1, -1}; // To hold position of enemy General 
            int numCapts = 0;
            int[] moveTo;
            int[] moveFrom;
            char charCode;


            List<Piece> myPieces = new List<Piece>(); // to hold friendly pieces
            List<Piece> enemyPieces = new List<Piece>(); // to hold enemy pieces
            List<int[]> allEnemyMoves = new List<int[]>(); // All possible enemy moves
            List<int[]> checkMateLocs = new List<int[]>(); // All possible check mate locations (not confirmed, but possible)

            // Create board state representation

            char[,] advanceBoard = GenBoardRep(bstext, allowedChars);

            // inspecting each square of current board state representation (2D char array)

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    int[] posIndex = { i, j }; // Current position
                    switch (advanceBoard[i, j]) // Inspects 2D array representation of board state and instantiates a piece object as they are encountered.
                                                // objects are assigned to either the friendly piece or enemy piece lists
                    {
                        case 'Z':
                        case 'z':
                            Zombie zombie = new Zombie(advanceBoard[i, j], new int[] { i, j });

                            zombie.MovesSelect(advanceBoard, posIndex);
                            zombie.IsProtected(advanceBoard, posIndex);

                            if (friendPieces.Contains(advanceBoard[i, j]))
                            {
                                myPieces.Add(zombie);
                            }
                            else
                            {
                                enemyPieces.Add(zombie);
                            }   allEnemyMoves.AddRange(zombie.AllMoves);
                            break;

                        case 'B':
                        case 'b':
                            Builder builder = new Builder(advanceBoard[i, j], new int[] { i, j });

                            builder.MovesSelect(advanceBoard, posIndex);
                            builder.IsProtected(advanceBoard, posIndex);

                            if (friendPieces.Contains(advanceBoard[i, j]))
                            {
                                myPieces.Add(builder);
                            }
                            else
                            {
                                enemyPieces.Add(builder);
                                allEnemyMoves.AddRange(builder.AllMoves);
                            }
                            break;

                        case 'M':
                        case 'm':
                            Miner miner = new Miner(advanceBoard[i, j], new int[] { i, j });

                            miner.MovesSelect(advanceBoard, posIndex);
                            miner.IsProtected(advanceBoard, posIndex);

                            if (friendPieces.Contains(advanceBoard[i, j]))
                            {
                                myPieces.Add(miner);
                            }
                            else
                            {
                                enemyPieces.Add(miner);
                                allEnemyMoves.AddRange(miner.AllMoves);
                            }
                            break;

                        case 'J':
                        case 'j':
                            Jester jester = new Jester(advanceBoard[i, j], new int[] { i, j });

                            jester.MovesSelect(advanceBoard, posIndex);
                            jester.IsProtected(advanceBoard, posIndex);

                            if (friendPieces.Contains(advanceBoard[i, j]))
                            {
                                myPieces.Add(jester);
                            }
                            else
                            {
                                enemyPieces.Add(jester);
                                allEnemyMoves.AddRange(jester.AllMoves);
                            }
                            break;

                        case 'S':
                        case 's':
                            Sentinel sentinel = new Sentinel(advanceBoard[i, j], new int[] { i, j });

                            sentinel.MovesSelect(advanceBoard, posIndex);
                            sentinel.IsProtected(advanceBoard, posIndex);

                            if (friendPieces.Contains(advanceBoard[i, j]))
                            {
                                myPieces.Add(sentinel);
                            }
                            else
                            {
                                enemyPieces.Add(sentinel);
                                allEnemyMoves.AddRange(sentinel.AllMoves);
                            }
                            break;

                        case 'C':
                        case 'c':
                            Catapult catapult = new Catapult(advanceBoard[i, j], new int[] { i, j });

                            catapult.MovesSelect(advanceBoard, posIndex);
                            catapult.IsProtected(advanceBoard, posIndex);

                            if (friendPieces.Contains(advanceBoard[i, j]))
                            {
                                myPieces.Add(catapult);
                            }
                            else
                            {
                                enemyPieces.Add(catapult);
                                allEnemyMoves.AddRange(catapult.AllMoves);
                            }
                            break;

                        case 'D':
                        case 'd':
                            Dragon dragon = new Dragon(advanceBoard[i, j], new int[] { i, j });

                            dragon.MovesSelect(advanceBoard, posIndex);
                            dragon.IsProtected(advanceBoard, posIndex);

                            if (friendPieces.Contains(advanceBoard[i, j]))
                            {
                                myPieces.Add(dragon);
                            }
                            else
                            {
                                enemyPieces.Add(dragon);
                                allEnemyMoves.AddRange(dragon.AllMoves);
                            }
                            break;

                        case 'G':
                        case 'g':
                            General general = new General(advanceBoard[i, j], new int[] { i, j });

                            general.MovesSelect(advanceBoard, posIndex);
                            general.IsProtected(advanceBoard, posIndex);

                            if (friendPieces.Contains(advanceBoard[i, j]))
                            {
                                general.BuilderFriend(advanceBoard, posIndex); // To check if General can be protected by builder
                                myPieces.Add(general);
                            }
                            else
                            {
                                enemyPieces.Add(general);
                                allEnemyMoves.AddRange(general.AllMoves);
                            }
                            break;

                        case '#':
                        case '.':
                            break;
                    }

                }
            }

            bool areEqual; //for comparing if two locations (arrays) are equal
            bool areEqual2; //for comparing if two locations (arrays) are equal


            // Checks if General's no capture moves are safe. General might need to consider this moves to escape danger
            foreach (Piece piece in myPieces)
            {
                if (piece.Code == 'G' || piece.Code == 'g') // find General
                {
                    List<int[]> move2Remove = new List<int[]>();
                    foreach (int[] gloc in piece.NoCaptMoves)
                    {
                        foreach (Piece epiece in enemyPieces)
                        {
                            if (epiece.Code == 'G' || epiece.Code == 'g') // find General/////////////////////////////////NEW!!!!!!!!
                            {
                                enemyGPos = epiece.CurrentPosition; // Remember enemy's current position
                            }/////////////////////////////////////////////////////////////////////////////////////////////NEW!!!!!!!!
                            foreach (int[] loc in epiece.AllMoves)
                            {
                                areEqual2 = gloc.SequenceEqual(loc); // locations coincide

                                if (areEqual2) // friendly piece in danger of bieng captured if it moves to this location
                                //if (areEqual2 && epiece.canCaptOrConvrt)///////////////////BORRAR
                                {
                                    foreach (int element2 in gloc)///////////////////BORRAR
                                    {
                                        Console.Write(element2);

                                    }///////////////////BORRAR
                                    move2Remove.Add(gloc); // Collect the move to be removed 
                                }
                            }
                        }
                    }
                    // Remove collected moves from friendly pieces
                    foreach (int[] removed in move2Remove)
                    {
                        piece.NoCaptMoves.Remove(removed);
                    }
                    break; // once general has been found and locations checked exit loop
                }
                
            }


            // Check if any one of all possible moves of each friendly piece coincides with a possible move of each enemy piece
            foreach (Piece piece in myPieces)
            {
                List<int[]> moves2Remove = new List<int[]>();

                foreach (int[] mloc in piece.Moves)
                {
                    foreach (Piece epiece in enemyPieces)
                    {
                        if (piece.Code == 'g' || piece.Code == 'G') // Only General cosiders all posible enemy locations
                        {
                            foreach (int[] loc in epiece.AllMoves)
                            {
                                areEqual = mloc.SequenceEqual(loc); // locations coincide

                                if (areEqual) // firendly piece in danger of bieng captured if it moves to this location 
                                                                        //if (areEqual && epiece.canCaptOrConvrt)
                                {
                                    moves2Remove.Add(mloc); // Collect the move to be removed 
                                }
                            }
                        }

                        else 
                        {
                            //Console.WriteLine("my moves to this point are {0}", piece.Moves.Count);
                            foreach (int[] loc in epiece.Moves)
                            {
                                areEqual = mloc.SequenceEqual(loc); // locations coincide

                                if (areEqual && epiece.CanCaptOrConvrt) // firendly piece in danger of bieng captured if it moves to this location
                                {
                                    moves2Remove.Add(mloc); // Collect the move to be removed 
                                    piece.DangerMoves.Add(mloc);
                                }
                            }
                        }                        
                    }
                }

                // Remove collected moves from friendly pieces
                foreach (int[] removed in moves2Remove)
                {
                    piece.Moves.Remove(removed);
                }
            }

            bool includeI = true; // if true possible check mate location on i (row) is included
            bool includeJ = true; // if true possible check mate location on j (column) is included

            // Check if in any possible move locations for each friendly piece there is an enemy piece to capture
            foreach (Piece piece in myPieces) 
            {
                foreach (int[] mloc in piece.Moves)
                {
                    // Find if any friendly piece possible move could potentially threaten the enemy's General///////NEW!
                    if (mloc[0] == enemyGPos[0]) // row coincides with enemy G position
                    {
                        if (mloc[1] > enemyGPos[1])
                        {
                            for (int j = enemyGPos[1] + 1; j < mloc[1]; j++) 
                            {
                                if (advanceBoard[mloc[0], j] != '.')
                                {
                                    includeI = false;
                                    break;
                                }
                            }
                        }
                        else 
                        {
                            for (int j = mloc[1] + 1; j < enemyGPos[1]; j++)
                            {
                                if (advanceBoard[mloc[0], j] != '.')
                                {
                                    includeI = false;
                                    break;
                                }
                            }
                        }

                        if (includeI) { checkMateLocs.Add(mloc); }
                    }

                    if (mloc[1] == enemyGPos[1]) // column coincides with enemy G position
                    {
                        if (mloc[0] > enemyGPos[0])
                        {
                            for (int i = enemyGPos[0] + 1; i < mloc[0]; i++)
                            {
                                if (advanceBoard[mloc[1], i] != '.')
                                {
                                    includeJ = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = mloc[0] + 1; i < enemyGPos[0]; i++)
                            {
                                if (advanceBoard[mloc[1], i] != '.')
                                {
                                    includeJ = false;
                                    break;
                                }
                            }
                        }

                        if (includeJ) { checkMateLocs.Add(mloc); }
                    }

                    if (checkMateLocs.Count != 0)
                    {
                        threatEG = true;
                        threatEGLoc = checkMateLocs[0];
                    }                  
                    ////////////////////////////////////////////////////////////////////////////////////////NEW!!!!!!!!!!!!!!!!!
                    
                    foreach (Piece epiece in enemyPieces)
                    {
                        areEqual = epiece.CurrentPosition.SequenceEqual(mloc);
                        if (areEqual && piece.CanCaptOrConvrt) // there is an enemy piece
                        {
                            if (epiece.SentinelProtect) // enemy piece is protected
                            {
                                continue; // next one in list
                            }
                            else
                            {
                                piece.KillAt = mloc; // remember capture location
                                piece.KillScore = epiece.Worth; // priority level of capture move becomes the value of the enemy piece to be captured
                            }
                        }
                    }
                }
            }

            // check if in any of the possible move locations for each enemy piece is a friendly piece to see if friendly piece is in danger
            foreach (Piece epiece in enemyPieces) 
            {
                foreach (int[] loc in epiece.Moves)
                {
                    foreach (Piece piece in myPieces)
                    {
                        areEqual = piece.CurrentPosition.SequenceEqual(loc);
                        if (areEqual && epiece.CanCaptOrConvrt) // friendly piece might be in danger of being captured
                        {
                            if (piece.SentinelProtect) // friendly piece is protected
                            {
                                continue; // don't worry about it!
                            }
                            else
                            {
                                piece.EminentThreat = true; // friendly piece in eminent DANGER!


                                if ((piece.Code == 'G' || piece.Code == 'g') && piece.Moves.Count == 0)
                                {
                                    lookForPT = true; //(prime target )
                                    generalPos = piece.CurrentPosition;
                                    primeTarget = epiece.CurrentPosition; // remember enemy piece. We might be able to take it down
                                    piece.Moves.AddRange(piece.NoCaptMoves); // General looks to it's list of no capture moves to scape

                                    if (piece.BF)
                                    {
                                        checkWallSave = true;
                                        builderSavior = piece.BFAt; // remember location of builder friend
                                    }
                                    ///////////////////////////////////////////////////////////////////////////////////JUST ADDED
                                    foreach (int[] mloc in piece.NoCaptMoves) // Check if there are danger moves within the list of General's no capture moves (just added)
                                    {
                                        List<int[]> moves2Remove = new List<int[]>();

                                        foreach (int[] thisloc in epiece.RuleMoves)
                                        {
                                            areEqual = thisloc.SequenceEqual(mloc);
                                            if (areEqual)
                                            {
                                                moves2Remove.Add(mloc); // Collect the move to be removed 
                                                piece.DangerMoves.Add(mloc);

                                            }
                                        }

                                        // Remove collected moves that were identified as dangerous
                                        foreach (int[] removed in moves2Remove)
                                        {
                                            piece.Moves.Remove(removed);
                                        }
                                    }///////////////////////////////////////////////////////////////////////////////////JUST ADDED
                                }

                                if (piece.Worth > piece.KillScore) // if value of piece is greater than what i can capture...
                                {
                                    piece.KillScore = piece.Worth; // assign priority level to the piece's value
                                }
                            }
                            
                        }
                    }
                }
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Piece primeEnemy = null;

            // CHECK IF THERE IS A PRIME TARGET (ENEMY PIECE THREATENING FRIENDLY GENERAL)
            if (lookForPT) 
            {
                bool hasToExit = false;
                foreach (Piece piece in myPieces)
                {
                    foreach (int[] mloc in piece.AllMoves)
                    {
                        areEqual = primeTarget.SequenceEqual(mloc);
                        if (areEqual)
                        {
                            primeEnemy = piece;
                            primeEnemy.KillAt = mloc; // Set saviour's capture move to this location 
                            hasToExit = true;
                            //noPrimeTarget = false;
                            break;
                        }
                    }

                    if (hasToExit) { break; }
                }
            }/////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Piece bWallSave = null;

            int bColDiff;
            int bRowDiff;
            int eColDiff;
            int eRowDiff;

            // CHECK IF ATTACK FROM ENEMY PIECE TO GENERAL CAN BE BLOCKED BY A WALL
            if (checkWallSave)
            {
                foreach (Piece piece in myPieces)
                {
                    areEqual = builderSavior.SequenceEqual(piece.CurrentPosition);
                    if (areEqual)
                    {
                        bWallSave = piece;
                        foreach (int[] bloc in bWallSave.RuleMoves) 
                        {
                            bRowDiff = Math.Abs(bloc[0] - generalPos[0]); // row difference to current piece position
                            bColDiff = Math.Abs(bloc[1] - generalPos[1]); // column difference to current piece position
                            eRowDiff = Math.Abs(primeTarget[0] - generalPos[0]); // row difference to current piece position
                            eColDiff = Math.Abs(primeTarget[1] - generalPos[1]); // column difference to current piece position

                            if ((((bloc[0] == primeTarget[0] && bloc[0] == generalPos[0]) || (bloc[1] == primeTarget[1] && bloc[1] == generalPos[1])) || (bRowDiff== bColDiff && eRowDiff == eColDiff)) && advanceBoard[bloc[0], bloc[1]] == '.')
                            {
                                bWallSave.KillAt = bloc;
                                wallSave = true; break;
                            }
                        }
                        break;
                    }
                }
            }/////////////////////////////////////////////////////////////////////////////////////////////////////////////NEW!!!!!!!!

            Piece eneGThreatnr = null;

            // LOOK FOR LOCATION WHERE ENEMY CAN BE THREATENED
            if (threatEG)
            {
                bool hasToExit = false;
                foreach (Piece piece in myPieces)
                {
                    foreach (int[] mloc in piece.Moves)
                    {
                        areEqual = threatEGLoc.SequenceEqual(mloc);
                        if (areEqual)
                        {
                            eneGThreatnr = piece;
                            eneGThreatnr.KillAt = mloc; // Set saviour's capture move to this location 
                            hasToExit = true;
                            break;
                        }
                    }

                    if (hasToExit) { break; }
                }
            }/////////////////////////////////////////////////////////////////////////////////////////////////////////////NEW!!!!!!

            if (primeEnemy != null) // if a piece with first selection of moves was found
            {
                moveTo = primeEnemy.KillAt;
                moveFrom = primeEnemy.CurrentPosition;
                charCode = primeEnemy.Code;
            }

            else if (wallSave) // if a piece with first selection of moves was found (((IF)))
            {
                moveTo = bWallSave.KillAt;
                moveFrom = bWallSave.CurrentPosition;
                charCode = bWallSave.Code;
            }

            //else if (eneGThreatnr != null) // if a piece with first selection of moves was found IF////////////////////////////////////////////////NEW!!!!!!

            //{
            //    moveTo = eneGThreatnr.KillAt;
            //    moveFrom = eneGThreatnr.CurrentPosition;
            //    charCode = eneGThreatnr.Code;
            //    Console.WriteLine("Yes we can threaten enemy G");
            //}////////////////////////////////////////////////////////////////////////////////////////////////////////////NEW!!!!!!

            else //(noPrimeTarget) 
            {
                // SORT FRIENDLY PIECES BASED ON PRIORITY LEVEL.
                List<Piece> sorted = myPieces // WHAT TO PICK TO ONE WITH HIGHEST CAPTURE SCORE (PRIORITY LEVEL) WHILE AT THE SAME TIME THE LOWEST VALUE.
                                              //Piece result = myPieces
                    .OrderByDescending(pcs => pcs.KillScore)  // Order piece objects by killScore in descending order
                    .ThenBy(pcs => pcs.Worth)             // Then order piece objects by worth in ascending order
                    .ToList(); // Convert to a list


                Piece movePiece = null;

                foreach (Piece piece in sorted)
                {
                    if (piece.Moves.Count == 0)
                    {
                        continue; // Go to the next piece if no moves available
                    }

                    movePiece = piece; // remember this piece
                    break; // Exit the loop with the first piece with available moves
                }


                if (movePiece != null) // if a piece with first selection of moves was found
                {
                    ///////////////////////////////////////////Original
                    if (movePiece.KillAt != null)
                    {
                        moveTo = movePiece.KillAt;
                    }
                    else
                    {
                        moveTo = movePiece.Moves[0]; // Do the black or white piece thing [ATTENTION!]
                    }

                    moveFrom = movePiece.CurrentPosition;
                    charCode = movePiece.Code;
                    /////////////////////////////////////////////////Original
                }
                else // Have to consider last selection of moves (from danger moves list)
                {
                    foreach (Piece piece in sorted)
                    {
                        if (piece.DangerMoves.Count == 0)
                        {
                            continue; // Skip to the next object if the property list is empty
                        }

                        movePiece = piece; // Assign the current object to the result
                        break; // Exit the loop since a non-empty property list is found
                    }
                    moveTo = movePiece.DangerMoves[0];
                    moveFrom = movePiece.CurrentPosition;
                    charCode = movePiece.Code;
                }
            }
            


            //UPDATE BOARD STATE
            if ((charCode == 'j' || charCode == 'J') && advanceBoard[moveTo[0], moveTo[1]] != '.') // It's a Jester (only program knows if friendlies are lower or upper but we are only dealing with friendly pieces here)
            {

                if (char.IsUpper(charCode) ^ char.IsUpper(advanceBoard[moveTo[0], moveTo[1]])) // if enemies - convert move
                {

                    if (char.IsLower(charCode))
                    {
                        advanceBoard[moveTo[0], moveTo[1]] = char.ToLowerInvariant(advanceBoard[moveTo[0], moveTo[1]]); // converts enemy piece
                    }

                    else
                    {
                        advanceBoard[moveTo[0], moveTo[1]] = char.ToUpperInvariant(advanceBoard[moveTo[0], moveTo[1]]); // converts enemy piece
                    }
                }
                else // friendly piece - swap move
                {
                    advanceBoard[moveFrom[0], moveFrom[1]] = advanceBoard[moveTo[0], moveTo[1]]; // swap places
                    advanceBoard[moveTo[0], moveTo[1]] = charCode;
                }

            }

            else if ((charCode == 'b' || charCode == 'B') && advanceBoard[moveTo[0], moveTo[1]] == '.' && wallSave) // It's a Jester (only program knows if friendlies are lower or upper but we are only dealing with friendly pieces here)
            {
                advanceBoard[moveTo[0], moveTo[1]] = '#';

            }

            else // What normally happens for most pieces
            {
                advanceBoard[moveTo[0], moveTo[1]] = charCode;
                advanceBoard[moveFrom[0], moveFrom[1]] = '.';

            }

            return advanceBoard; // state after move

        }

        /// <summary>
        /// Creates a 2D array representation of the board state. Also checks for invalid characters in text file.
        /// </summary>
        /// <param name="boardState"> file path containing text file </param>
        /// <returns> A 2D char array </returns>
        static char[,] GenBoardRep(string boardState, char[] expectedChars)
        {
            char[,] boardMat = new char[9, 9]; // Intialise char array of board state representation

            //string[] result = boardState.Split('\r'); // split the lines into separate arrays...ingnore comment

            using StreamReader reader = new(boardState);
            //int numRows = boardState.Split('\n').Length; // number of rows
            //Console.WriteLine("The number of rows is: {0}", numRows );
            //if (numRows > 9 || numRows < 9) // text file has wrong number of columns
            //{
            //    Console.WriteLine("Error: The file provided contains the wrong number of characters miao");
            //    // Terminate program
            //    Environment.Exit(0);
            //}

            try 
            {
                for (int i = 0; i < 9; i++)
                {
                    string currentline = reader.ReadLine();
                    int numCols = currentline.Length;
                    if (numCols > 9 || numCols < 9) // text file has wrong number of columns
                    {
                        Console.WriteLine("Error: The file provided contains the wrong number of characters");
                        // Terminate program
                        Environment.Exit(0);
                    }

                    for (int j = 0; j < 9; j++)
                    {
                        if (expectedChars.Contains(currentline[j])) // All valid characters
                        {
                            boardMat[i, j] = currentline[j];
                        }
                        else // Invalid character found!
                        {
                            Console.WriteLine("Text file provided contains invalid characters.");
                            // Terminate program
                            Environment.Exit(0);
                        }
                    }
                }
            }

            catch (IOException)
            {
                Console.WriteLine("An error occurred while attempting to read the file.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Access to file denied.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return boardMat;
        }
    }
}
