using Microsoft.EntityFrameworkCore;
using PlaylistService.Database;

namespace PlaylistService.Configurations
{
    public static class Database
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("DefaultConnection:PlaylistManagementDatabase").Value!;
            var databaseName = configuration.GetSection("DefaultConnection:PlaylistManagementDatabaseName").Value!;
            services.AddDbContext<PlaylistManagementDbContext>(options => options.UseMongoDB(connectionString, databaseName));
            return services;
        }
    }
}