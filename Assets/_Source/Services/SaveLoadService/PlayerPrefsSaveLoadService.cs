using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Services.SaveLoadService
{
    public class PlayerPrefsSaveLoadService : ISaveLoadService
    {
        public bool IsExist(string fileName) => PlayerPrefs.HasKey(fileName);

        public void Save<T>(T data, string fileName)
        {
            PlayerPrefs.SetString(fileName, JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            }));
        }

        public async UniTask<T> Load<T>(string fileName)
        {
            if (IsExist(fileName) == false)
                throw new ArgumentException($"File {fileName} doesn't exist");

            return LoadFileFromFullPath<T>(fileName);
        }

        public void Delete(string fileName)
        {
            if (IsExist(fileName))
                PlayerPrefs.DeleteKey(fileName);
        }

        private T LoadFileFromFullPath<T>(string fullPath)
        {
            var jsonString = PlayerPrefs.GetString(fullPath);
            var data = JsonConvert.DeserializeObject<T>(jsonString, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            if (data == null)
                throw new ArgumentException($"File {Path.GetFileName(fullPath)} doesn't contain data of type {typeof(T).Name}");

            return data;
        }
    }
}