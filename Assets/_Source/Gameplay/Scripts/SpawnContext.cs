using UnityEngine;

namespace Shmigliki.Gameplay
{
    public class SpawnContext
    {
        [SerializeField] private float _spawnHeight;
        [SerializeField] private Transform _currentParent;
        [SerializeField] private Transform _nextParent;

        public float SpawnHeight => _spawnHeight;
        public Transform CurrentParent => _currentParent;
        public Transform NextParent => _nextParent;
    }
}