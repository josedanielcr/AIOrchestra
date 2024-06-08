using AIOrchestra.UserManagementService.Common.Entities;
using AIOrchestra.UserManagementService.Database;
using AIOrchestra.UserManagementService.Requests;
using CommonLibrary;
using Confluent.Kafka;
using KafkaLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
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

                (user, bool wasFound) = GetUserFromDbIfExists(user);

                if (!wasFound) await AddUserToDatabaseAsync(user);

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

        private (User, bool) GetUserFromDbIfExists(User user)
        {
            var dbUser = dbContext.Users.FirstOrDefault(u => u.Id == user.Id);
            if (dbUser != null)
            {
                return (dbUser, true);
            }
            return (user, false);
        }

        private void ValidateUserFields(User user)
        {
            if (user == null)
            {
                throw new Exception("User is null");
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
                Nickname = createUserRequest!.Nickname,
                Picture = createUserRequest!.Picture
            };
        }
    }
}