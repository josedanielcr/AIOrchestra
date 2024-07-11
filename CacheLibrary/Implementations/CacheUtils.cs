using CacheLibrary.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CacheLibrary.Implementations
{
    public class CacheUtils : ICacheUtils
    {
        private readonly IConnectionMultiplexer connectionMultiplexer;
        private readonly IDatabase db;

        public CacheUtils(IConnectionMultiplexer connectionMultiplexer)
        {
            this.connectionMultiplexer = connectionMultiplexer;
            db = this.connectionMultiplexer.GetDatabase();
        }

        public async Task<T> Get<T>(string key)
        {
            try
            {
                var value = await db.StringGetAsync(key);
                if (value.HasValue)
                {
                    var stringValue = value.ToString();
                    return JsonConvert.DeserializeObject<T>(stringValue)!;
                }
                return default!;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void Remove(string key)
        {
            db.KeyDelete(key);
        }

        public void Set<T>(string key, T value, TimeSpan? expiry = null)
        {
            db.StringSet(key, JsonConvert.SerializeObject(value), expiry);
        }
    }
}