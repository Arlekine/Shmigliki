using System;

namespace Services.SceneLoadingService
{
    public interface IScenesLoadingService
    {
        void Load(string sceneName);
        void LoadWithData<T>(string sceneName, T data, Action additionalAction = null);
    }
}