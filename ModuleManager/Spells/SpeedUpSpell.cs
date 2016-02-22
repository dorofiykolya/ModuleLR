using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager
{
    public class SpeedUpSpell : Spell
    {
        private readonly SpeedModifier _modifier;

        public SpeedUpSpell()
        {
            _modifier = new SpeedModifier(1.1f);
        }

        public override void Process()
        {
            GetModule<PlayerModule>().AddModifier(_modifier);
        }

        public override void PostProcess()
        {
            GetModule<PlayerModule>().RemoveModifier(_modifier);
        }
    }
}
