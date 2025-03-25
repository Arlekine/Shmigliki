using System;
using System.Collections.Generic;
using System.Linq;
using EventBusSystem;
using UnityEngine;

namespace Shmigliki.Gameplay
{
    public class MonsterEvolveScoringSystem : IMonsterEvolvingHandler
    {
        private class Combo
        {
            private float _maxTimeBetweenCombo;

            private int _currentCombo;
            private float _comboExpireTime;
            private Monster _prevEvolvedMonster;

            public Combo(float maxTimeBetweenCombo, Monster comboStartMonster)
            {
                _maxTimeBetweenCombo = maxTimeBetweenCombo;

                _prevEvolvedMonster = comboStartMonster;
                _currentCombo = 1;
                _comboExpireTime = Time.time + _maxTimeBetweenCombo;
            }

            public bool IsComboExpired => Time.time > _comboExpireTime;

            public bool IsPartOfCombo(Monster evolvedMonster, Monster eatenMonster) 
                => evolvedMonster == _prevEvolvedMonster || eatenMonster == _prevEvolvedMonster;

            public int UpdateCombo(Monster evolvedMonster)
            {
                _currentCombo++;
                _comboExpireTime = Time.time + _maxTimeBetweenCombo;
                _prevEvolvedMonster = evolvedMonster;
                return _currentCombo;
            }
            
        }

        private MonsterEvolveScoringConfig _config;
        private Score _score;
        
        private List<Combo> _activeCombos = new List<Combo>();

        public MonsterEvolveScoringSystem(MonsterEvolveScoringConfig config, Score score)
        {
            _config = config;
            _score = score;

            EventBus.Subscribe(this);
        }

        public void Dispose()
        {
            EventBus.Unsubscribe(this);
        }

        public void OnMonsterEvolved(Monster evolvedMonster, Monster eatenMonster)
        {
            _activeCombos.RemoveAll(x => x.IsComboExpired);

            var combo = _activeCombos.FirstOrDefault(x => x.IsPartOfCombo(evolvedMonster, eatenMonster));
            var comboCounter = 1;

            if (combo != null)
                comboCounter = combo.UpdateCombo(evolvedMonster);
            else
                _activeCombos.Add(new Combo(_config.MaxTimeBetweenCombo, evolvedMonster));
            
            var points = _config.GetScoreForLevel(evolvedMonster.Level);
            points = (int)(points * _config.GetComboMultiplier(comboCounter));

            _score.AddPoints(points);

            if (comboCounter > 1)
                EventBus.RaiseEvent<IComboHandler>(x => x.OnCombo(evolvedMonster.transform.position, comboCounter));
        }
    }
}