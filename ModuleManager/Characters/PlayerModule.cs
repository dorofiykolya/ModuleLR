using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager
{
    public class PlayerModule : CharacterModule
    {
        private Player _player;
        private InputModule _inputModule;

        public bool IsDigging
        {
            get { return GetModule<Player>().IsDigging; }
        }

        public bool GodMode
        {
            get { return GetModule<Player>().GodMode; }
            set { GetModule<Player>().GodMode = value; }
        }

        public Player SetPlayer<T>() where T : Player
        {
            _player = AddModule<T>();
            return _player;
        }

        public override void AddModifier(Modifier modifier)
        {
            _player.AddModifier(modifier);
        }

        public override void RemoveModifier(Modifier modifier)
        {
            _player.RemoveModifier(modifier);
        }

        public override void Initialize()
        {
            base.Initialize();
            _inputModule = GetModule<InputModule>();
        }

        public override void PreTick(float time)
        {

        }

        public override void Tick(float time)
        {
            _player.Move();
            Input input;
            while ((input = _inputModule.Dequeue(InputType.Skill)) != null)
            {
                _player.NextInput(input);
            }

            while ((input = _inputModule.Dequeue(InputType.Move)) != null)
            {
                _player.NextInput(input);
            }
        }

        public override void PostTick(float time)
        {

        }

        public override void FinalTick(float time)
        {

        }
    }
}
