using AIOrchestra.UserManagementService.Database;
using AIOrchestra.UserManagementService.Entities;
using AIOrchestra.UserManagementService.Requests;
using AIOrchestra.UserManagementService.Shared;
using CommonLibrary;
using Newtonsoft.Json;

namespace AIOrchestra.UserManagementService.Features
{
    public class CreateUser
    {
        private readonly UserManagementDbContext dbContext;

        public CreateUser(UserManagementDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateUserAsync(BaseRequest request)
        {
            User user = ExtractUserFromRequest(request);
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
        }

        private User ExtractUserFromRequest(BaseRequest request)
        {
            var createUserRequest = JsonConvert.DeserializeObject<CreateUserRequest>(request.Value.ToString()!);
            return new User
            {
                Email = createUserRequest!.Email,
                Name = createUserRequest!.Name
            };
        }
    }
}