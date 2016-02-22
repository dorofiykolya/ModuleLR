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

        public bool IsDigging
        {
            get { return GetModule<Player>().IsDigging; }
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

        public override void PreTick(float time)
        {
            
        }

        public override void Tick(float time)
        {

        }

        public override void PostTick(float time)
        {

        }

        public override void FinalTick(float time)
        {

        }
    }
}
