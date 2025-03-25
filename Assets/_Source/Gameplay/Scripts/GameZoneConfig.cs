using UnityEngine;

namespace Shmigliki.Gameplay
{
    [CreateAssetMenu(menuName = "Configs/GameZoneConfig", fileName = "GameZoneConfig")]
    public class GameZoneConfig : ScriptableObject
    {
        [SerializeField] private float _widthToHeightMod;
        [SerializeField] private float _screenBordersOffset;
        [SerializeField] private float _centerYOffset;
        [SerializeField] private float _spawnOffset;

        public float WidthToHeightMod => _widthToHeightMod;
        public float ScreenBordersOffset => _screenBordersOffset;
        public float CenterYOffset => _centerYOffset;
        public float SpawnOffset => _spawnOffset;
    }
}