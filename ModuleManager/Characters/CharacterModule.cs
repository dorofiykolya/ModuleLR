using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager
{
    public abstract class CharacterModule : ModuleManager, ITick
    {
        public abstract void PreTick(float time);

        public abstract void Tick(float time);

        public abstract void PostTick(float time);

        public abstract void FinalTick(float time);

        public abstract void AddModifier(Modifier modifier);

        public abstract void RemoveModifier(Modifier modifier);
    }
}
