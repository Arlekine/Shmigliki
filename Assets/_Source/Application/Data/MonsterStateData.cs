using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Shmigliki.Gameplay;
using Unity.Mathematics;
using UnityEngine;

namespace Shmigliki.Application.Data
{
    [Serializable]
    public class MonsterStateData
    {
        [JsonProperty] private List<MonsterTag> _tags = new List<MonsterTag>();
        [JsonProperty] private float _positionX;
        [JsonProperty] private float _positionY;
        [JsonProperty] private float _rotationX;
        [JsonProperty] private float _rotationY;
        [JsonProperty] private int _sizeOffset;
        [JsonProperty] private int _level;
        [JsonProperty] private int _age;

        [JsonConstructor]
        public MonsterStateData(float positionX, float positionY, float rotationX, float rotationY, int sizeOffset, int level, int age)
        {
            _positionX = positionX;
            _positionY = positionY;
            _rotationX = rotationX;
            _rotationY = rotationY;
            _sizeOffset = sizeOffset;
            _level = level;
            _age = age;
        }

        public MonsterStateData(Monster monster)
        {
            _tags = monster.Tags.Cast<MonsterTag>().ToList();

            _positionX = monster.transform.position.x;
            _positionY = monster.transform.position.y;
            _rotationX = monster.transform.eulerAngles.x;
            _rotationY = monster.transform.eulerAngles.y;

            _sizeOffset = monster.SizeLevelOffset;
            _level = monster.Level;
            _age = monster.Age;

        }

        public void RestoreState(Monster monster)
        {
            foreach (var monsterTag in _tags)
            {
                if (monsterTag is not Busy)
                    monster.AddTag((MonsterTag)monsterTag);
            }

            monster.transform.position = new Vector3(_positionX, _positionY);
            monster.transform.eulerAngles = new Vector3(_rotationX, _rotationY);

            for (int i = 0; i < _level; i++)
                monster.Grow(0f);

            for (int i = 0; i < _age; i++)
                monster.IncreaseAge();

            monster.SetSizeLevelOffset(_sizeOffset);
        }
    }
}
