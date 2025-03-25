using System.Collections.Generic;
using System.Linq;
using EventBusSystem;
using UnityEngine;
using Zenject;

namespace Shmigliki.Gameplay
{
    public class GameLose : ITickable
    {
        private IMonsterHolder _monsterHolder;
        private GameZone _gameZone;
        private float _timeToLose;

        private float _monsterHigherThanRampTime;
        private bool _lost;

        private List<Monster> _monstersHigherThanRamp = new List<Monster>();

        public GameLose(IMonsterHolder monsterHolder, GameZone gameZone, float timeToLose)
        {
            _monsterHolder = monsterHolder;
            _gameZone = gameZone;
            _timeToLose = timeToLose;
        }

        public void Tick()
        {
            if (_lost) return;

            var newHigherThanRamp = _monsterHolder.CurrentMonsters.Where(x =>
                x.transform.position.y >= _gameZone.UpBorder && x.IsNewCreated == false && _monstersHigherThanRamp.Contains(x) == false);

            foreach (var monster in newHigherThanRamp)
                monster.MonsterView.SetHigherThanRamp(true);

            var monsterLowerThanRamp = _monstersHigherThanRamp.Where(x => x != null && x.transform.position.y < _gameZone.UpBorder);

            foreach (var monster in monsterLowerThanRamp)
                monster.MonsterView.SetHigherThanRamp(false);

            _monstersHigherThanRamp.RemoveAll(x => x == null || x.transform.position.y < _gameZone.UpBorder);

            _monstersHigherThanRamp.AddRange(newHigherThanRamp);
            
            if (_monstersHigherThanRamp.Count == 0)
                _monsterHigherThanRampTime = 0;
            else
                _monsterHigherThanRampTime += Time.deltaTime;

            if (_monsterHigherThanRampTime >= _timeToLose)
            {
                _lost = true;
                Debug.Log("LOSE");
                EventBus.RaiseEvent<ILoseHandler>(x => x.OnLost());
            }
        }
    }
}