using UnityEngine;

namespace Shmigliki.Gameplay
{
    public class MonsterView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _main;
        [SerializeField] private SpriteRenderer _outline;

        [Space] 
        [SerializeField] private Color _defalutOulineColor = Color.white;
        [SerializeField] private Color _higherThanRampColor = Color.white;

        public void SetHigherThanRamp(bool isHigher)
        {
            _outline.color = isHigher ? _higherThanRampColor : _defalutOulineColor;
        }
    }
}