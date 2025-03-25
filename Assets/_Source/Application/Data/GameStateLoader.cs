using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Services.SaveLoadService;
using Shmigliki.Gameplay;

namespace Shmigliki.Application.Data
{
    public class GameStateLoader
    {
        private ISaveLoadService _saveLoadService;
        private string _gameDataID;

        private GameState _gameState;

        public GameStateLoader(ISaveLoadService saveLoadService, string gameDataId)
        {
            _saveLoadService = saveLoadService;
            _gameDataID = gameDataId;
        }

        public GameState GameState => _gameState;

        public async UniTask<GameState> Initialize()
        {
            if (_saveLoadService.IsExist(_gameDataID))
                _gameState = await _saveLoadService.Load<GameState>(_gameDataID);
            else
                _gameState = new GameState(new List<Monster>(), null, null, new Score(0, 1f, 0));
            
            return _gameState;
        }

        public void Clear()
        {
            _gameState.Score.Reset();
            _gameState = new GameState(new List<Monster>(), null, null, _gameState != null ? _gameState.Score : new Score(0, 1f, 0));
        }

        public void SaveGameState(IReadOnlyList<Monster> monsters, Monster currentToSpawn, Monster nextMonster, Score score)
        {
            _gameState = new GameState(monsters.ToList(), currentToSpawn, nextMonster, score);
            _saveLoadService.Save(_gameState, _gameDataID);
        }
    }
}