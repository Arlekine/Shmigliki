using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Shmigliki.Gameplay.UI
{
    public class ScoreView : MonoBehaviour
    {
        private const string MultiplierFormat = "x{0}";

        [SerializeField] private TMP_Text _currentScore;
        [SerializeField] private TMP_Text _maxScore;
        [SerializeField] private TMP_Text _multiplier;

        private Score _score;

        [Inject]
        public void Initialize(Score score)
        {
            _score = score;

            _currentScore.text = _score.CurrentScore.ToString();
            _multiplier.text = String.Format(MultiplierFormat, _score.CurrentMultiplier);
            _maxScore.text = _score.MaxScore.ToString();

            _score.CurrentScoreUpdated += OnCurrentScoreUpdate;
            _score.CurrentMultiplierUpdated += OnMultiplierUpdated;
            _score.MaxScoreUpdated += OnMaxScoreUpdate;
        }

        public void OnMultiplierUpdated(float multiplier)
        {
            _multiplier.text = multiplier.ToString();
        }

        public void OnCurrentScoreUpdate(int newCurrentScore)
        {
            _currentScore.text = newCurrentScore.ToString();
        }

        public void OnMaxScoreUpdate(int newMaxScore)
        {
            _maxScore.text = newMaxScore.ToString();
        }

        private void OnDestroy()
        {
            _score.CurrentScoreUpdated -= OnCurrentScoreUpdate;
            _score.CurrentMultiplierUpdated -= OnMultiplierUpdated;
            _score.MaxScoreUpdated -= OnMaxScoreUpdate;
        }
    }
}