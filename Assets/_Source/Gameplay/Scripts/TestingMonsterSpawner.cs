using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Shmigliki.Gameplay
{
    public class TestingMonsterSpawner : ITickable
    {
        private Camera _camera;
        private IMonsterFactory _monsterFactory;
        private IMonsterHolder _monsterHolder;

        private static List<KeyCode> _spawnKeys = new List<KeyCode>()
        {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9,
            KeyCode.Alpha0,
        };

        public TestingMonsterSpawner(Camera camera, IMonsterFactory monsterFactory)
        {
            _camera = camera;
            _monsterFactory = monsterFactory;
        }

        public void Tick()
        {
            for (int i = 0; i < _spawnKeys.Count; i++)
            {
                if (Input.GetKeyDown(_spawnKeys[i]))
                {
                    var position = _camera.ScreenToWorldPoint(Input.mousePosition);
                    position.z = 0f;
                    var monster = _monsterFactory.Create(i, position);

                    monster.Deleted += OnMonsterDeleted;

                    foreach (var currentMonster in _monsterHolder.CurrentMonsters)
                    {
                        currentMonster.IncreaseAge();
                    }
                }
            }
        }

        private void OnMonsterDeleted(Monster monster)
        {
            monster.Deleted -= OnMonsterDeleted;
        }
    }
}