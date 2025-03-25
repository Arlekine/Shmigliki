using System;
using UnityEngine.SceneManagement;
using Zenject;

namespace Services.SceneLoadingService
{
    public class ZenjectSceneLoadingService : IScenesLoadingService
    {
        private Zenject.ZenjectSceneLoader _loader;

        public ZenjectSceneLoadingService(ZenjectSceneLoader loader)
        {
            _loader = loader;
        }

        public void Load(string sceneName) =>
            _loader.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        public void LoadWithData<T>(string sceneName, T data, Action additionalAction = null) =>
            _loader.LoadSceneAsync(sceneName, LoadSceneMode.Single, container =>
            {
                container.BindInterfacesAndSelfTo<T>().FromInstance(data).AsSingle();
                additionalAction?.Invoke();
            });
    }
}