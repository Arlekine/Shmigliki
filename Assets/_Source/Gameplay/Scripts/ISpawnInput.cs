using System;

namespace Shmigliki.Gameplay
{
    public interface ISpawnInput
    {
        event Action Spawn;

        float SpawnPosition { get; }
        public void UpdateSpawnZone(SpawnZone spawnZone);
    }
}