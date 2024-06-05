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
            SendResponseToApiGateway(request, user);
        }

        private void SendResponseToApiGateway(BaseRequest request, User user)
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

            producer.ProduceAsync(Topics.ApiGatewayResponse, request.OperationId, response);
        }

        private async Task AddUserToDatabaseAsync(User user)
        {
            try
            {
                await dbContext.Users.AddAsync(user);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while adding user to database" + e);
                throw;
            }
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