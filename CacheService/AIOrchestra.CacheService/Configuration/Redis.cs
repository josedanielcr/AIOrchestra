using StackExchange.Redis;

namespace AIOrchestra.CacheService.Configuration
{
    public static class Redis
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var RedisConfiguration = configuration.GetSection("Redis")["Configuration"];
                return ConnectionMultiplexer.Connect(RedisConfiguration!);
            });
            return services;
        }
    }
}
