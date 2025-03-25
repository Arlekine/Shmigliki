using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Shmigliki.Gameplay
{
    [Serializable]
    public class Score
    {
        [JsonProperty] private int _currentScore;
        [JsonProperty] private float _currentMultiplier;
        [JsonProperty] private int _maxScore;
        
        public Score(int currentScore, float currentMultiplier, int maxScore)
        {
            if (currentMultiplier < 1)
                throw new ArgumentException($"{nameof(currentMultiplier)} cant be less than 1");

            if (currentScore < 0)
                throw new ArgumentException($"{nameof(currentScore)} cant be less than 0");

            if (maxScore < currentScore)
                throw new ArgumentException($"{nameof(maxScore)} cant be less than {nameof(currentScore)}");

            _currentScore = currentScore;
            _currentMultiplier = currentMultiplier;
            _maxScore = maxScore;
        }

        public event Action<int> MaxScoreUpdated;
        public event Action<int> CurrentScoreUpdated;
        public event Action<float> CurrentMultiplierUpdated;
        
        public int CurrentScore => _currentScore;
        public int MaxScore => _maxScore;

        public float CurrentMultiplier
        {
            get => _currentMultiplier;
            set
            {
                if (_currentMultiplier < 1)
                    throw new ArgumentException($"{nameof(_currentMultiplier)} cant be less than 1");

                _currentMultiplier = 1;
            }
        }

        public void Reset()
        {
            _currentScore = 0;
            _currentMultiplier = 1;
        }

        public void AddPoints(int points)
        {
            if (points <= 0)
                throw new ArgumentException($"{nameof(points)} should be positive");

            _currentScore += Mathf.RoundToInt(points * _currentMultiplier);
            CurrentScoreUpdated?.Invoke(_currentScore);

            if (_currentScore > _maxScore)
            {
                _maxScore = _currentScore;
                MaxScoreUpdated?.Invoke(_maxScore);
            }
        }
    }
}