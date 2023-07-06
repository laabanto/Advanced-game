using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Class containig attributes of the Dragon piece 
    /// </summary>
    internal class Dragon:Piece
    {
        public Dragon(char code, int[] currentPosition) : base(code, currentPosition)

        {
            worth = 7;
            this.code = code;
            this.currentPosition = currentPosition;           
        }

        /// <summary>
        /// Determines all legal moves that can be made by a Dragon object within a board state
        /// </summary>
        /// <param name="boardState"></param>
        /// <param name="positionIndex"></param>

        public override void MovesSelect(char[,] boardState, int[] positionIndex)
        {
            int stopi1 = 8;
            int stopi2 = 8;
            int stopi3 = 8;
            int stopi4 = 8;

            int colDiff;
            int rowDiff;

            List<int[]> allPotentialMoves = new List<int[]>();

            List<int[]> northDir = new List<int[]>();
            List<int[]> southDir = new List<int[]>();
            List<int[]> eastDir = new List<int[]>();
            List<int[]> westDir = new List<int[]>();
            List<int[]> northWestDir = new List<int[]>();
            List<int[]> southWestDir = new List<int[]>();
            List<int[]> northEastDir = new List<int[]>();
            List<int[]> southEastDir = new List<int[]>();

            List<int[]> captureLocs = new List<int[]>();
            List<int[]> nonCaptureLocs = new List<int[]>();


            //COLLECTING POSSIBLE MOVES IN ALL EIGHT DIRECTIONS. DOES NOT INCLUDE 8 IMMEDIATE SQUARES (NO CAPTURE)
            for (int i = 0; i < 9; i++) // for all rows
            {
                for (int j = 0; j < 9; j++) // for all columns
                {
                    rowDiff = Math.Abs(i - positionIndex[0]); // row difference to current piece position
                    colDiff = Math.Abs(j - positionIndex[1]); // column difference to current piece position

                    if (i == positionIndex[0] && j > positionIndex[1]) // EAST positionIndex[1] + 1 // DONE
                    {
                        if (j > stopi1) // locations blocked by a piece
                        {
                            continue;
                        }

                        else if (j == positionIndex[1] + 1) // locations blocked by a piece
                        {
                            if (boardState[i, j] != '.') // immidiate location blocked by a piece
                            {
                                stopi1 = j;
                            }

                            else // empty square
                            {
                                int[] eastLoc = { i, j };
                                nonCaptureLocs.Add(eastLoc);
                            }
                        }

                        else
                        {
                            if (boardState[i, j] != '.') // locations blocked by a piece
                            {
                                stopi1 = j;
                            }
                            int[] eastLoc = { i, j };
                            eastDir.Add(eastLoc);
                        }                       
                    }

                    else if (rowDiff == colDiff && (i > positionIndex[0] && j > positionIndex[1])) // SOUTH-EAST j > positionIndex[1] + 1 DONE
                    {
                        if (j > stopi2) // locations blocked by a piece
                        {
                            continue;
                        }

                        else if (j == positionIndex[1] + 1) // locations blocked by a piece
                        {
                            if (boardState[i, j] != '.') // immidiate location blocked by a piece
                            {
                                stopi2 = j;
                            }

                            else // empty square
                            {
                                int[] southELoc = { i, j };
                                nonCaptureLocs.Add(southELoc);
                            }
                        }

                        else
                        {
                            if (boardState[i, j] != '.') // locations blocked by a piece
                            {
                                stopi2 = j;
                            }
                            int[] southELoc = { i, j };
                            southEastDir.Add(southELoc);
                        }
                    }


                    else if (j == positionIndex[1] && i > positionIndex[0]) // SOUTH | && i != 8 | i > positionIndex[0] + 1 // DONE
                    {
                        if (i > stopi3) // locations blocked by a piece
                        {
                            continue;
                        }

                        else if (i == positionIndex[0] + 1) // locations blocked by a piece
                        {
                            if (boardState[i, j] != '.') // immidiate location blocked by a piece
                            {
                                stopi3 = i; // Not added to All moves because Dragon can't capture on immidiate squares
                            }

                            else // empty square
                            {
                                int[] southLoc = { i, j };
                                nonCaptureLocs.Add(southLoc);
                            }
                        }

                        else
                        {
                            if (boardState[i, j] != '.') // locations blocked by a piece
                            {
                                stopi3 = i;
                            }
                            int[] southLoc = { i, j };
                            southDir.Add(southLoc);
                        }
                    }

                    else if (rowDiff == colDiff && (i > positionIndex[0] && j < positionIndex[1])) // SOUTH-WEST //DONE
                    {
                        if (i > stopi4) // locations blocked by a piece
                        {
                            continue;
                        }

                        else if (i == positionIndex[0] + 1) // locations blocked by a piece
                        {
                            if (boardState[i, j] != '.') // immidiate location blocked by a piece
                            {
                                stopi4 = i;
                            }

                            else // empty square
                            {
                                int[] southWLoc = { i, j };
                                nonCaptureLocs.Add(southWLoc);
                            }
                        }

                        else
                        {
                            if (boardState[i, j] != '.') // locations blocked by a piece
                            {
                                stopi4 = i;
                            }
                            int[] southWLoc = { i, j };
                            southWestDir.Add(southWLoc);
                        }
                    }

                    else if (rowDiff == colDiff && (i < positionIndex[0] && j < positionIndex[1]))  // NORTH-WEST | && continueNW // DONE ///////////////
                    {
                        if (i == positionIndex[0] - 1) // locations blocked by a piece
                        {
                            if (boardState[i, j] != '.') // immidiate location blocked by a piece
                            {
                                northWestDir.Clear();
                            }

                            else // empty square
                            {
                                int[] northWLoc = { i, j };
                                nonCaptureLocs.Add(northWLoc);
                            }
                        }

                        else // non immidiate possible capture locations
                        {                      
                            if (boardState[i, j] != '.') // locations blocked by a piece
                            {
                                northWestDir.Clear(); // delete all previously added
                                int[] northWLoc = { i, j };
                                northWestDir.Add(northWLoc);
                            }

                            else
                            {
                                int[] northWLoc = { i, j };
                                northWestDir.Add(northWLoc);
                            }
                        }                       
                    }

                    else if (j == positionIndex[1] && i < positionIndex[0] - 1) // NORTH. //DONE && continueN
                    {
                        if (i == positionIndex[0] - 1) // locations blocked by a piece
                        {
                            if (boardState[i, j] != '.') // immidiate location blocked by a piece
                            {
                                northDir.Clear();
                            }

                            else // empty square
                            {
                                int[] northLoc = { i, j };
                                nonCaptureLocs.Add(northLoc);
                            }
                        }

                        else // non immidiate possible capture locations
                        {
                            if (boardState[i, j] != '.') // locations blocked by a piece
                            {
                                northDir.Clear(); // delete all previously added
                                int[] northLoc = { i, j };
                                northDir.Add(northLoc);
                            }

                            else
                            {
                                int[] northLoc = { i, j };
                                northDir.Add(northLoc);
                            }
                        }                        
                    }

                    else if (rowDiff == colDiff && (i < positionIndex[0] - 1 && j > positionIndex[1] + 1)) // NORTH-EAST //DONE && continueNE
                    {
                        if (i == positionIndex[0] - 1) // locations blocked by a piece
                        {
                            if (boardState[i, j] != '.') // immidiate location blocked by a piece
                            {
                                northEastDir.Clear();
                            }

                            else // empty square
                            {
                                int[] northELoc = { i, j };
                                nonCaptureLocs.Add(northELoc);
                            }
                        }

                        else // non immidiate possible capture locations
                        {
                            if (boardState[i, j] != '.') // locations blocked by a piece
                            {
                                northEastDir.Clear(); // delete all previously added
                                int[] northELoc = { i, j };
                                northEastDir.Add(northELoc);
                            }

                            else
                            {
                                int[] northELoc = { i, j };
                                northEastDir.Add(northELoc);
                            }
                        }                        
                    }

                    else if (i == positionIndex[0] && j < positionIndex[1] - 1) // WEST | //&& j != 0 // DONE && continueW
                    {
                        if (i == positionIndex[0] - 1) // locations blocked by a piece
                        {
                            if (boardState[i, j] != '.') // immidiate location blocked by a piece
                            {
                                westDir.Clear();
                            }

                            else // empty square
                            {
                                int[] westLoc = { i, j };
                                nonCaptureLocs.Add(westLoc);
                            }
                        }

                        else // non immidiate possible capture locations
                        {
                            if (boardState[i, j] != '.') // locations blocked by a piece
                            {
                                westDir.Clear(); // delete all previously added
                                int[] westLoc = { i, j };
                                westDir.Add(westLoc);
                                //allMoves.Add(westLoc);
                            }

                            else
                            {
                                int[] westLoc = { i, j };
                                westDir.Add(westLoc);
                                //allMoves.Add(westLoc);
                            }
                        }
                    }

                    else
                    {
                        continue; // ignore all other posibilities 
                    }
                }
            }//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            // Combine all posible moves
            allPotentialMoves.AddRange(eastDir);
            allPotentialMoves.AddRange(southDir);
            allPotentialMoves.AddRange(northDir);
            allPotentialMoves.AddRange(westDir);
            allPotentialMoves.AddRange(northEastDir);
            allPotentialMoves.AddRange(southEastDir);
            allPotentialMoves.AddRange(northWestDir);
            allPotentialMoves.AddRange(southWestDir);

            allMoves.AddRange(allPotentialMoves);

            foreach (int[] loc in allPotentialMoves)
            {
                if (boardState[loc[0], loc[1]] == '.')
                {   
                    //allMoves.Add(loc);
                    nonCaptureLocs.Add(loc); //adding...
                }

                else if (boardState[loc[0], loc[1]] == '#')
                {
                    //allMoves.Add(loc); // can't capture. Only blocks
                    continue;
                }

                else // A piece is here
                {
                    if (char.IsUpper(code) ^ char.IsUpper(boardState[loc[0], loc[1]])) // if enemies
                    {
                        captureLocs.Add(loc);
                    }
                    else
                    {
                        //allMoves.Add(loc);
                        continue;
                    }
                }
            }           

            if (captureLocs.Count == 0)
            {
                moves = nonCaptureLocs;
            }
            else
            {
                moves = captureLocs;
            }
        }
    }
}

