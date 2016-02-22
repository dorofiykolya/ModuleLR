using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager
{
    public class ModuleManager : Module
    {
        private readonly List<Module> _modules;

        public ModuleManager()
        {
            _modules = new List<Module>();
        }

        public Module AddModule(Module module)
        {
            _modules.Add(module);
            module.ModuleManager = this;
            return module;
        }

        public T AddModule<T>() where T : Module
        {
            return (T)AddModule(Activator.CreateInstance<T>());
        }

        public override T GetModule<T>()
        {
            var result = (T)_modules.FirstOrDefault(m => m is T);
            if (result == null && ModuleManager != null)
            {
                result = (T)ModuleManager.GetModule<T>();
            }
            return result;
        }

        public override void GetModules<T>(List<T> result = null)
        {
            if (result == null) result = new List<T>();
            if (ModuleManager != null)
            {
                ModuleManager.GetModules<T>(result);
            }
            result.AddRange(_modules.OfType<T>());
        }

        public override T[] GetModules<T>()
        {
            var result = new List<T>();
            GetModules<T>(result);
            return result.ToArray();
        }

        public override void Initialize()
        {
            foreach (var module in _modules)
            {
                module.Initialize();
            }
        }
    }
}
