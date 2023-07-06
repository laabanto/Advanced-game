using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Advance
{
    class Zombie : Piece
    {
        /// <summary>
        /// Class containig attributes of the Zombie piece 
        /// </summary>
        public Zombie(char code, int[] currentPosition) : base(code, currentPosition)
        {
            worth = 1;
            this.code = code;
            this.currentPosition = currentPosition;
        }

        /// <summary>
        /// Determines all legal moves that can be made by a Zombie object within a board state
        /// </summary>
        /// <param name="boardState"></param>
        /// <param name="positionIndex"></param>
        public override void MovesSelect(char[,] boardState, int[] positionIndex)

        {
            // Useful intial variables
            List<int[]> potentialMoves = new List<int[]>();
            List<int[]> possibleLeapKills = new List<int[]>();
            List<int[]> captureMoves = new List<int[]>();
            List<int[]> nonCaptMoves = new List<int[]>();
            int step = 2;
            int end1 = 3;
            int end2 = 3;
            int minus = 1;

            // Determine direction of Zombie steps or leaps
            int dir;
            if (char.IsUpper(code))
            {
                dir = 1; // White pieces move up
            } else
            {
                dir = -1; // Black pieces move down         
            }

            //if (positionIndex[1] - (i - 1) < 0)...BORRAR


            if (positionIndex[1] - 1 < 0) // If zombie is at a cell right on the left boundary
            {
                end1 = 2;
                end2 = 2;
                minus = 0;
                step = 0;
            }

            if (positionIndex[1] - 2 < 0) // If zombie is one cell away from the left boundary
            {
                end2 = 2;
                step = 0;
            }

            if (positionIndex[1] + 1 >= boardState.GetLength(1)) // If zombie is at a cell right on the right boundary
            {
                end1 = 2;
                end2 = 2;
            }

            if (positionIndex[1] + 2 >= boardState.GetLength(1)) // If zombie is one cell away from the right boundary
            {
                end2 = 2;
            }

            ///////////////////////////////////////////////////////////////////POTENTIAL MOVES////////////////////////////////////////////////////////////////////////////////////////////
            for (int i = 0; i < end1; i++) // get potential legal moves
            {
                if (positionIndex[0] - dir < 0 || positionIndex[0] - dir >= boardState.GetLength(1)) { continue; } // if zombie has reached the opposite end of board and cannot move anymore

                else // it can still move
                {
                    int[] location1 = { positionIndex[0] - dir, positionIndex[1] + (i - minus) }; // possible move locations regarless of whether they can be made.
                    potentialMoves.Add(location1); // adding...
                    allMoves.Add(location1);
                    ruleMoves.Add(location1);
                }
            }

            for (int j = 0; j < end2; j++)
            {
                if (positionIndex[0] - 2 * dir < 0 || positionIndex[0] - 2 * dir >= boardState.GetLength(1)) { continue; } // If zombie is one cell away from reaching the opposite side of the board

                else
                {
                    int[] location2 = { positionIndex[0] - 2 * dir, positionIndex[1] + (j - step) }; // possible leap kill locations regarless of whether they can be made.
                    //legalPositions.Add(location2); // adding...
                    possibleLeapKills.Add(location2); // adding...
                    allMoves.Add(location2);
                    ruleMoves.Add(location2);

                }
                step -= 1;
            }


            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            for (int i = 0; i < potentialMoves.Count; i++) // defining which locations from the three cells in front that can actually be moved into or if a enemy piece can be captured
            {
                int[] posOut1 = potentialMoves[i];
                if (boardState[posOut1[0], posOut1[1]] == '.') // can move into
                {
                    nonCaptMoves.Add(posOut1);
                    //allMoves.Add(posOut1);
                }
                else if (boardState[posOut1[0], posOut1[1]] == '#') // can do nothing
                {
                    //allMoves.Add(posOut1);
                    continue;
                }
                else // there is a piece at this location
                {

                    if (char.IsUpper(code) ^ char.IsUpper(boardState[posOut1[0], posOut1[1]])) // if enemies
                    {
                        captureMoves.Add(posOut1);
                    }
                    else // friendly piece
                    {
                        continue; // do nothing
                    }
                }
            }

            for (int i = 0; i < possibleLeapKills.Count; i++) // defining which locations from the three cells in front that can actually be moved into or if a enemy piece can be captured
            {
                int[] posOut2 = possibleLeapKills[i];
                if (boardState[posOut2[0], posOut2[1]] == '.' || boardState[posOut2[0], posOut2[1]] == '#') // can't move into these squares
                {
                    continue;
                }
                else // there is a piece at this location
                {

                    if (char.IsUpper(code) ^ char.IsUpper(boardState[posOut2[0], posOut2[1]])) // if enemies
                    {
                        if (posOut2[1] - 2 == positionIndex[1] ) // obstructed by piece
                        {
                            if (boardState[positionIndex[0] - dir, positionIndex[1] + 1] != '.') // obstruction
                            {
                                continue;
                            }
                            else // enemy piece can be captured
                            {
                                captureMoves.Add(posOut2);
                            }
                        }
                        else if (posOut2[1] == positionIndex[1]) // obstructed by piece
                        {
                            if (boardState[positionIndex[0] - dir, positionIndex[1]] != '.') // obstruction
                            {
                                continue;
                            }
                            else // enemy piece can be captured
                            {
                                captureMoves.Add(posOut2);
                            }
                        }
                        else if (posOut2[1] + 2 == positionIndex[1]) // obstructed by piece
                        {
                            if (boardState[positionIndex[0] - dir, positionIndex[1] - 1] != '.') // obstruction
                            {
                                continue;
                            }
                            else // enemy piece can be captured
                            {
                                captureMoves.Add(posOut2);
                            }
                        }
                        else // not obstructed by another piece
                        {
                            captureMoves.Add(posOut2);
                        }                        
                    }
                    else // friendly piece
                    {
                        continue; // do nothing
                    }
                }

                Console.WriteLine("here 0:}");// BORRAR
                // remove locations from leap locations that could never be reached because another piece is in the way
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///
            // remove locations from leap locations that could never be reached because another piece is in the way
            for (int i = 0; i < possibleLeapKills.Count; i++)
            {
                int[] posOut2 = possibleLeapKills[i];
                if (posOut2[1] - 2 == positionIndex[1]) // obstructed by piece
                {
                    if (boardState[positionIndex[0] - dir, positionIndex[1] + 1] != '.') // obstruction
                    {
                        allMoves.Remove(posOut2);
                    }
                    else // enemy piece can be captured
                    {
                        continue;
                    }
                }
                if (posOut2[1] == positionIndex[1]) // obstructed by piece
                {
                    if (boardState[positionIndex[0] - dir, positionIndex[1]] != '.') // obstruction
                    {
                        allMoves.Remove(posOut2);
                    }
                    else // enemy piece can be captured
                    {
                        continue;
                    }
                }
                if (posOut2[1] + 2 == positionIndex[1]) // obstructed by piece
                {
                    //Console.WriteLine("here 1:}");// BORRAR
                    if (boardState[positionIndex[0] - dir, positionIndex[1] - 1] != '.') // obstruction
                    {
                        //Console.WriteLine("here 2:}");// BORRAR
                        allMoves.Remove(posOut2);
                    }
                    else // enemy piece can be captured
                    {
                        continue;
                    }
                }
            }///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            if (captureMoves.Count == 0)
            {
                moves = nonCaptMoves;
            }
            else
            {
                moves = captureMoves;
            }
        }

        
        
    }
}
