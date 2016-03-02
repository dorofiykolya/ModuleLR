using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager
{
    public class InputModule : Module
    {
        private PlayerModule _playerModule;
        private readonly Dictionary<InputType, Queue<Input>> _dictionary = new Dictionary<InputType, Queue<Input>>(); 

        public override void Initialize()
        {
            _playerModule = GetModule<PlayerModule>();
            _dictionary[InputType.Move] = new Queue<Input>();
            _dictionary[InputType.Skill] = new Queue<Input>();
        }

        public Input Dequeue(InputType type)
        {
            return _dictionary[type].Count != 0? _dictionary[type].Dequeue() : null;
        }

        public void Input(Input input)
        {
            _dictionary[input.Type].Enqueue(input);
        }
    }

    public enum InputType
    {
        Move,
        Skill
    }

    public enum InputAction
    {
        MoveLeft,
        MoveRight,
        MoveUp,
        MoveDown,
        DigLeft,
        DigRight
    }

    public class Input
    {
        public InputType Type;
        public InputAction Action;
        public int SkillId;
    }
}
