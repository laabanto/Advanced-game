using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    class PieceMove
    {
        protected internal char pieceType;
        protected internal int[] moveTo;
        protected internal int[] moveFrom;

        public PieceMove(char pieceType, int[] moveTo, int[] moveFrom)
        {
            this.pieceType = pieceType;
            this.moveTo = moveTo;
            this.moveFrom = moveFrom;
        }
    }  
}
