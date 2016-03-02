using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager
{
    public class Cell
    {
        public CellType Type;
        public Point Point;

        public Cell(Point point)
        {
            Point = point;
        }

        public bool IsRemovable
        {
            get { return Type == CellType.Block; }
        }

        public bool IsEmpty
        {
            get { return Type == CellType.Empty; }
        }

        public bool IsAvailableToMove
        {
            get
            {
                switch (Type)
                {

                    case CellType.Empty:
                    case CellType.HLadr:
                    case CellType.Ladder:
                    case CellType.RopeBar:
                    case CellType.Teleport:
                        return true;
                    case CellType.Trap:
                    case CellType.Block:
                    case CellType.Solid:
                        return false;
                }
                return false;
            }
        }

        public bool IsBlock { get { return Type == CellType.Block; } }

        public bool Any(params CellType[] types)
        {
            foreach (var type in types)
            {
                if (type == Type) return true;
            }
            return false;
        }
    }
}
