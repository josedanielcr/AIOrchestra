using AIOrchestra.UserManagementService.Database;
using Microsoft.EntityFrameworkCore;

namespace AIOrchestra.UserManagementService.Configurations
{
    public static class Database
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("DefaultConnection:UserManagementDatabase").Value!;
            var databaseName = configuration.GetSection("DefaultConnection:UserManagementDatabaseName").Value!;
            services.AddDbContext<UserManagementDbContext>(options => options.UseMongoDB(connectionString, databaseName));
            return services;
        }
    }
}
