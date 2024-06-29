using AIOrchestra.UserManagementService.Common.Entities;
using AIOrchestra.UserManagementService.Requests;
using AIOrchestra.UserManagementService.Shared;
using CommonLibrary;
using KafkaLibrary.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SharedLibrary;
using System.Net;

namespace AIOrchestra.UserManagementService.Features
{
    public class SetupUser
    {
        private readonly UserDbUtils userDbUtils;
        private readonly IProducer producer;

        public SetupUser(UserDbUtils userDbUtils, IProducer producer)
        {
            this.userDbUtils = userDbUtils;
            this.producer = producer;
        }

        public async Task SetupUserAsync(BaseRequest request)
        {
            BaseResponse response = ApplicationResponseUtils.GenerateResponse(request.OperationId,
                request.ApiVersion, true, HttpStatusCode.OK, null, null, null, null, Topics.PlaylistService, null, null, request.HandlerMethod);

            try
            {
                response = await ExecuteSetupUser(request, response);
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

        private async Task<BaseResponse> ExecuteSetupUser(BaseRequest request, BaseResponse response)
        {
            User userReq = ExtractUserFromRequest(request);
            ValidateUserFields(userReq);

            (User user, bool wasFound) = await userDbUtils.GetUserFromDbIfExists(userReq);

            await UpdateUserDbAsync(userReq, user, wasFound);

            response = ApplicationResponseUtils.AddSuccessResultToResponse(response, user);
            return response;
        }

        private async Task UpdateUserDbAsync(User userReq, User user, bool wasFound)
        {
            if (wasFound)
            {
                user.IsProfileCompleted = true;
                user.Email = userReq.Email;
                user.Name = userReq.Name;
                user.Nickname = userReq.Nickname;
                user.Age = userReq.Age;
                user.Danceability = userReq.Danceability;
                user.Energy = userReq.Energy;
                user.Loudness = userReq.Loudness;
                user.Speechiness = userReq.Speechiness;
                user.Instrumentalness = userReq.Instrumentalness;
                user.Liveness = userReq.Liveness;
                userDbUtils.dbContext.Entry(user).State = EntityState.Modified;
                await userDbUtils.UpdateUserInDatabaseAsync(user);
            }
        }

        private void ValidateUserFields(User user)
        {
            if (string.IsNullOrEmpty(user.Name)
                || string.IsNullOrEmpty(user.Email)
                || string.IsNullOrEmpty(user.Nickname)
                || user.Age < 0)
            {
                throw new Exception("You must provide all neccesary data to continue the algorithm training");
            }
        }

        private User ExtractUserFromRequest(BaseRequest request)
        {
            var createUserRequest = JsonConvert.DeserializeObject<SetupUserRequest>(request.Value.ToString()!);
            return new User
            {
                Email = createUserRequest!.Email,
                Nickname = createUserRequest!.Nickname,
                Name = createUserRequest!.Name,
                Age = createUserRequest!.Age,
                Danceability = createUserRequest!.Danceability,
                Energy = createUserRequest!.Energy,
                Loudness = createUserRequest!.Loudness,
                Speechiness = createUserRequest!.Speechiness,
                Instrumentalness = createUserRequest!.Instrumentalness,
                Liveness = createUserRequest!.Liveness
            };
        }
    }
}