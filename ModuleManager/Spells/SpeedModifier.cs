using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager
{
    class SpeedModifier : Modifier
    {
        private readonly float _percent;

        public SpeedModifier(float percent) : base(CharacterProperty.Speed)
        {
            _percent = percent;
        }

        public override float GetValue(float value)
        {
            return value * _percent;
        }
    }
}
