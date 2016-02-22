using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager
{
    class InputModule : Module
    {
        private PlayerModule _playerModule;

        public override void Initialize()
        {
            _playerModule = GetModule<PlayerModule>();
        }

        public void Input(Input input)
        {
            
        }
    }

    enum InputType
    {
        MoveLeft,
        MoveRight,
        MoveUp,
        MoveDown,
        DigLeft,
        DigRight
    }

    class Input
    {
        public InputType Type;
    }
}
