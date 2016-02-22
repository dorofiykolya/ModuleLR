using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager
{
    class TickModule : Module
    {
        private ITick[] _ticks;
        private int _lastTime = 0;

        public override void Initialize()
        {
            _ticks = GetModules<ITick>();
        }

        public int Ticks { get; private set; }

        public void Tick()
        {
            Ticks++;
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

            PreTick(deltaTime);
            Tick(deltaTime);
            PostTick(deltaTime);
            FinalTick(deltaTime);
        }

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
