using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Class containig attributes of the Miner piece 
    /// </summary>
    internal class Miner : Piece
    {
        public Miner(char code, int[] currentPosition) : base(code, currentPosition)

        {
            worth = 4;
            this.code = code;
            this.currentPosition = currentPosition;
        }

        /// <summary>
        /// Determines all legal moves that can be made by a Miner object within a board state
        /// </summary>
        /// <param name="boardState"></param>
        /// <param name="positionIndex"></param>
        public override void MovesSelect(char[,] boardState, int[] positionIndex)
        {
            int stopi1 = 8;
            int stopi2 = 8;

            List<int[]> allPotentialMoves = new List<int[]>();
            List<int[]> northDir = new List<int[]>();
            List<int[]> southDir = new List<int[]>();
            List<int[]> eastDir = new List<int[]>();
            List<int[]> westDir = new List<int[]>();
            List<int[]> captureLocs = new List<int[]>();
            List<int[]> nonCaptureLocs = new List<int[]>();

            for (int i = 0; i < 9; i++) // for all rows
            {
                for (int j = 0; j < 9; j++) // for all columns
                {
                    if (i == positionIndex[0] && j > positionIndex[1]) // EAST
                    {
                       if (j > stopi1) // locations blocked by a piece
                        {
                            continue;
                        }

                        else
                        {
                            if (boardState[i, j] != '.') // locations blocked by a piece
                            {
                                stopi1 = j;
                            }
                            int[] eastLoc = { i, j }; // Add last piece in this direction
                            eastDir.Add(eastLoc);
                        }

                    }


                    else if (j == positionIndex[1] && i > positionIndex[0]) // SOUTH | && i != 8 
                    {
                        if (i > stopi2) // locations blocked by a piece
                        {
                            continue;
                        }

                        else
                        {
                            if (boardState[i, j] != '.') // locations blocked by a piece
                            {
                                stopi2 = i;
                            }
                            int[] southLoc = { i, j }; // Add last piece in this direction
                            southDir.Add(southLoc);
                        }
                    }

                    else if (j == positionIndex[1] && i < positionIndex[0]) // NORTH. Not including cell on board edge
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

                    else if (i == positionIndex[0] && j < positionIndex[1]) // WEST | //&& j != 0
                    {
                        if (boardState[i, j] != '.') // locations blocked by a piece
                        {
                            westDir.Clear(); // delete all previously added
                            int[] westLoc = { i, j };
                            westDir.Add(westLoc);
                        }

                        else
                        {
                            int[] westLoc = { i, j };
                            westDir.Add(westLoc);
                        }
                    }


                    else
                    {
                        continue; // ignore all other posibilities 
                    }
                }
            }
            // Combine all posible moves
            allPotentialMoves.AddRange(eastDir);
            allPotentialMoves.AddRange(southDir);
            allPotentialMoves.AddRange(northDir);
            allPotentialMoves.AddRange(westDir);

            allMoves.AddRange(allPotentialMoves);

            foreach (int[] loc in allPotentialMoves)
            {
                if (boardState[loc[0], loc[1]] == '.' || boardState[loc[0], loc[1]] == '#') // wall can be moved into. In essence capturing them
                {
                    //allMoves.Add(loc);
                    nonCaptureLocs.Add(loc); //adding...
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


