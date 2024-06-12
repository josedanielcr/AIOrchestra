using AIOrchestra.UserManagementService.Common.Entities;
using AIOrchestra.UserManagementService.Database;

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

        public (User, bool) GetUserFromDbIfExists(User user)
        {
            var dbUser = dbContext.Users.FirstOrDefault(u => u.Id == user.Id);
            if (dbUser != null)
            {
                return (dbUser, true);
            }
            return (user, false);
        }
    }
}
