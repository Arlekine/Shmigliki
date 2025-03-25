using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Shmigliki.Gameplay
{
    [CreateAssetMenu(menuName = "Configs/MonsterConfig", fileName = "Monster Config")]
    public class MonsterConfig : ScriptableObject
    {
        [Serializable]
        public class MonsterData
        {
            [SerializeField] private Color _color = Color.white;
            [SerializeField] private float _sizeModificatorModificator;

            public Color Color => _color;
            public float SizeModificator => _sizeModificatorModificator;
        }

        [SerializeField] private List<MonsterData> _datas = new List<MonsterData>();
        [SerializeField] private float _newCreatedTime = 1f;

        public MonsterData GetData(int index) => _datas[index];
        public int MaxLevel => _datas.Count - 1;
        public float NewCreatedTime => _newCreatedTime;
    }
}