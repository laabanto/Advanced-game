using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Class containig attributes of the Sentinel piece 
    /// </summary>
    internal class Sentinel : Piece
    {
        //protected int worth;

        //public int Worth //{ get; private set; }
        //{
        //    get { return worth; } //Read-only outsidet this class
        //    private set
        //    { // can be changed in this class 
        //        worth = value; // Added error checking
        //    }// can be changed in this class
        //}
        public Sentinel(char code, int[] currentPosition) : base(code, currentPosition)

        {
            worth = 5;
            this.code = code;
            this.currentPosition = currentPosition;
        }

        /// <summary>
        /// Determines all legal moves that can be made by a Sentinel object within a board state
        /// </summary>
        /// <param name="boardState"></param>
        /// <param name="positionIndex"></param>
        public override void MovesSelect(char[,] boardState, int[] positionIndex)
        {
            List<int[]> protectLocs = new(); // NOTE: add as property
            List<int[]> captureLocs = new(); // NOTE: add as property
            List<int[]> possibleMoves = new(); // All moves the Sentinel can make without cosidering what is in the cell or if cell is outside of board boundaries
            List<int[]> nonCaptMoves = new();

            for (int i = -2; i < 3; i++)
            {
                for (int j = -2; j < 3; j++)
                {
                    int[] currentCell = { positionIndex[0] + i, positionIndex[1] + j }; // extract current
                    bool areEqual = positionIndex.SequenceEqual(currentCell);

                    if (areEqual) // if inspected position is the same as current position
                    {
                        continue; // next cell
                    }
                    else if (currentCell[0] < 0 || currentCell[0] >= boardState.GetLength(0) || currentCell[1] < 0 || currentCell[1] >= boardState.GetLength(1)) // if outside board boundaries
                    {
                        continue; // next cell
                    }

                    else
                    {
                        if (Math.Abs(j) % 2 == 0) // columns where locations of interest are at i = -1 and 1
                        {
                            if (Math.Abs(i) == 1)
                            {

                                if (j == 0) // Protect locations
                                {
                                    protectLocs.Add(currentCell); // I DON'T THINK WE NEED THIS AT ALL
                                }

                                else // move locations
                                {
                                    allMoves.Add(currentCell);
                                    possibleMoves.Add(currentCell);
                                }
                            }

                            else
                            {
                                continue; // illegal positions
                            }
                        }

                        else // columns where locations of interest are at i = -2, 0 and 2
                        {
                            if (Math.Abs(i) % 2 == 0)
                            {
                                if (i == 0) // Protect locations
                                {
                                    protectLocs.Add(currentCell);
                                }

                                else // move locations
                                {
                                    allMoves.Add(currentCell);
                                    possibleMoves.Add(currentCell);
                                }
                            }

                            else
                            {
                                continue; // illegal positions
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < possibleMoves.Count; i++) //checking if any locations from possible move cells have potential enemy piece captures.
            {
                int[] posOut = possibleMoves[i];
                if (boardState[posOut[0], posOut[1]] == '.') // can do nothing
                {
                    nonCaptMoves.Add(posOut);
                }
                else if (boardState[posOut[0], posOut[1]] == '#') // can do nothing
                {
                    continue;
                }
                else // there is a piece at this location
                {

                    if (char.IsUpper(code) ^ char.IsUpper(boardState[posOut[0], posOut[1]])) // if enemies
                    {
                        captureLocs.Add(posOut);
                    }
                    else // friendly piece
                    {
                        continue; 
                    }
                }

            }

            if (captureLocs.Count == 0)
            {
                moves = nonCaptMoves;
            }
            else
            {
                moves = captureLocs;
            }
        }
    }
}

