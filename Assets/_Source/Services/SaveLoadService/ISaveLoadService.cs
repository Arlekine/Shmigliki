using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Services.SaveLoadService
{
    public interface ISaveLoadService
    {
        bool IsExist(string fileName);
        void Save<T>(T data, string fileName);
        UniTask<T> Load<T>(string fileName);
        void Delete(string fileName);
    }
}