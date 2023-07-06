using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    internal class Jester : Piece
    {
        /// <summary>
        /// Class containig attributes of the Jester piece 
        /// </summary>
        public Jester(char code, int[] currentPosition) : base(code, currentPosition)

        {
            worth = 3;
            this.code = code;
            this.currentPosition = currentPosition;
        }

        /// <summary>
        /// Determines all legal moves that can be made by a Jester object within a board state
        /// </summary>
        /// <param name="boardState"></param>
        /// <param name="positionIndex"></param>
        public override void MovesSelect(char[,] boardState, int[] positionIndex)
        {
            List<int[]> convertOrSwap = new List<int[]>();
            List<int[]> possibleMoves = new List<int[]>();

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int[] currentCell = { positionIndex[0] + i, positionIndex[1] + j }; // extract current
                    bool areEqual = positionIndex.SequenceEqual(currentCell);

                    if (areEqual) // if inspected position is the same as current position
                    {
                        //Console.WriteLine("They are equal u dummie");
                        continue;
                    }
                    else if (currentCell[0] < 0 || currentCell[0] >= boardState.GetLength(0) || currentCell[1] < 0 || currentCell[1] >= boardState.GetLength(1)) // if outside board boundaries
                    {
                        continue;
                    }

                    else
                    {
                        allMoves.Add(currentCell);
                    
                        if (boardState[currentCell[0], currentCell[1]] == '.')
                        {
                            possibleMoves.Add(currentCell); //adding...
                        }
                        else if (boardState[currentCell[0], currentCell[1]] == '#') // can't do nothing with a wall
                        {
                            continue;
                        }
                        else // A piece is here. includes friendly and enemy pieces
                        {
                            convertOrSwap.Add(currentCell); //adding...
                        }
                    }
                }
            }

            if (convertOrSwap.Count == 0)
            {
                moves = possibleMoves;
            }
            else
            {
                moves = convertOrSwap;
            }
        }
    }
}
