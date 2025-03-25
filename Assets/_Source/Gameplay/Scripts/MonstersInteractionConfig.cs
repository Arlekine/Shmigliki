using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using EventBusSystem;
using UnityEngine;

namespace Shmigliki.Gameplay
{
    [CreateAssetMenu(menuName = "Configs/MonstersInteractionConfig", fileName = "MonstersInteractionConfig")]
    public class MonstersInteractionConfig : ScriptableObject
    {
        [Serializable]
        private class DefaultConsumeData
        {
            [SerializeField] private float _moveTime;
            [SerializeField] private Ease _moveEase;
            [SerializeField] private float _scaleTime;
            [SerializeField] private float _heightOffset;
            [SerializeField] private float _heightOffsetTime;
            [SerializeField] private Ease _heigthOffsetEase;

            public float MoveTime => _moveTime;
            public Ease MoveEase => _moveEase;
            public float ScaleTime => _scaleTime;
            public float HeightOffset => _heightOffset;
            public float HeightOffsetTime => _heightOffsetTime;
            public Ease HeigthOffsetEase => _heigthOffsetEase;
        }

        [SerializeField] private DefaultConsumeData _defaultConsumeData;

        public bool IsMonsterAPriorToMonsterB(Monster monsterA, Monster monsterB)
        {
            if (monsterA.HasTag<Special>() && monsterB.HasTag<Special>() == false)
                return true;
            else if (monsterA.HasTag<Special>() == false && monsterB.HasTag<Special>())
                return false;
            else if (monsterA.Age >= monsterB.Age)
                return true;
            else
                return false;
        }

        public async UniTaskVoid DefaultMonstersConsume(Monster monsterA, Monster monsterB)
        {
            var olderMonster = IsMonsterAPriorToMonsterB(monsterA, monsterB) ? monsterA : monsterB;
            var youngerMonster = IsMonsterAPriorToMonsterB(monsterA, monsterB) ? monsterB : monsterA;

            olderMonster.AddTag(new Busy());
            youngerMonster.AddTag(new Busy());

            foreach (var monsterACollider in monsterA.Colliders)
                foreach (var monsterBCollider in monsterB.Colliders)
                    Physics2D.IgnoreCollision(monsterACollider, monsterBCollider);

            var evolvingPoint = (olderMonster.transform.position + youngerMonster.transform.position) * 0.5f;

            await UniTask.WhenAll(olderMonster.transform.DOMove(evolvingPoint, _defaultConsumeData.MoveTime).SetEase(_defaultConsumeData.MoveEase).ToUniTask(), 
                youngerMonster.transform.DOMove(evolvingPoint, 0.2f).SetEase(_defaultConsumeData.MoveEase).ToUniTask());

            olderMonster.Grow(_defaultConsumeData.ScaleTime);
            youngerMonster.Delete(_defaultConsumeData.ScaleTime + 0.05f);

            await UniTask.Delay(TimeSpan.FromSeconds(_defaultConsumeData.ScaleTime));

            EventBus.RaiseEvent<IMonsterEvolvingHandler>(x => x.OnMonsterEvolved(olderMonster, youngerMonster));
            olderMonster.RemoveTag<Busy>();
        }
    }
}