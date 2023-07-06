
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    internal class General : Piece
    {
        /// <summary>
        /// Class containig attributes of the General piece 
        /// </summary>
        public General(char code, int[] currentPosition) : base(code, currentPosition)

        {
            worth = 10;
            this.code = code;
            this.currentPosition = currentPosition;
        }

        /// <summary>
        /// Determines all legal moves that can be made by a General object within a board state
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

                    if (areEqual) // if inspected position is the same as current position
                    {
                        continue;
                    }
                    else if (currentCell[0] < 0 || currentCell[0] >= boardState.GetLength(0) || currentCell[1] < 0 || currentCell[1] >= boardState.GetLength(1)) // if outside board boundaries
                    {
                        continue;
                    }

                    else
                    {
                        if (boardState[currentCell[0], currentCell[1]] == '.')
                        {
                            allMoves.Add(currentCell);
                            possibleMoves.Add(currentCell); //adding...
                        }
                        else if (boardState[currentCell[0], currentCell[1]] == '#')
                        {
                            allMoves.Add(currentCell);
                            //continue;
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
                            }
                        }
                    }
                }
            }

            noCaptMoves = possibleMoves; // In case General is in danger and the other no capture moves need to be considered to scape

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
