using System;
using System.IO;
using Nancy.Helpers;
using Newtonsoft.Json;

namespace MiniProxy.Caching
{
    public class FileCache : ICache
    {
        private readonly string _directoryPath;

        public FileCache()
        {
            _directoryPath = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "Cache");
            if (!Directory.Exists(_directoryPath))
            {
                Directory.CreateDirectory(_directoryPath);
            }
        }

        public T GetOrAdd<T>(string key, Func<T> factory)
        {
            var filePath = CreateFilePath(key);
            if (File.Exists(filePath))
            {
                return ReadFromFile<T>(filePath);
            }

            var data = factory();

            WriteToFile(filePath, data);

            return data;
        }

        private static T ReadFromFile<T>(string filePath)
        {
            var rawData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(rawData);
        }

        private static void WriteToFile<T>(string filePath, T data)
        {
            var serialized = JsonConvert.SerializeObject(data);
            File.WriteAllText(filePath, serialized);
        }

        private string CreateFilePath(string key)
        {
            return Path.Combine(_directoryPath, $"{HttpUtility.UrlEncode(key)}.data");
        }
    }
}