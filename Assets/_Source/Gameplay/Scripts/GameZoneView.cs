using UnityEngine;
using Zenject;

namespace Shmigliki.Gameplay
{
    public class GameZoneView : MonoBehaviour
    {
        [SerializeField] private Transform _background;
        [SerializeField] private Transform _leftBorder;
        [SerializeField] private Transform _rightBorder;
        [SerializeField] private Transform _downBorder;
        [SerializeField] private Transform _upBorder;

        [Inject]
        public void Initialize(GameZone gameZone)
        {
            _leftBorder.position = new Vector3(gameZone.LeftBorder, gameZone.CenterY);
            _rightBorder.position = new Vector3(gameZone.RightBorder, gameZone.CenterY);
            _upBorder.position = new Vector3(0f, gameZone.UpBorder);
            _downBorder.position = new Vector3(0f, gameZone.DownBorder);

            _leftBorder.localScale = new Vector3(_leftBorder.localScale.x, gameZone.Height);
            _rightBorder.localScale = new Vector3(_rightBorder.localScale.x, gameZone.Height);
            _upBorder.localScale = new Vector3(gameZone.Width, _upBorder.localScale.y, 1f);
            _downBorder.localScale = new Vector3(gameZone.Width, _upBorder.localScale.y, 1f);

            _background.localScale = new Vector3(gameZone.Width, gameZone.Height, 1f);
            _background.position = gameZone.Center;
        }
    }
}