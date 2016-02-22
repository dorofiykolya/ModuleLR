using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager
{
    public abstract class Module
    {
        public abstract void Initialize();

        public ModuleManager ModuleManager { get; protected internal set; }

        public virtual T GetModule<T>() where T : Module
        {
            return ModuleManager.GetModule<T>();
        }

        public virtual void GetModules<T>(List<T> result = null) where T : class 
        {
            ModuleManager.GetModules<T>(result);
        }

        public virtual T[] GetModules<T>() where T : class
        {
            return ModuleManager.GetModules<T>();
        }
    }
}
