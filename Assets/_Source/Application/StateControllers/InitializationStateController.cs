using Cysharp.Threading.Tasks;
using Services.SceneLoadingService;
using Shmigliki.Application.Data;
using Shmigliki.Gameplay;
using Zenject;
using IInitializable = Zenject.IInitializable;

namespace Shmigliki.Application
{
    public class InitializationStateController : IInitializable
    {
        private DiContainer _diContainer;
        private GameStateLoader _gameStateLoader;
        private IScenesLoadingService _scenesLoading;

        public InitializationStateController(DiContainer diContainer, GameStateLoader gameStateLoader, IScenesLoadingService scenesLoading)
        {
            _diContainer = diContainer;
            _gameStateLoader = gameStateLoader;
            _scenesLoading = scenesLoading;
        }

        public void Initialize()
        {
            InitializationProcess().Forget();
        }

        private async UniTaskVoid InitializationProcess()
        {
            await _gameStateLoader.Initialize();
            _diContainer.Bind<Score>().FromInstance(_gameStateLoader.GameState.Score);

            _scenesLoading.Load(Scenes.Gameplay.ToString());
        }
    }
}