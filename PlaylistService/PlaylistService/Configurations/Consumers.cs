using PlaylistService.Consumers;

namespace PlaylistService.Configurations
{
    public static class Consumers
    {
        public static IServiceCollection AddConsumers(this IServiceCollection services)
        {
            services.AddHostedService<PlaylistConsumer>();
            return services;
        }
    }
}
