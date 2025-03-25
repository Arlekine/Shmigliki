using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Shmigliki.Gameplay
{
    public class SpawningController : ILateTickable, IDisposable, IMonsterHolder
    {
        private MonsterSpawner _monsterSpawner;
        private INextSpawnableHolder _nextSpawnableHolder;
        private ISpawnInput _spawnInput;
        private GameZone _gameZone;
        private float _minPauseBetweenSpawns;

        private Monster _nextMonster;
        private Monster _currentMonster;

        private List<Monster> _currentMonsters = new List<Monster>();

        public SpawningController(MonsterSpawner monsterSpawner, INextSpawnableHolder nextSpawnableHolder, ISpawnInput spawnInput, GameZone gameZone, float minPauseBetweenSpawns)
        {
            _monsterSpawner = monsterSpawner;
            _nextSpawnableHolder = nextSpawnableHolder;
            _minPauseBetweenSpawns = minPauseBetweenSpawns;
            _gameZone = gameZone;
            _spawnInput = spawnInput;

            _currentMonsters = new List<Monster>();

            _monsterSpawner.Spawned += OnSpawned;
        }

        public Monster NextMonster => _nextMonster;
        public Monster CurrentMonster => _currentMonster;
        public IReadOnlyList<Monster> CurrentMonsters => _currentMonsters;

        public void StartSpawning(Monster currentToSpawn, Monster nextToSpawn)
        {
            if (currentToSpawn == null)
            {
                CreateNextMonster();
                UpdateCurrentMonster();
            }
            else
            {
                _currentMonster = currentToSpawn;
                _currentMonster.Deactivate();

                UpdateSpawnZoneByCurrentMonster();
            }

            if (nextToSpawn == null)
            {
                CreateNextMonster();
            }
            else
            {
                _nextMonster = nextToSpawn;
                _nextMonster.transform.position = GetNextMonsterPosition();
                _nextMonster.Deactivate();
            }

            _monsterSpawner.Activate();
        }

        public void SetInitialMonsters(IEnumerable<Monster> monsters)
        {
            _currentMonsters.AddRange(monsters);
            _currentMonsters.ForEach(x => x.Deleted += OnDeleted);
        }

        public void Dispose()
        {
            _currentMonsters.ForEach(x => x.Deleted -= OnDeleted);
        }

        private void OnDeleted(Monster monster)
        {
            monster.Deleted -= OnDeleted;
            _currentMonsters.Remove(monster);
        }

        public void LateTick()
        {
            _currentMonster.transform.position = _monsterSpawner.SpawnPosition;
        }

        private void UpdateCurrentMonster()
        {
            _currentMonster = _nextMonster;
            UpdateSpawnZoneByCurrentMonster();
        }
        
        private async void OnSpawned(Vector3 spawnPosition)
        {
            _monsterSpawner.Deactivate();

            _currentMonster.Activate();
            _currentMonsters.ForEach(x => x.IncreaseAge());
            _currentMonsters.Add(_currentMonster);
            _currentMonster.Deleted += OnDeleted;

            await UniTask.Delay(TimeSpan.FromSeconds(_minPauseBetweenSpawns));
            //TODO: animate current to next transition -> wait till end -> create new next -> animate -> _monsterSpawner.Activate();

            UpdateCurrentMonster();
            _monsterSpawner.Activate();

            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));

            CreateNextMonster();
        }

        private void CreateNextMonster()
        {
            _nextMonster = _nextSpawnableHolder.GetNextMonster(GetNextMonsterPosition());
            _nextMonster.Deactivate();
        }

        private Vector3 GetNextMonsterPosition() => _monsterSpawner.SpawnPosition + Vector3.up * 2f;

        private void UpdateSpawnZoneByCurrentMonster() =>
            _spawnInput.UpdateSpawnZone(new SpawnZone(_gameZone.LeftBorder + _currentMonster.Size, _gameZone.RightBorder - _currentMonster.Size));
    }
}