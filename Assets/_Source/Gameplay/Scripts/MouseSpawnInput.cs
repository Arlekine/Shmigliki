using System;
using UnityEngine;
using Zenject;

namespace Shmigliki.Gameplay
{
    public class MouseSpawnInput : ISpawnInput, ITickable
    {
        private const int MOUSE_BUTTON_INDEX = 0;

        private Camera _mainCamera;
        private SpawnZone _spawnZone;

        public MouseSpawnInput(Camera mainCamera)
        {
            _mainCamera = mainCamera;
        }

        public void UpdateSpawnZone(SpawnZone spawnZone) => _spawnZone = spawnZone;

        public event Action Spawn;
        public float SpawnPosition { get; private set; }
        
        public void Tick()
        {
            SpawnPosition = Mathf.Clamp(_mainCamera.ScreenToWorldPoint(Input.mousePosition).x, _spawnZone.MinSpawnX, _spawnZone.MaxSpawnX);

            if (Input.GetMouseButtonUp(MOUSE_BUTTON_INDEX))
                Spawn?.Invoke();
        }
    }
}