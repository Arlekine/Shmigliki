using System;
using Shmigliki.Gameplay;
using Shmigliki.Gameplay.UI;
using UnityEngine;
using UnityEngine.LowLevel;
using Zenject;

namespace Shmigliki.Application.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private bool _isTesting;

        [Space] [SerializeField] private float _minSpawnX;
        [SerializeField] private float _maxSpawnX;
        [SerializeField] private float _spawnHeight;
        [SerializeField] private float _minPauseBetweenSpawns;
        [SerializeField] private Monster _monsterPrefab;
        [SerializeField] private Transform _dynamicParent;
        [Space]
        [SerializeField] private float _loseHeight;
        [SerializeField] private float _loseTime;

        [Header("Configs")]
        [SerializeField] private GameZoneConfig _gameZoneConfig;
        [SerializeField] private MonsterConfig _monsterConfig;
        [SerializeField] private MonsterEvolveScoringConfig _monsterEvolveScoringConfig;
        [SerializeField] private ComboViewConfig _comboConfig;
        [SerializeField] private MonstersInteractionConfig _interactionConfig;


        public override void InstallBindings()
        {
            BindConfigs();

            Container.Bind<GameZone>().AsSingle();
            Container.BindInterfacesAndSelfTo<MonsterFactory>().AsSingle().WithArguments(_monsterPrefab, _dynamicParent);

            Container.Bind<Camera>().FromInstance(Camera.main).AsSingle();
            Container.BindInterfacesAndSelfTo<MouseSpawnInput>().AsSingle();

            if (_isTesting)
                Container.BindInterfacesAndSelfTo<TestingMonsterSpawner>().AsSingle().NonLazy();
            else
            {
                Container.Bind<INextSpawnableHolder>().To<DefaultNextMonsterHolder>().AsSingle();

                Container.BindInterfacesAndSelfTo<MonsterSpawner>()
                    .AsSingle()
                    .NonLazy();

                Container.BindInterfacesAndSelfTo<SpawningController>()
                    .AsSingle()
                    .WithArguments(_minPauseBetweenSpawns)
                    .NonLazy();
            }

            Container.BindInterfacesAndSelfTo<ConsumeMonsterOfSameSizeRule>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MonsterCollideSystem>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameLose>().AsSingle().WithArguments(_loseTime).NonLazy();
            
            Container.BindInterfacesAndSelfTo<MonsterEvolveScoringSystem>().AsSingle().NonLazy();

            if (_isTesting == false)
                Container.BindInterfacesAndSelfTo<GameplayStateController>().AsSingle().NonLazy();
        }

        private void BindConfigs()
        {
            Container.Bind<MonsterEvolveScoringConfig>().FromInstance(_monsterEvolveScoringConfig).AsSingle();
            Container.Bind<ComboViewConfig>().FromInstance(_comboConfig).AsSingle();
            Container.Bind<MonstersInteractionConfig>().FromInstance(_interactionConfig).AsSingle();
            Container.Bind<MonsterConfig>().FromInstance(_monsterConfig).AsSingle();
            Container.Bind<GameZoneConfig>().FromInstance(_gameZoneConfig).AsSingle();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(new Vector3(_minSpawnX, _spawnHeight, 0f), new Vector3(_minSpawnX, _spawnHeight - 5f, 0f));
            Gizmos.DrawLine(new Vector3(_maxSpawnX, _spawnHeight, 0f), new Vector3(_maxSpawnX, _spawnHeight - 5f, 0f));
            Gizmos.DrawLine(new Vector3(_maxSpawnX, _loseHeight, 0f), new Vector3(_minSpawnX, _loseHeight, 0f));
        }
    }
}