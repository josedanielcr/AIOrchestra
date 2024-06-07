using AIOrchestra.UserManagementService.Common.Entities;
using AIOrchestra.UserManagementService.Database;
using AIOrchestra.UserManagementService.Requests;
using CommonLibrary;
using Confluent.Kafka;
using KafkaLibrary.Interfaces;
using Newtonsoft.Json;
using System.Net;

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
            BaseResponse response = GenerateBaseResponse(request);

            try
            {
                User user = ExtractUserFromRequest(request);
                ValidateUserFields(user);
                await AddUserToDatabaseAsync(user);

                response.IsSuccess = true;
                response.IsFailure = false;
                response.StatusCode = HttpStatusCode.OK;
                response.Value = user;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.IsFailure = true;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Error = new CommonLibrary.Error
                {
                    Code = HttpStatusCode.InternalServerError.ToString(),
                    Message = e.Message,
                    Details = null
                };
            }
            finally
            {
                await producer.ProduceAsync(Topics.ApiGatewayResponse, request.OperationId, response);
            }
        }

        private void ValidateUserFields(User user)
        {
            if (user == null)
            {
                throw new Exception("User is null");
            }

            var dbUser = dbContext.Users.FirstOrDefault(u => u.Email == user.Email);
            if (dbUser != null)
            {
                throw new Exception("User already exists");
            }

            if (user.Name == null || user.Email == null)
            {
                throw new Exception("User name or email is null");
            }
        }

        private static BaseResponse GenerateBaseResponse(BaseRequest request)
        {
            return new BaseResponse
            {
                OperationId = request.OperationId,
                ApiVersion = request.ApiVersion,
                ServicedBy = Topics.UserManagement,
                HandlerMethod = request.HandlerMethod,
                ProcessingTime = 0,
                AdditionalDetails = null,
                Value = null
            };
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