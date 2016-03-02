using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager
{
    public class TimeModule : Module, ITick
    {
        private float _remainigTime = float.NaN;

        public bool IsTimeout
        {
            get { return !float.IsNaN(_remainigTime) && _remainigTime <= 0f; }
        }

        public float RemainigTime
        {
            get { return _remainigTime; }
            set { _remainigTime = value; }
        }

        public override void Initialize()
        {
            _remainigTime = float.NaN;
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
