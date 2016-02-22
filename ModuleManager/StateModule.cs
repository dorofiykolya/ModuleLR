using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager
{
    public class StateModule : Module
    {
        private State _state;

        public State Previous { get; private set; }
        public State State
        {
            get { return _state; }
            set
            {
                if (value != _state)
                {
                    Previous = _state;
                    _state = value;
                }
            }
        }

        public void Pause()
        {
            if (IsRunning)
            {
                State = State.Pause;
            }
        }

        public void Resume()
        {
            if (IsPause)
            {
                State = State.Running;
            }
        }

        public bool IsRunning { get { return _state == State.Running; } }

        public bool IsPause { get { return _state == State.Pause; } }

        public override void Initialize()
        {
            
        }
    }

    public enum State
    {
        Unknown,
        Pause,
        Start,
        Running,
        Stop,
        Dead
    }
}
