using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager
{
    public class DiggingModule : Module, ITick
    {
        public class DiggInfo
        {
            public Cell Cell;
            public float RemainingTime;
        }

        private readonly List<DiggInfo> _list = new List<DiggInfo>();
        private CellModule _cellModule;
        private PlayerModule _playerModule;
        private CoinModule _coinModule;

        public override void Initialize()
        {
            _cellModule = GetModule<CellModule>();
            _playerModule = GetModule<PlayerModule>();
            _coinModule = GetModule<CoinModule>();
        }

        public void Digg(Point point)
        {
            var cell = _cellModule.Get(point.X, point.Y);
            _list.Add(new DiggInfo
            {
                Cell = cell,
                RemainingTime = 1f
            });
        }

        public bool IsDigg(Point point)
        {
            return _list.Exists(c => c.RemainingTime > 0f && c.Cell.Point == point);
        }

        public bool AvailableToDigg(CharacterAction nextMove)
        {
            var player = _playerModule.GetModule<Player>();
            var x = player.X;
            var y = player.Y;

            switch (nextMove)
            {
                case CharacterAction.DigLeft:
                    if (y < _cellModule.Bottom &&
                        x > 0 &&
                        _cellModule.Get(x - 1, y + 1).IsNotHiddenBlock &&
                        _cellModule.Get(x - 1, y).IsEmpty &&
                        !_coinModule.IsCoin(x - 1, y))
                    {
                        return true;
                    }
                    break;
                case CharacterAction.DigRight:
                    if (y < _cellModule.Bottom &&
                        x < _cellModule.Right &&
                        _cellModule.Get(x + 1, y + 1).IsNotHiddenBlock &&
                        _cellModule.Get(x + 1, y).IsEmpty &&
                        !_coinModule.IsCoin(x + 1, y))
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }

        public void PreTick(float time)
        {
            foreach (var diggInfo in _list)
            {
                diggInfo.RemainingTime -= time;
            }
            DiggInfo info;
            while ((info = _list.FirstOrDefault(m => m.RemainingTime <= 0f)) != null)
            {
                _list.Remove(info);
            }
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
