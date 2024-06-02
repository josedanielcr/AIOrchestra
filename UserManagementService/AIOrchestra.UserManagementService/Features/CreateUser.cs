using AIOrchestra.UserManagementService.Database;
using AIOrchestra.UserManagementService.Entities;
using AIOrchestra.UserManagementService.Requests;
using CommonLibrary;
using KafkaLibrary.Interfaces;
using Newtonsoft.Json;

namespace AIOrchestra.UserManagementService.Features
{
    public class CreateUser
    {
        private readonly UserManagementDbContext dbContext;
        private readonly IProducer producer;

        public CreateUser(UserManagementDbContext dbContext, IProducer producer)
        {
            this.dbContext = dbContext;
            this.producer = producer;
        }

        public async Task CreateUserAsync(BaseRequest request)
        {
            User user = ExtractUserFromRequest(request);
            await AddUserToDatabaseAsync(user);
            await SendResponseToApiGateway(request, user);
        }

        private async Task SendResponseToApiGateway(BaseRequest request, User user)
        {
            var response = SharedLibrary.GenerateApplicationResponse.GenerateResponse(
                            request.OperationId,
                            request.ApiVersion,
                            true,
                            System.Net.HttpStatusCode.OK,
                            null,
                            null,
                            null,
                            null,
                            Topics.UserManagement,
                            null,
                            user,
                            request.HandlerMethod
            );

            await producer.ProduceAsync(Topics.ApiGatewayResponse, request.OperationId, response);
        }

        private async Task AddUserToDatabaseAsync(User user)
        {
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