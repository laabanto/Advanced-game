using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Class containig attributes of the Builder piece 
    /// </summary>
    class Builder : Piece
    {
        public Builder(char code, int[] currentPosition) : base(code, currentPosition)

        {
            worth = 2;
            this.code = code;
            this.currentPosition = currentPosition;
        }

        /// <summary>
        /// Determines all legal moves that can be made by a Builder object within a board state
        /// </summary>
        /// <param name="boardState"></param>
        /// <param name="positionIndex"></param>
        public override void MovesSelect(char[,] boardState, int[] positionIndex)
        {
            List<int[]> captureLocations = new List<int[]>();
            List<int[]> possibleMoves = new List<int[]>();

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int[] currentCell = { positionIndex[0] + i, positionIndex[1] + j }; // extract current
                    bool areEqual = positionIndex.SequenceEqual(currentCell);

                    if (areEqual) // if inspected position is the same as current position...do not include
                    {
                        continue;
                    }
                    else if (currentCell[0] < 0 || currentCell[0] >= boardState.GetLength(0) || currentCell[1] < 0 || currentCell[1] >= boardState.GetLength(1)) // if outside board boundaries
                    {
                        continue;
                    }

                    else
                    {
                        ruleMoves.Add(currentCell);

                        if (boardState[currentCell[0], currentCell[1]] == '.')
                        {
                            possibleMoves.Add(currentCell); //adding...
                            allMoves.Add(currentCell);

                        }

                        else if (boardState[currentCell[0], currentCell[1]] == '#') // can do nothing
                        {
                            allMoves.Add(currentCell);
                        }

                        else // A piece is here
                        {
                            if (char.IsUpper(code) ^ char.IsUpper(boardState[currentCell[0], currentCell[1]])) // if enemies
                            {
                                captureLocations.Add(currentCell);
                            }
                            else
                            {
                                allMoves.Add(currentCell);
                                //continue;
                            }
                        }
                    }
                }
            }

            if (captureLocations.Count == 0)
            {
                moves = possibleMoves;
            }
            else
            {
                moves = captureLocations;
            }
        }
    }
}
