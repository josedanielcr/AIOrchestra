namespace ApiGateway.Configuration
{
    public static class MediatR
    {
        public static IServiceCollection AddApplicationMediatR(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(Program).Assembly);
            });

            return services;
        }
    }
}
