using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Stocks.Core.Cache
{
    public class OfflineCachedRepository : ICachedRepository
    {
        string folderPath = @"c:\temp\";
        private string CreateFileName(string key) => $"{folderPath}{key}.json";

        private string ReadFromCache(string key)
        {
            var fileName = CreateFileName(key);
            string value = null;
            if (File.Exists(fileName))
                value = File.ReadAllText(fileName);
            return value;
        }

        public T ReadFromCache<T>(string key)
        {
            var value = ReadFromCache(key);
            return value is null ? default
               : JsonConvert.DeserializeObject<T>(value);
        }

        public async Task<T> ReadFromCacheAsync<T>(string key)
        {
            return await Task.Run(() => ReadFromCache<T>(key));
        }

        public async Task<string> ReadStringFromCacheAsync(string key)
        {
            return ReadFromCache(key);
        }

        public async Task WriteToCacheAsync<T>(string key, T value, int? expiration = null)
        {
            File.WriteAllText(CreateFileName(key), JsonConvert.SerializeObject(value));
        }
    }
}
