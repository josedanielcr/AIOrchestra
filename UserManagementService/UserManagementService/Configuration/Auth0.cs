using Auth0.AspNetCore.Authentication;

namespace UserManagementService.Configuration
{
    public static class Auth0
    {
        public static IServiceCollection AddAuth0(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuth0WebAppAuthentication(options =>
            {
                options.Domain = configuration["Auth0:Domain"]!;
                options.ClientId = configuration["Auth0:ClientId"]!;
            });

            return services;
        }
    }
}