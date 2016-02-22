using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager
{
    public class SpellModule : ModuleManager, ITick
    {
        public override void Initialize()
        {

        }

        public SpellModule Add<T>() where T : Spell
        {
            AddModule<T>();
            return this;
        }

        public void PreTick(float time)
        {
            foreach (var spell in GetModules<Spell>())
            {
                if (spell.RemainingTime > 0f && !spell.Added)
                {
                    spell.Added = true;
                    spell.Process();
                }
            }
        }

        public void Tick(float time)
        {
            foreach (var spell in GetModules<Spell>())
            {
                spell.Tick(time);
            }
        }

        public void PostTick(float time)
        {
            foreach (var spell in GetModules<Spell>())
            {
                if (spell.RemainingTime <= 0f && spell.Added)
                {
                    spell.Added = false;
                    spell.PostProcess();
                }
            }
        }

        public void FinalTick(float time)
        {

        }
    }

    public abstract class Spell : Module
    {
        public override void Initialize()
        {

        }

        public void Set(float time)
        {
            RemainingTime = time;
        }

        public bool Added { get; set; }

        public float RemainingTime { get; set; }

        public abstract void Process();

        public abstract void PostProcess();

        public virtual void Tick(float time)
        {
            RemainingTime = Math.Max(0f, RemainingTime - time);
        }
    }
}
