using CacheLibrary.Interfaces;
using CommonLibrary;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using PlaylistService.Common.Entities;
using PlaylistService.Requests;
using PlaylistService.Shared;
using SharedLibrary;
using System.Net;

namespace PlaylistService.Features
{
    public class DeletePlaylist
    {
        private readonly ICacheUtils cacheUtils;
        private readonly PlaylistDbUtils playlistDbUtils;

        public DeletePlaylist(ICacheUtils cacheUtils, PlaylistDbUtils playlistDbUtils)
        {
            this.cacheUtils = cacheUtils;
            this.playlistDbUtils = playlistDbUtils;
        }


        public async Task DeletePlaylistAsync(BaseRequest baseRequest)
        {
            BaseResponse response = ApplicationResponseUtils.GenerateResponse(baseRequest.OperationId,
                             baseRequest.ApiVersion, true, HttpStatusCode.OK, null, null, null, null, Topics.PlaylistService, null, null, baseRequest.HandlerMethod);
            try
            {
                string playlistIdReq = ExtractPlaylistFromRequest(baseRequest);
                await playlistDbUtils.DeletePlaylistAsync(playlistIdReq);
                response.Status = RequestStatus.Completed;
                response.Value = new
                {
                    Result = true
                };
            }
            catch (Exception e)
            {
                response = ApplicationResponseUtils.AddErrorResultToResponse(response, e);
                response.Status = RequestStatus.Failed;
            }
            finally
            {
                cacheUtils.Set(response.OperationId, response);
            }
        }

        private string ExtractPlaylistFromRequest(BaseRequest baseRequest)
        {
            var createPlaylistReq = JsonConvert.DeserializeObject<DeletePlaylistReq>(baseRequest.Value.ToString()!);
            return createPlaylistReq!.PlaylistId;
        }
    }
}
