using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager
{
    public class Events
    {
    }

    public class PlayerAnimationEvent : Event
    {
        public enum AnimationEvent
        {
            RunLeft,
            RunRight,
            RunUpDn,
            BarLeft,
            BarRight,
            FallLeft,
            FallRight,
            Stop,
            Dead
        }

        public PlayerAnimationEvent(AnimationEvent evt)
        {
            Event = evt;
        }

        public AnimationEvent Event;
    }
}
