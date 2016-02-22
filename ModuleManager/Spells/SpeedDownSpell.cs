using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager.Spells
{
    public class SpeedDownSpell : Spell
    {
        private readonly SpeedModifier _modifier;

        public SpeedDownSpell()
        {
            _modifier = new SpeedModifier(0.5f);
        }

        public override void Process()
        {
            GetModule<GuardModule>().AddModifier(_modifier);
        }

        public override void PostProcess()
        {
            GetModule<GuardModule>().RemoveModifier(_modifier);
        }
    }
}
