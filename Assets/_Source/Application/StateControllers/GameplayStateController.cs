using System;
using EventBusSystem;
using Services.SceneLoadingService;
using Shmigliki.Application.Data;
using Shmigliki.Gameplay;
using UnityEngine;
using Zenject;

namespace Shmigliki.Application
{
    public class GameplayStateController : ILoseHandler, ITickable, IApplicationQuit
    {
        private const float SAVE_OFFSET = 2f;

        private GameStateLoader _gameStateLoader;
        private SpawningController _spawningController;
        private IMonsterFactory _monsterFactory;
        private IMonsterHolder _monsterHolder;
        private IScenesLoadingService _scenesLoadingService;

        private float _nextSaveTime;

        public GameplayStateController(GameStateLoader gameStateLoader, SpawningController spawningController, IMonsterHolder monsterHolder, IMonsterFactory monsterFactory, IScenesLoadingService scenesLoadingService)
        {
            _monsterHolder = monsterHolder;
            _gameStateLoader = gameStateLoader;
            _spawningController = spawningController;
            _monsterFactory = monsterFactory;
            _scenesLoadingService = scenesLoadingService;
        }

        [Inject]
        private void Initialize()
        {
            var gameState = _gameStateLoader.GameState;
            var monsters = gameState.CreateMonsters(_monsterFactory);
            _monsterHolder.SetInitialMonsters(monsters);

            _spawningController.StartSpawning(gameState.CreateCurrentToSpawnOrDefault(_monsterFactory), gameState.CreateNextToSpawnOrDefault(_monsterFactory));

            EventBus.Subscribe(this);
            _nextSaveTime = Time.time + SAVE_OFFSET;
        }

        public void Dispose()
        {
            EventBus.Unsubscribe(this);
        }

        public void OnLost()
        {
            _gameStateLoader.Clear(); 
            _scenesLoadingService.Load(Scenes.Gameplay.ToString());

            //TODO: show lose UI
        }

        public void Tick()
        {
            if (Time.time >= _nextSaveTime)
            {
                Save();
                _nextSaveTime = Time.time + SAVE_OFFSET;
            }
        }

        private void Save() =>
            _gameStateLoader.SaveGameState(_monsterHolder.CurrentMonsters, _spawningController.CurrentMonster, _spawningController.NextMonster, _gameStateLoader.GameState.Score);

        public void OnApplicationQuit() => Save();
        
    }
}