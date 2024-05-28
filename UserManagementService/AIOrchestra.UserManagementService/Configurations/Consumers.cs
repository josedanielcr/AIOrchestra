using AIOrchestra.UserManagementService.Consumers;

namespace AIOrchestra.UserManagementService.Configurations
{
    public static class Consumers
    {
        public static IServiceCollection AddConsumers(this IServiceCollection services)
        {
            services.AddHostedService<UserManagementConsumer>();
            return services;
        }
    }
}
