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
        public bool IsHide;

        public Cell(Point point)
        {
            Point = point;
        }
        
        public bool IsEmpty
        {
            get { return Type == CellType.Empty; }
        }

        public bool IsBlock
        {
            get { return Type == CellType.Block; }
        }

        public bool IsNotHiddenBlock
        {
            get { return IsBlock && !IsHide; }
        }

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
