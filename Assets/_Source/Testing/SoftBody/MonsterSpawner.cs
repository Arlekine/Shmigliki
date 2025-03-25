using System.Collections;
using TMPro;
using UnityEngine;

namespace Testing.Softbody
{
    public class MonsterSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _prefabs;
        [SerializeField] private Transform _spawnMax;
        [SerializeField] private Transform _spawnMin;
        [SerializeField] private float _spawnPause;

        [Space]
        [SerializeField] private TMP_Text _counter;

        private int _spawnedMonsters = 0;

        private IEnumerator Start()
        {
            while (true)
            {
                var prefab = _prefabs[Random.Range(0, _prefabs.Length)];
                var randomPoint = new Vector3(Random.Range(_spawnMin.position.x, _spawnMax.position.x), _spawnMax.position.y, 0f);
                var monster = Instantiate(prefab, randomPoint, Quaternion.Euler(new Vector3(0f, 0f, Random.Range(-90f, 90f))));
                monster.transform.localScale = Vector3.one * Random.Range(0.35f, 1.5f);
                _spawnedMonsters++;
                _counter.text = _spawnedMonsters.ToString();

                yield return new WaitForSeconds(_spawnPause);
            }
        }
    }
}
