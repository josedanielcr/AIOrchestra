using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ApiGateway.Configuration
{
    public static class Authentication
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = $"https://{configuration["Auth0:Authority"]}";
                options.Audience = configuration["Auth0:Audience"];
            });
            return services;
        }
    }
}