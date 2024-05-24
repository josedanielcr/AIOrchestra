using AIOrchestra.CacheService.Consumers;

namespace AIOrchestra.CacheService.Configuration
{
    public static class Consumer
    {
        public static IServiceCollection AddConsumers(this IServiceCollection services)
        {
            services.AddHostedService<CacheConsumer>();
            return services;
        }
    }
}
