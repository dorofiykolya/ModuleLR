using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager
{
    public class LevelModule : TickModule
    {
        public bool IsPause { get; set; }
        public void Pause()
        {
            IsPause = true;
        }

        public void Resume()
        {
            IsPause = false;
        }

        public override void Tick()
        {
            AdvanceTime();
            if (!IsPause)
            {
                AdvanceTick();
            }
        }
    }
}
