using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Abstract Class containig all common attributes possessed by all pieces 
    /// </summary>
    abstract class Piece
    {
        protected int worth; // holds the value of a piece
        protected char code; // Character specifying piece type. Can be lower or upper case to designate team colour
        protected int[] currentPosition; // currrent position of piece object on board
        protected bool canCaptOrConvrt; // Can a piece object on board capture or convert(Jester)
        protected bool eminentThreat; // Piece is being Threatened by enemy piece
        protected bool bF; // (Builder friend) Use for General. General might be saved by a wall to block attack
        protected int[]? bFAt; // (Builder friend) Use for General. General might be saved by a wall to block attack
        protected int[]? killAt; // Piece can capture an enemy piece 
        protected int[]? beKilledAt; // if could be captured if it moves to a location
        protected int killScore; //Could also call priority level
        protected List<int[]> moves; // List of CAPTURE moves
        protected List<int[]> noCaptMoves; // List of NO CAPTURE moves. Used for THE GENERAL
        protected List<int[]> allMoves; // List of ALL other posible moves
        protected List<int[]> ruleMoves; // List of ALL moves within piece's move pattern
        protected List<int[]> dangerMoves; // List of possible but dangerous moves
        protected bool sentinelProtect; // Each piece checks it they are 

        protected internal int Worth //{ get; private set; }
        {
            get { return worth; } 
            private set
            { // can be changed in this class 
                worth = value; // Added error checking
            }// can be changed in this class
        }

        protected internal char Code
        {
            get { return code; } 
            private set
            { // can be changed in this class 
                code = value; 
            }// can be changed in this class
        }

        protected internal int[] CurrentPosition
        {
            get { return currentPosition; } 
            private set
            { // can be changed in this class 
                currentPosition = value; 
            }// can be changed in this class
        }

        protected internal bool CanCaptOrConvrt
        {
            get { return canCaptOrConvrt; } //Read-only outsidet this class
        }
        protected internal bool EminentThreat
        {
            get { return eminentThreat; } 
            set
            { // can be changed in this class 
                eminentThreat = value; 
            }
        } 
        protected internal bool BF
        {
            get { return bF; } //Read-only outsidet this class
        } 
        protected internal int[]? BFAt
        {
            get { return bFAt; } //Read-only outsidet this class

        }
        protected internal int[]? KillAt
        {
            get { return killAt; }
            set
            { // can be changed in this class 
                killAt = value;
            }
        }
 
        protected internal int KillScore
        {
            get { return killScore; } 
            set
            { // can be changed in this class 
                killScore = value; 
            }
        } 
        protected internal List<int[]> Moves
        {
            get { return moves; } 
            private set
            { // can be changed in this class 
                moves = value; 
            }
        }
        protected internal List<int[]> NoCaptMoves
        {
            get { return noCaptMoves; } 
            private set
            { // can be changed in this class 
                noCaptMoves = value; 
            }
        } 
        protected internal List<int[]> AllMoves
        {
            get { return allMoves; } 
            private set
            { // can be changed in this class 
                allMoves = value; 
            }
        } 
        protected internal List<int[]> RuleMoves
        {
            get { return ruleMoves; } //Read-only outsidet this class
        } 
        protected internal List<int[]> DangerMoves
        {
            get { return dangerMoves; } //Read-only outsidet this class
            private set
            { // can be changed in this class 
                dangerMoves = value;
            }
        } 
        protected internal bool SentinelProtect
        {
            get { return sentinelProtect; } //Read-only outsidet this class
        }

        public Piece(char code, int[] currentposition)
        {
            this.code = code;
            this.currentPosition = currentposition;
            eminentThreat = false;
            killScore = 0;
            moves = new List<int[]>();
            dangerMoves = new List<int[]>();
            noCaptMoves = new List<int[]>();
            allMoves = new List<int[]>();
            ruleMoves = new List<int[]>();
            sentinelProtect = false;
            canCaptOrConvrt = true;  


        }
        /// <summary>
        /// Determines all legal moves that can be made by piece assuming current board state
        /// </summary>
        /// <param name="valuesMat"></param>
        /// <param name="boardState"></param>
        /// <param name="positionIndex"></param>
        public abstract void MovesSelect(char[,] boardState, int[] positionIndex);

        /// <summary>
        /// Checks if a piece is protected by a Sentinel
        /// </summary>
        /// <param name="boardState"></param>
        /// <param name="positionIndex"></param>
        //public abstract void IsProtected(char[,] boardState, int[] positionIndex);
        public void IsProtected(char[,] boardState, int[] positionIndex)
        {

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int[] currentCell = { positionIndex[0] + i, positionIndex[1] + j }; // extract current position

                    if (positionIndex[0] == currentCell[0] || positionIndex[1] == currentCell[1])
                    {
                        if (positionIndex == currentCell) // if inspected position is the same as current position
                        {
                            continue; // next cell
                        }
                        else if (currentCell[0] < 0 || currentCell[0] >= boardState.GetLength(0) || currentCell[1] < 0 || currentCell[1] >= boardState.GetLength(1)) // if outside board boundaries
                        {
                            continue; // next cell
                        }

                        else
                        {
                            if (boardState[currentCell[0], currentCell[1]] == 's' || boardState[currentCell[0], currentCell[1]] == 'S') // Sentinel in adjacent cell
                            {
                                char sentiCode = boardState[currentCell[0], currentCell[1]];

                                if (char.IsUpper(code) ^ char.IsUpper(sentiCode)) // actually an enemy Sentinel
                                {
                                    continue; // next cell
                                }
                                else
                                {
                                    sentinelProtect = true; // Piece is protected (can't touch this!)
                                }
                            }
                            else
                            {
                                continue; // next cell
                            }
                        }
                    }
                    else
                    {
                        continue; // not a within protection range
                    }
                }
            }
        }

        /// <summary>
        /// Checks if a piece is a Builder friend by its side
        /// </summary>
        /// <param name="boardState"></param>
        /// <param name="positionIndex"></param>
        public void BuilderFriend(char[,] boardState, int[] positionIndex)
        {

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
                        if (boardState[currentCell[0], currentCell[1]] == 'b' || boardState[currentCell[0], currentCell[1]] == 'B') // A builder found
                        {
                            char builderCode = boardState[currentCell[0], currentCell[1]];

                            if (char.IsUpper(code) ^ char.IsUpper(builderCode)) // actually an enemy Builder
                            {
                                continue; // next cell
                            }
                            else
                            {
                                bF = true; // Piece has buider friend by it's side
                                bFAt = currentCell; //remember Builder friend's location

                            }
                        }

                        else // ignore all others
                        {
                            continue;
                        }
                    }
                }
            }
        }
    }
}
