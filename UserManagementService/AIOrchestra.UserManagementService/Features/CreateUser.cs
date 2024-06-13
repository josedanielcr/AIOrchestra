using AIOrchestra.UserManagementService.Common.Entities;
using AIOrchestra.UserManagementService.Database;
using AIOrchestra.UserManagementService.Requests;
using AIOrchestra.UserManagementService.Shared;
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
        private readonly UserDbUtils userDbUtils;
        private readonly IProducer producer;

        public CreateUser(UserDbUtils userDbUtils, IProducer producer)
        {
            this.userDbUtils = userDbUtils;
            this.producer = producer;
        }

        public async Task CreateUserAsync(BaseRequest request)
        {
            BaseResponse response = GenerateBaseResponse.GenerateBaseResponseSync(request);

            try
            {
                User userReq = ExtractUserFromRequest(request);
                ValidateUserFields(userReq);

                User user;
                (user, bool wasFound) = await userDbUtils.GetUserFromDbIfExists(userReq);

                if (!wasFound)
                {
                    await userDbUtils.AddUserToDatabaseAsync(userReq);
                    user = userReq;
                }

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