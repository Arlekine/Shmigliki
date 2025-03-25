using System;
using System.Collections.Generic;
using com.cyborgAssets.inspectorButtonPro;
using DG.Tweening;
using EventBusSystem;
using TMPro;
using UnityEngine;
using Zenject;

namespace Shmigliki.Gameplay.UI
{
    public class ComboView : MonoBehaviour, IComboHandler
    {
        [SerializeField] private RectTransform _comboLabelsParent;

        private ComboViewConfig _config;
        private MonsterEvolveScoringConfig _evolveScoringConfig;
        private Camera _camera;

        private Dictionary<TMP_Text, Sequence> _sequences = new Dictionary<TMP_Text, Sequence>();

        [Inject]
        private void Initialize(Camera camera, ComboViewConfig config, MonsterEvolveScoringConfig evolveScoringConfig)
        {
            _camera = camera;
            _config = config;
            _evolveScoringConfig = evolveScoringConfig;
        }

        [ProButton]
        public void OnCombo(Vector3 position, int comboCount)
        {
            var newLabel = Instantiate(_config.ComboTextPrefab, _comboLabelsParent);
            var newLabelRectTransform = newLabel.GetComponent<RectTransform>();
            newLabelRectTransform.anchorMin = Vector3.zero;
            newLabelRectTransform.anchorMax = Vector3.zero;
            
            newLabelRectTransform.anchoredPosition = _camera.WorldToScreenPoint(position);
            var comboData = _config.GetDataForComboCount(comboCount);

            var targetColor = comboData.Color;
            targetColor.a = 0f;

            newLabel.text = String.Format(comboData.Format, _evolveScoringConfig.GetComboMultiplier(comboCount).ToString("#.#"));
            newLabel.color = targetColor;
            newLabel.fontSize = comboData.FontSize;

            var mainSequence = DOTween.Sequence();
            var fadeSequence = DOTween.Sequence();

            fadeSequence.Append(newLabel.DOFade(1f, _config.FadeTime));
            fadeSequence.AppendInterval(_config.PauseBetweenFadeOut);
            fadeSequence.Append(newLabel.DOFade(0f, _config.FadeTime));

            mainSequence.Join(newLabelRectTransform.DOAnchorPosY(_config.HeightOffset, _config.Time).SetRelative().SetEase(_config.Curve));
            mainSequence.Join(fadeSequence);

            _sequences.Add(newLabel, mainSequence);

            mainSequence.onComplete += () =>
            {
                _sequences.Remove(newLabel);
                Destroy(newLabel.gameObject);
            };
        }

        public void Dispose()
        { }

        private void OnDestroy()
        {
            foreach (var key in _sequences.Keys)
                _sequences[key]?.Kill();
        }

        private void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }
    }
}