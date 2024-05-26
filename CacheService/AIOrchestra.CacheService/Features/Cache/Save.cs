using CommonLibrary;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace AIOrchestra.CacheService.Features.Cache
{
    public class Save
    {
        private readonly IConnectionMultiplexer redisConnectionMultiplexer;

        public Save(IConnectionMultiplexer redisConnectionMultiplexer)
        {
            this.redisConnectionMultiplexer = redisConnectionMultiplexer;
        }
        public void SaveInToCache(BaseRequest baseRequest)
        {
            var db = redisConnectionMultiplexer.GetDatabase();
            db.StringSet(baseRequest.OperationId, JsonConvert.SerializeObject(baseRequest));
        }
    }
}
