using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager
{
    public class TickModule : ModuleManager
    {
        private ITick[] _ticks;
        private int _lastTime = 0;
        private float _deltaTime;

        public override void Initialize()
        {
            _ticks = GetModules<ITick>();
        }

        public int Ticks { get; private set; }

        public virtual void Tick()
        {
            AdvanceTime();
            AdvanceTick();
        }

        private void CalculateDeltaTime()
        {
            float deltaTime = 0f;
            var time = Environment.TickCount;
            if (_lastTime == 0)
            {
                _lastTime = time;
            }
            else
            {
                deltaTime = (time - _lastTime) / 1000f;
            }
            _deltaTime = deltaTime;
        }

        protected virtual void AdvanceTime()
        {
            CalculateDeltaTime();
        }

        protected virtual void AdvanceTick()
        {
            Ticks++;
            PreTick(_deltaTime);
            Tick(_deltaTime);
            PostTick(_deltaTime);
            FinalTick(_deltaTime);
        }

        protected float DeltaTime { get { return _deltaTime; } }

        private void PreTick(float time)
        {
            foreach (var tick in _ticks)
            {
                tick.PreTick(time);
            }
        }

        private void Tick(float time)
        {
            foreach (var tick in _ticks)
            {
                tick.Tick(time);
            }
        }

        private void PostTick(float time)
        {
            foreach (var tick in _ticks)
            {
                tick.PostTick(time);
            }
        }

        private void FinalTick(float time)
        {
            foreach (var tick in _ticks)
            {
                tick.FinalTick(time);
            }
        }
    }
}
