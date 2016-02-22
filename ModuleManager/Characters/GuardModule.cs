using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager
{
    public class GuardModule : CharacterModule
    {
        public override void AddModifier(Modifier modifier)
        {
            foreach (var module in GetModules<Guard>())
            {
                module.AddModifier(modifier);
            }
        }

        public override void RemoveModifier(Modifier modifier)
        {
            foreach (var module in GetModules<Guard>())
            {
                module.RemoveModifier(modifier);
            }
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
