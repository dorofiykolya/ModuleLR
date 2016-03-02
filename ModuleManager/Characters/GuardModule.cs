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

        public void Set(GuardRecord[] guards, Point[] guardRespawn, int guardRespawnTime)
        {
            throw new NotImplementedException();
        }

        public Guard GetGuardAt(int x, int y)
        {
            foreach (var guard in GetModules<Guard>())
            {
                if (guard.X == x && guard.Y == y)
                {
                    return guard;
                }
            }
            return null;
        }

        public bool IsGuardAt(int x, int y)
        {
            return GetGuardAt(x, y) != null;
        }

        public bool IsGuardAlive(int x, int y)
        {
            return IsGuardAt(x, y);
        }
    }
}
