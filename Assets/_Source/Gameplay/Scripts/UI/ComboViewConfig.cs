using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Shmigliki.Gameplay.UI
{
    [CreateAssetMenu(menuName = "Configs/ComboViewConfig", fileName = "ComboViewConfig")]
    public class ComboViewConfig : ScriptableObject
    {
        [Serializable]
        public class Data
        {
            [SerializeField] private float _fontSize;
            [SerializeField] private Color _color = Color.white;
            [SerializeField] private string _format = "{0}";

            public float FontSize => _fontSize;
            public Color Color => _color;
            public string Format => _format;
        }

        [SerializeField] private TMP_Text _comboTextPrefab;
        [SerializeField] private List<Data> _comboDatas = new List<Data>();

        [Space]
        [SerializeField] private float _heightOffset;
        [SerializeField] private AnimationCurve _curve;
        [SerializeField][Min(0)] private float _time;
        [SerializeField][Min(0)] private float _fadeTime;

        public TMP_Text ComboTextPrefab => _comboTextPrefab;

        public float HeightOffset => _heightOffset;
        public AnimationCurve Curve => _curve;
        public float Time => _time;
        public float FadeTime => _fadeTime;
        public float PauseBetweenFadeOut => Time - FadeTime * 2f;

        public Data GetDataForComboCount(int comboCount)
        {
            var index = Mathf.Min(comboCount, _comboDatas.Count - 1);
            return _comboDatas[index];
        }

        private void OnValidate()
        {
            _fadeTime = Mathf.Min(_fadeTime, _time * 0.5f - 0.01f);
            for (int i = 0; i < _comboDatas.Count; i++)
            {
                if (_comboDatas[i].Format.Contains("{0}") == false)
                    Debug.LogError($"Data with index {i} format - doesn't contain '{'0'}'");
            }
        }
    }
}