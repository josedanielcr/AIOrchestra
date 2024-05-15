using AIOrchestra.APIGateway.Configurations.Packages;

namespace AIOrchestra.APIGateway.Configurations.Packages
{
    public static class MediatR
    {
        public static IServiceCollection AddMediatR(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(Program).Assembly);
            });

            return services;
        }

    }
}
