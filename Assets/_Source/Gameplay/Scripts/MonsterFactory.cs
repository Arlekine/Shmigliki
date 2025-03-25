using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shmigliki.Gameplay
{
    public class MonsterFactory : IMonsterFactory
    {
        private Transform _monsterParent;
        private Monster _monsterPrefab;
        private GameZone _gameZone;

        public MonsterFactory(Transform monsterParent, Monster monsterPrefab, GameZone gameZone)
        {
            _monsterParent = monsterParent;
            _monsterPrefab = monsterPrefab;
            _gameZone = gameZone;
        }

        public Monster Create(int initialLevel, Vector3 position)
        {
            var newMonster = UnityEngine.Object.Instantiate(_monsterPrefab, _monsterParent);
            newMonster.transform.position = position;

            newMonster.Initialize(_gameZone.Width, initialLevel);
            return newMonster;
        }
    }
}