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

    public class DiggingCellEvent : Event
    {
        public int X;
        public int Y;

        public DiggingCellEvent(int x, int y)
        {
            X = x;
            Y = y;
        }
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
            Dead,
            DigLeft,
            DigRight
        }

        public PlayerAnimationEvent(AnimationEvent evt)
        {
            Event = evt;
        }

        public AnimationEvent Event;
    }
}
