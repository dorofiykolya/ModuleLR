using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager
{
    public class Modifier
    {
        public Modifier(CharacterProperty property)
        {
            Property = property;
        }

        public CharacterProperty Property { get; private set; }

        public virtual float GetValue(float value)
        {
            return value;
        } 
    }
}
