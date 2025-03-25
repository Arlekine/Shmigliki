using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using EventBusSystem;
using UnityEngine;

namespace Shmigliki.Gameplay
{
    public class ConsumeMonsterOfSameSizeRule : IMonsterCollisionHandler
    {
        private MonstersInteractionConfig _monsterInteractionConfig;
        private MonsterConfig _monsterConfig;

        public ConsumeMonsterOfSameSizeRule(MonstersInteractionConfig monsterInteractionConfig, MonsterConfig config)
        {
            EventBus.Subscribe(this);
            _monsterInteractionConfig = monsterInteractionConfig;
            _monsterConfig = config;
        }

        public void OnMonsterCollided(Monster monsterA, Monster monsterB)
        {
            if (monsterA.Level == monsterB.Level && monsterA.Level != _monsterConfig.MaxLevel)
            {
                _monsterInteractionConfig.DefaultMonstersConsume(monsterA, monsterB).Forget();
            }
        }

        public void Dispose()
        {
            EventBus.Unsubscribe(this);
        }
    }
}