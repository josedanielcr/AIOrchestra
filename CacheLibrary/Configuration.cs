using CacheLibrary.Implementations;
using CacheLibrary.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace CacheLibrary
{
    public static class Configuration
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var RedisConfiguration = configuration.GetSection("Redis")["Configuration"];
                return ConnectionMultiplexer.Connect(RedisConfiguration!);
            });

            services.AddSingleton<ICacheUtils, CacheUtils>();
            return services;
        }
    }
}