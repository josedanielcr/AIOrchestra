using AIOrchestra.UserManagementService.Common.Entities;
using AIOrchestra.UserManagementService.Database;
using Microsoft.EntityFrameworkCore;

namespace AIOrchestra.UserManagementService.Shared
{
    public class UserDbUtils
    {
        private readonly UserManagementDbContext dbContext;

        public UserDbUtils(UserManagementDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddUserToDatabaseAsync(User user)
        {
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserInDatabaseAsync(User user)
        {
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task<(User, bool)> GetUserFromDbIfExists(User user)
        {
            var foundUser = await dbContext.Users.Where(u => u.Email == user.Email).FirstOrDefaultAsync();
            return (foundUser!, foundUser != null);
        }
    }
}
