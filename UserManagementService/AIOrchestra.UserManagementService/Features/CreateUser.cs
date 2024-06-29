using AIOrchestra.UserManagementService.Common.Entities;
using AIOrchestra.UserManagementService.Database;
using AIOrchestra.UserManagementService.Requests;
using AIOrchestra.UserManagementService.Shared;
using CommonLibrary;
using Confluent.Kafka;
using KafkaLibrary.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SharedLibrary;
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
            BaseResponse response = ApplicationResponseUtils.GenerateResponse(request.OperationId,
                request.ApiVersion, true, HttpStatusCode.OK, null, null, null, null, Topics.PlaylistService, null, null, request.HandlerMethod);

            try
            {
                response = await ExecuteCreateUser(request, response);
            }
            catch (Exception e)
            {
                response = ApplicationResponseUtils.AddErrorResultToResponse(response, e);
            }
            finally
            {
                await producer.ProduceAsync(Topics.ApiGatewayResponse, request.OperationId, response);
            }
        }

        private async Task<BaseResponse> ExecuteCreateUser(BaseRequest request, BaseResponse response)
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

            return ApplicationResponseUtils.AddSuccessResultToResponse(response, user);
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