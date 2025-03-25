using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Shmigliki.Gameplay
{
    public class MonsterSpawner : ITickable, IDisposable
    {
        private ISpawnInput _spawnInput;
        private float _spawnHeight;

        private bool _isActive;
        
        public MonsterSpawner(GameZone gameZone, ISpawnInput spawnInput)
        {
            _spawnHeight = gameZone.SpawnHeight;
            _spawnInput = spawnInput;
            SpawnPosition = new Vector3(0f, _spawnHeight, 0f);
        }

        public event Action<Vector3> Spawned; 

        public Vector3 SpawnPosition { get; private set; }

        public void Activate()
        {
            _spawnInput.Spawn += OnSpawn;
            _isActive = true;
        }

        public void Deactivate()
        {
            _spawnInput.Spawn -= OnSpawn;
            _isActive = false;
        }

        public void Dispose()
        {
            _spawnInput.Spawn -= OnSpawn;
        }

        public void Tick()
        {
            if (_isActive)
                SpawnPosition = new Vector3(_spawnInput.SpawnPosition, _spawnHeight, 0f);
        }

        private void OnSpawn()
        {
            Spawned?.Invoke(SpawnPosition);
        }
    }
}