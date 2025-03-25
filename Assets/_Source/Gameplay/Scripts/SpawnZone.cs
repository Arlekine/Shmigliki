namespace Shmigliki.Gameplay
{
    public struct SpawnZone
    {
        private float _minSpawnX;
        private float _maxSpawnX;

        public SpawnZone(float minSpawnX, float maxSpawnX)
        {
            _minSpawnX = minSpawnX;
            _maxSpawnX = maxSpawnX;
        }

        public float MinSpawnX => _minSpawnX;
        public float MaxSpawnX => _maxSpawnX;
    }
}