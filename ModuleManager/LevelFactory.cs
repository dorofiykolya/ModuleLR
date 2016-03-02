using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModuleManager.Spells;

namespace ModuleManager
{
    public class LevelFactory
    {
        public LevelModule Instantiate(GameLevelRecord record)
        {
            var module = new LevelModule();
            module.AddModule<TimeModule>();
            module.AddModule<InputModule>();
            module.AddModule<PlayerModule>()
                .SetPlayer<Player>()
                .SetValue(CharacterProperty.Speed, 1f)
                .SetValue(CharacterProperty.Life, 1)
                .SetValue(CharacterProperty.DiggingSpeed, 1f);

            module.AddModule<DiggingModule>();
            module.AddModule<TeleportModule>().Set(record.Teleports);
            module.AddModule<CellModule>().Set(record.Size, record.Cells);
            module.AddModule<GuardModule>().Set(record.Guards, record.GuardRespawn, record.GuardRespawnTime);
            module.AddModule<StateModule>();
            module.AddModule<CoinModule>().Set(record.GoldGhests);
            module.AddModule<SpellModule>()
                .Add<SpeedUpSpell>()
                .Add<SpeedDownSpell>()
                .Add<DiggingSpell>();

            
            return module;
        }
    }
}
