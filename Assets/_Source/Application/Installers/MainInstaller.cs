using Services.SaveLoadService;
using Services.SceneLoadingService;
using Shmigliki.Application.Data;
using UnityEngine;
using Zenject;

namespace Shmigliki.Application.Installers
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private string _gameStateDataID;

        public override void InstallBindings()
        {
            EventBusSystem.EventBus.Clear();
            EventBusSystem.EventBusHelper.Clear();

            Container.Bind<ISaveLoadService>().To<PlayerPrefsSaveLoadService>().AsSingle();
            Container.Bind<IScenesLoadingService>().To<ZenjectSceneLoadingService>().AsSingle();

            Container.Bind<GameStateLoader>().AsSingle().WithArguments(_gameStateDataID);
            Container.BindInterfacesAndSelfTo<InitializationStateController>().AsSingle().NonLazy();
        }
    }
}