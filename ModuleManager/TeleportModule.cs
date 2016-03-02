using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager
{
    public class TeleportModule : Module
    {
        private TeleportRecord[] _teleports;

        public override void Initialize()
        {

        }

        public TeleportRecord Get(int x, int y)
        {
            var point = new Point { X = x, Y = y };
            var result = _teleports.FirstOrDefault(t => t.From == point);
            if (result == null)
            {
                result = _teleports.FirstOrDefault(t => t.To == point);
            }
            return result;
        }

        public void Set(TeleportRecord[] teleports)
        {
            _teleports = teleports;
        }
    }
}
