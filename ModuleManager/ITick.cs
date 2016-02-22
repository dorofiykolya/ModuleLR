using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager
{
    public interface ITick
    {
        void PreTick(float time);
        void Tick(float time);
        void PostTick(float time);
        void FinalTick(float time);
    }
}
