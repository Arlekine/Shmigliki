using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Shmigliki.Gameplay
{
    public class GameZone
    {
        private GameZoneConfig _config;

        private float _leftBorder;
        private float _rightBorder;
        private float _upBorder;
        private float _downBorder;

        public GameZone(Camera camera, GameZoneConfig config)
        {
            _config = config;

            _leftBorder = camera.ViewportToWorldPoint(new Vector3(0f, 0F, 0f)).x + config.ScreenBordersOffset;
            _rightBorder = camera.ViewportToWorldPoint(new Vector3(1f, 0F, 0f)).x - config.ScreenBordersOffset;

            var width = _rightBorder - _leftBorder;
            var height = width * config.WidthToHeightMod;

            _downBorder = -height * 0.5f + config.CenterYOffset;
            _upBorder = height * 0.5f + config.CenterYOffset;
        }

        public float LeftBorder => _leftBorder;
        public float RightBorder => _rightBorder;
        public float UpBorder => _upBorder;
        public float DownBorder => _downBorder;
        public float CenterX => (LeftBorder + RightBorder) * 0.5f;
        public float CenterY => (UpBorder + DownBorder) * 0.5f;
        public Vector3 Center => new Vector3(CenterX, CenterY);
        public float Width => _rightBorder - _leftBorder;
        public float Height => UpBorder - DownBorder;
        public float SpawnHeight => UpBorder + _config.SpawnOffset;
    }
}