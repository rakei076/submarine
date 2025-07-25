using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace submarine
{
    internal struct Position
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public Position(int row, int col)
        {
            Row = row;
            Column = col;
        }

        public bool Equals(Position other)
        {
            return Row == other.Row && Column == other.Column;
        }

        public bool IsValid()
        {
            return 0 <= Row && Row < Program.ROW_SIZE
                && 0 <= Column && Column < Program.COLUMN_SIZE;
        }
    }
} 