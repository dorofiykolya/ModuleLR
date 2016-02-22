using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager
{
    public class CellModule : Module
    {
        public override void Initialize()
        {

        }

        public void Set(Cell[,] cells)
        {

        }

        public Cell Get(int x, int y)
        {
            return null;
        }

        public Cell this[int x, int y] { get { return Get(x, y); } }
    }
}
