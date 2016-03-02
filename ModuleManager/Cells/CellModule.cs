using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ModuleManager
{
    public class CellModule : Module, ITick
    {
        private Cell[,] _map;
        private TeleportModule _teleport;
        private Point Size;

        public CellModule()
        {

        }

        public int Left { get { return 0; } }
        public int Top { get { return 0; } }
        public int Right { get { return Size.X; } }
        public int Bottom { get { return Size.Y; } }

        public override void Initialize()
        {
            _teleport = GetModule<TeleportModule>();
        }

        public Cell Get(int x, int y)
        {
            return _map[x, y];
        }

        public bool IsEmpty(int x, int y)
        {
            return Get(x, y).IsEmpty;
        }

        public Cell this[int x, int y] { get { return Get(x, y); } }

        public int SizeX { get { return Size.X; } }
        public int SizeY { get { return Size.Y; } }
        public float TileW { get { return 1; } }
        public float TileH { get { return 1; } }
        public float W2 { get { return (1f / 2f); } }
        public float H2 { get { return (1f / 2f); } }
        public float W4 { get { return (1f / 4f); } }
        public float H4 { get { return (1f / 4f); } }
        public int MaxTileX { get { return SizeX - 1; } }
        public int MaxTileY { get { return SizeY - 1; } }
        public float TileScale { get { return 1; } }

        public void Set(Point size, CellRecord[] cells)
        {
            Size = size;
            _map = new Cell[size.X, size.Y];
            for (int x = 0; x < size.X; x++)
            {
                for (int y = 0; y < size.Y; y++)
                {
                    _map[x, y] = new Cell(new Point { X = x, Y = y }) { Type = CellType.Empty };
                }
            }

            foreach (var cell in cells)
            {
                _map[cell.Point.X, cell.Point.Y] = new Cell(cell.Point)
                {
                    Type = cell.CellType
                };
            }
        }

        public void ShowHiddenLadder()
        {
            
        }

        public void PreTick(float time)
        {

        }

        public void Tick(float time)
        {

        }

        public void PostTick(float time)
        {

        }

        public void FinalTick(float time)
        {

        }
    }
}
