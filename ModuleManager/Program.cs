using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ModuleManager.Spells;

namespace ModuleManager
{
    class Program
    {
        private static readonly object Sync = new object();

        static void Main(string[] args)
        {
            var module = new ModuleManager();

            module.AddModule<TickModule>();
            module.AddModule<InputModule>();
            module.AddModule<PlayerModule>()
                .SetPlayer<Player>()
                .SetValue(CharacterProperty.Speed, 1f)
                .SetValue(CharacterProperty.Life, 1)
                .SetValue(CharacterProperty.DiggingSpeed, 1f);

            module.AddModule<CellModule>();
            module.AddModule<EventDispatcherModule>().EVENT += OnEvent;
            module.AddModule<GuardModule>();
            module.AddModule<StateModule>();
            module.AddModule<CoinModule>();
            module.AddModule<SpellModule>()
                .Add<SpeedUpSpell>()
                .Add<SpeedDownSpell>()
                .Add<DiggingSpell>();
            

            module.Initialize();

            ThreadPool.QueueUserWorkItem((s) =>
            {
                while (true)
                {
                    lock (Sync)
                    {
                        module.GetModule<TickModule>().Tick();
                    }
                    Thread.Sleep(100);
                }
            });

            while (true)
            {
                var key = Console.ReadKey(true);
                lock (Sync)
                {
                    switch (key.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            module.GetModule<InputModule>().Input(new Input { Type = InputType.MoveLeft });
                            break;
                        case ConsoleKey.RightArrow:
                            module.GetModule<InputModule>().Input(new Input { Type = InputType.MoveRight });
                            break;
                        case ConsoleKey.UpArrow:
                            module.GetModule<InputModule>().Input(new Input { Type = InputType.MoveUp });
                            break;
                        case ConsoleKey.DownArrow:
                            module.GetModule<InputModule>().Input(new Input { Type = InputType.MoveDown });
                            break;
                        case ConsoleKey.Z:
                            module.GetModule<InputModule>().Input(new Input { Type = InputType.DigLeft });
                            break;
                        case ConsoleKey.X:
                            module.GetModule<InputModule>().Input(new Input { Type = InputType.DigRight });
                            break;
                        case ConsoleKey.Escape:
                            var state = module.GetModule<StateModule>();
                            if (state.IsPause) state.Resume();
                            else state.Pause();
                            break;
                    }
                }
            }
        }

        private static void OnEvent(Event evt)
        {

        }
    }
}
