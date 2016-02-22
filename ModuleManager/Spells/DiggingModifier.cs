using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager.Spells
{
    public class DiggingModifier : Modifier
    {
        private readonly float _percent;

        public DiggingModifier(float percent) : base(CharacterProperty.DiggingSpeed)
        {
            _percent = percent;
        }

        public override float GetValue(float value)
        {
            return value*_percent;
        }
    }
}
