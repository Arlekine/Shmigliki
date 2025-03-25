using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Shmigliki.Gameplay;
using UnityEngine;
using Zenject;

namespace Shmigliki.Application.Data
{
    [Serializable]
    public class GameState
    {
        [JsonProperty] private List<MonsterStateData> _monsters;
        [JsonProperty] private MonsterStateData _currentMonsterToSpawn;
        [JsonProperty] private MonsterStateData _nextMonsterToSpawn;
        [JsonProperty] private Score _score;
        //TODO: add next and current spawnable + passive and active items for the run + level/map

        [JsonConstructor]
        public GameState(List<MonsterStateData> monsters, MonsterStateData currentToSpawn, MonsterStateData nextToSpawn, Score score)
        {
            _monsters = monsters;
            _score = score;
        }

        public GameState(List<Monster> monsters, Monster currentToSpawn, Monster nextToSpawn, Score score)
        {
            _score = score;

            _monsters = new List<MonsterStateData>();
            _currentMonsterToSpawn = currentToSpawn ? new MonsterStateData(currentToSpawn) : null;
            _nextMonsterToSpawn = nextToSpawn ? new MonsterStateData(nextToSpawn): null;

            foreach (var monster in monsters)
                _monsters.Add(new MonsterStateData(monster));
        }

        public Score Score => _score;

        public Monster CreateCurrentToSpawnOrDefault(IMonsterFactory monstersFactory)
        {
            if (_currentMonsterToSpawn == null)
                return null;

            return CreateMonsterFromSavedState(monstersFactory, _currentMonsterToSpawn);
        }

        public Monster CreateNextToSpawnOrDefault(IMonsterFactory monstersFactory)
        {
            if (_nextMonsterToSpawn == null)
                return null;

            return CreateMonsterFromSavedState(monstersFactory, _nextMonsterToSpawn);
        }

        public List<Monster> CreateMonsters(IMonsterFactory monstersFactory)
        {
            var monsters = new List<Monster>();

            foreach (var monsterStateData in _monsters)
            {
                var newMonster = CreateMonsterFromSavedState(monstersFactory, monsterStateData);
                monsters.Add(newMonster);
            }

            return monsters;
        }

        private Monster CreateMonsterFromSavedState(IMonsterFactory factory, MonsterStateData stateData)
        {
            var newMonster = factory.Create(0, Vector3.zero);
            stateData.RestoreState(newMonster);
            return newMonster;
        }
    }
}