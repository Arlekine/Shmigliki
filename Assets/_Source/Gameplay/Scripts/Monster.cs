using System;
using System.Collections.Generic;
using System.Linq;
using com.cyborgAssets.inspectorButtonPro;
using DG.Tweening;
using UnityEngine;
using Zenject;
using static UnityEngine.Rendering.DebugUI;

namespace Shmigliki.Gameplay
{
    [SelectionBase]
    public class Monster : MonoBehaviour
    {
        [SerializeField] private MonsterConfig _monsterConfig;
        [SerializeField] private List<CircleCollider2D> _colliders;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private MonsterView _monsterView;
        
        [Space]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _spriteRendererSizeMultiplier;

        private float _zoneSize = 0;
        private int _sizeLevelOffset = 0;
        private int _currentLevel = 0;
        private int _currentAge = 0;
        private int _activityBlockers = 0;
        private float _activationTime;

        private List<MonsterTag> _tags = new List<MonsterTag>();

        public event Action<Monster> Deleted;

        public bool IsActive => _activityBlockers == 0;

        public int ActivityBlockers
        {
            get => _activityBlockers;
            set => _activityBlockers = Mathf.Max(0, value);
        }

        public bool IsNewCreated => Time.time < _activationTime + _monsterConfig.NewCreatedTime;
        public int SizeLevelOffset => _sizeLevelOffset;
        public float Size => _monsterConfig.GetData(Level - SizeLevelOffset).SizeModificator * _zoneSize;
        public int Level => _currentLevel;
        public int Age => _currentAge;
        public IReadOnlyList<CircleCollider2D> Colliders => _colliders;
        public MonsterView MonsterView => _monsterView;
        public IReadOnlyList<MonsterTag> Tags => _tags;
        
        public void Initialize(float zoneSize, int startLevel)
        {
            _zoneSize = zoneSize;
            SetLevel(startLevel, 0f);
        }

        public void AddTag<T>(T tag) where T : MonsterTag
        {
            if (HasTag<T>())
                throw new ArgumentException($"{typeof(T)} already exist on this {nameof(Monster)}");

            tag.OnAdded(this);
            _tags.Add(tag);
        }

        public void RemoveTag<T>() where T : MonsterTag
        {
            if (HasTag<T>())
            {
                var tag = _tags.Find(x => x is T);
                _tags.Remove(tag);

                tag.OnRemoved(this);
            }
        }

        public bool HasTag<T>() where T : MonsterTag
        {
            return _tags.Find(x => x is T) != null;
        }

        [ProButton]
        private void ShowTags()
        {
            _tags.ForEach(x =>
            {
                if (x is Business xx)
                    print(xx.AdditionalPoints);
                else print("Other");
            });
        }

        public void Activate()
        {
            GetComponentsInChildren<Rigidbody2D>().ToList().ForEach(x => x.bodyType = RigidbodyType2D.Dynamic);
            _colliders.ForEach(x => x.enabled = true);
            ActivityBlockers--;

            _activationTime = Time.time;
        }

        public void Deactivate()
        {
            GetComponentsInChildren<Rigidbody2D>().ToList().ForEach(x => x.bodyType = RigidbodyType2D.Kinematic);
            _colliders.ForEach(x => x.enabled = false);
            ActivityBlockers++;
        }

        [ProButton]
        public void Grow(float time)
        {
            SetLevel(_currentLevel + 1, time);
        }

        [ProButton]
        public void Delete(float offset = 0f)
        {
            Deactivate();
            Deleted?.Invoke(this);
            gameObject.SetActive(false);

            Destroy(gameObject, offset);
        }

        public void IncreaseAge() => _currentAge++;

        public void SetSizeLevelOffset(int offset, float sizeChangeTime = 0f)
        {
            if (offset < 0)
                throw new ArgumentException($"{nameof(Monster)} {nameof(offset)} cant be less than zero");

            _sizeLevelOffset = offset;
            Mathf.Min(offset, _currentLevel - 1);
            SetSize(_currentLevel, sizeChangeTime);
        }

        private void SetLevel(int level, float sizeChangeTime = 0f)
        {
            _currentLevel = level;

            var data = _monsterConfig.GetData(level);
            _spriteRenderer.color = data.Color;

            SetSize(level, sizeChangeTime);
        }

        private void SetSize(int level, float sizeChangeTime = 0f)
        {
            var sizeLevel = Mathf.Max(0, level - _sizeLevelOffset);
            var size = _monsterConfig.GetData(sizeLevel).SizeModificator * _zoneSize;
            //_colliders.radius = size;
            _spriteRenderer.transform.DOScale(size * _spriteRendererSizeMultiplier, sizeChangeTime).SetEase(Ease.OutBack);

        }
    }
}
