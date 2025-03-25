using UnityEngine;

namespace Shmigliki.Gameplay
{
    public class DefaultNextMonsterHolder : INextSpawnableHolder
    {
        private const int MAX_SPAWN_LEVEL = 3;

        private IMonsterFactory _monsterFactory;

        public DefaultNextMonsterHolder(IMonsterFactory monsterFactory)
        {
            _monsterFactory = monsterFactory;
        }

        public Monster GetNextMonster(Vector3 position)
        {
            var startMonsterLevel = Random.Range(0, MAX_SPAWN_LEVEL + 1);
            var newMonster = _monsterFactory.Create(startMonsterLevel, position);
            return newMonster;
        }
    }
}