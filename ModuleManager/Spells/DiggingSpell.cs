using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModuleManager.Spells;

namespace ModuleManager
{
    public class DiggingSpell : Spell
    {
        private readonly DiggingModifier _modifier = new DiggingModifier(1.5f);

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
