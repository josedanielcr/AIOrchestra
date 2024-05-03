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
            }).AddJwtBearer(options =>
            {
                options.Authority = "https://dev-qcwyqko8w0p3q5ht.us.auth0.com/";
                options.Audience = "https://localhost:8083";
            });
            return services;
        }
    }
}