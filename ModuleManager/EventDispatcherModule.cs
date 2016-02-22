using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager
{
    public class EventDispatcherModule : Module
    {
        public event Action<Event> EVENT;

        public override void Initialize()
        {

        }

        public void DispatchEvent(Event evt)
        {
            if (EVENT != null)
            {
                EVENT(evt);
            }
        }
    }

    public abstract class Event : IDisposable
    {
        private static readonly Dictionary<int, Stack<Event>> _pool = new Dictionary<int, Stack<Event>>();

        private bool _disposed;
        private readonly int _id;

        protected Event()
        {
            _id = GetType().FullName.GetHashCode();
        }

        public static T Instantiate<T>() where T : Event
        {
            var id = typeof (T).FullName.GetHashCode();
            Stack<Event> result;
            if (_pool.TryGetValue(id, out result))
            {
                if (result.Count != 0)
                {
                    var evt = (T) result.Pop();
                    evt._disposed = false;
                    return evt;
                }
            }
            return Activator.CreateInstance<T>();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                Stack<Event> stack;
                if (!_pool.TryGetValue(_id, out stack))
                {
                    _pool[_id] = stack = new Stack<Event>();
                }
                stack.Push(this);
            }
        }
    }
}
