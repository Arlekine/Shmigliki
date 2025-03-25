using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shmigliki.Gameplay
{
    [CreateAssetMenu(menuName = "Configs/MonsterEvolveScoringConfig", fileName = "MonsterEvolveScoringConfig")]
    public class MonsterEvolveScoringConfig : ScriptableObject
    {
        [SerializeField] private float _maxTimeBetweenCombo;
        [SerializeField] private List<int> _scoreForLevel = new List<int>();
        [SerializeField] private List<float> _multiplierForCombo = new List<float>();

        public float MaxTimeBetweenCombo => _maxTimeBetweenCombo;

        public int GetScoreForLevel(int level) => _scoreForLevel[level];

        public float GetComboMultiplier(int combo)
        {
            combo = Mathf.Min(combo, _multiplierForCombo.Count);
            return _multiplierForCombo[combo - 1];
        }
    }
}