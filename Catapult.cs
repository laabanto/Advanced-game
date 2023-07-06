using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    internal class Catapult : Piece
    {
        /// <summary>
        /// Class containig attributes of the Catapult piece 
        /// </summary>
        public Catapult(char code, int[] currentPosition) : base(code, currentPosition)

        {
            worth = 6;
            this.code = code;
            this.currentPosition = currentPosition;
        }

        /// <summary>
        /// Determines all legal moves that can be made by a Catapult object within a board state
        /// </summary>
        /// <param name="boardState"></param>
        /// <param name="positionIndex"></param>
        public override void MovesSelect(char[,] boardState, int[] positionIndex)
        {
            List<int[]> captureMoves = new List<int[]>();
            List<int[]> nonCaptMoves = new List<int[]>();

            List<int[]> potentialMoves = new List<int[]>();
            List<int[]> possibleKills = new List<int[]>();

            int colDiff;
            int rowDiff;

            for (int i = 0; i < 9; i++) // for all rows
            {
                for (int j = 0; j < 9; j++) // for all columns
                {
                    rowDiff = Math.Abs(i - positionIndex[0]); // row difference to current piece position
                    colDiff = Math.Abs(j - positionIndex[1]); // column difference to current piece position

                    //IMMIDIATE MOVE ONLY LOCATIONS (NO CAPTURE)///////////////////////////////////////////////////////////
                    if (i == positionIndex[0] && j == positionIndex[1] + 1) // EAST
                    {
                        int[] loc = { i, j };
                        potentialMoves.Add(loc);
                    }


                    else if (j == positionIndex[1] && i == positionIndex[0] + 1) // SOUTH 
                    {
                        int[] loc = { i, j };
                        potentialMoves.Add(loc);
                    }

                    else if (j == positionIndex[1] && i == positionIndex[0] - 1) // NORTH
                    {
                        int[] loc = { i, j };
                        potentialMoves.Add(loc);
                    }

                    else if (i == positionIndex[0] && j == positionIndex[1] - 1) // WEST 
                    {
                        int[] loc = { i, j };
                        potentialMoves.Add(loc);
                    }
                    //CATAPULT ATTACK LOCATIONS//////////////////////////////////////////////////////////////////////////////////////////
                    else if (i == positionIndex[0] && j - positionIndex[1] == 3) // EAST
                    {
                        int[] loc = { i, j };
                        possibleKills.Add(loc);
                        allMoves.Add(loc);
                    }

                    else if (rowDiff == colDiff && (i - positionIndex[0] == 2 && j - positionIndex[1] == 2)) // SOUTH-EAST
                    {
                        int[] loc = { i, j };
                        possibleKills.Add(loc);
                        allMoves.Add(loc);
                    }


                    else if (j == positionIndex[1] && i - positionIndex[0] == 3) // SOUTH | && i != 8 
                    {
                        int[] loc = { i, j };
                        possibleKills.Add(loc);
                        allMoves.Add(loc);
                    }

                    else if (rowDiff == colDiff && (i - positionIndex[0] == 2 && positionIndex[1] - j  == 2)) // SOUTH-WEST
                    {
                        int[] loc = { i, j };
                        possibleKills.Add(loc);
                        allMoves.Add(loc);
                    }

                    else if (rowDiff == colDiff && (positionIndex[0] - i == 2 && positionIndex[1] - j == 2))  // NORTH-WEST
                    {
                        int[] loc = { i, j };
                        possibleKills.Add(loc);
                        allMoves.Add(loc);
                    }

                    else if (j == positionIndex[1] && positionIndex[0] - i == 3) // NORTH. Not including cell on board edge
                    {
                        int[] loc = { i, j };
                        possibleKills.Add(loc);
                        allMoves.Add(loc);
                    }

                    else if (rowDiff == colDiff && (positionIndex[0] - i == 2 && j - positionIndex[1] == 2)) // NORTH-EAST
                    {
                        int[] loc = { i, j };
                        possibleKills.Add(loc);
                        allMoves.Add(loc);
                    }

                    else if (i == positionIndex[0] && positionIndex[1] - j == 3) // WEST | //&& j != 0
                    {
                        int[] loc = { i, j };
                        possibleKills.Add(loc);
                        allMoves.Add(loc);
                    }///////////////////////////////////////////////////////////////////////////////////////////////////////


                    else
                    {
                        continue; // ignore all other posibilities 
                    }
                }
            }

            for (int i = 0; i < potentialMoves.Count; i++) // defining which locations from the three cells in front that can actually be moved into or if a enemy piece can be captured
            {
                int[] posOut1 = potentialMoves[i];
                if (boardState[posOut1[0], posOut1[1]] == '.') // can move into
                {
                    nonCaptMoves.Add(posOut1);
                    //allMoves.Add(posOut1);
                }
              
                else // there is a piece at this location
                {
                        continue; // do nothing
                }
            }

            for (int i = 0; i < possibleKills.Count; i++) // defining which locations from the three cells in front that can actually be moved into or if a enemy piece can be captured
            {
                int[] posOut2 = possibleKills[i];
                if (boardState[posOut2[0], posOut2[1]] == '.' || boardState[posOut2[0], posOut2[1]] == '#') // not an enemy in sight
                {
                    continue;
                }

                else // there is a piece at this location
                {

                    if (char.IsUpper(code) ^ char.IsUpper(boardState[posOut2[0], posOut2[1]])) // if enemies
                    {
                        captureMoves.Add(posOut2);
                    }
                    else // friendly piece
                    {
                        continue; // do nothing
                    }
                }
            }


            if (captureMoves.Count == 0)
            {
                canCaptOrConvrt = false;
                moves = nonCaptMoves;
            }
            else
            {
                moves = captureMoves; 
            }
        }
    }
}
