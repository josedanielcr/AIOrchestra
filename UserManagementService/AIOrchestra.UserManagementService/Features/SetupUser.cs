﻿using AIOrchestra.UserManagementService.Common.Entities;
using AIOrchestra.UserManagementService.Common.Enums;
using AIOrchestra.UserManagementService.Database;
using AIOrchestra.UserManagementService.Requests;
using AIOrchestra.UserManagementService.Shared;
using CommonLibrary;
using KafkaLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
            BaseResponse response = GenerateBaseResponse.GenerateBaseResponseSync(request);
            try
            {
                User userReq = ExtractUserFromRequest(request);
                ValidateUserFields(userReq);

                (User user, bool wasFound) = await userDbUtils.GetUserFromDbIfExists(userReq);

                await UpdateUserDbAsync(userReq, user, wasFound);

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

        private async Task UpdateUserDbAsync(User userReq, User user, bool wasFound)
        {
            if (wasFound)
            {
                user.IsProfileCompleted = true;
                user.Email = userReq.Email;
                user.Name = userReq.Name;
                user.Nickname = userReq.Nickname;
                user.Age = userReq.Age;
                user.Country = userReq.Country;
                user.Genre = userReq.Genre;
                user.Language = userReq.Language;
                user.Ethnicity = userReq.Ethnicity;
                userDbUtils.dbContext.Entry(user).State = EntityState.Modified;
                await userDbUtils.UpdateUserInDatabaseAsync(user);
            }
        }

        private void ValidateUserFields(User user)
        {
            if (string.IsNullOrEmpty(user.Name)
                || string.IsNullOrEmpty(user.Email)
                || string.IsNullOrEmpty(user.Nickname)
                || user.Age < 0
                || string.IsNullOrEmpty(user.Country))
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
                Country = createUserRequest!.Country,
                Genre = (Genre)Enum.Parse(typeof(Genre), createUserRequest!.Genre, true),
                Language = (Languages)Enum.Parse(typeof(Languages), createUserRequest!.Language, true),
                Ethnicity = (Ethnicity)Enum.Parse(typeof(Ethnicity), createUserRequest!.Ethnicity, true)
            };
        }
    }
}