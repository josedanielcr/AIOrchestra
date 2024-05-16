namespace AIOrchestra.APIGateway.Configurations
{
    public static class Cors
    {
        public static IServiceCollection AddApplicationCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                                       builder =>
                                       {
                                           builder.WithOrigins("http://localhost:4200")
                                               .AllowAnyHeader()
                                               .AllowAnyMethod();
                                       });
            });
            return services;
        }
    }
}
