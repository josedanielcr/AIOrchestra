using CommonLibrary;
using KafkaLibrary.Implementations;
using KafkaLibrary.Interfaces;
using Newtonsoft.Json;
using PlaylistService.Common.Entities;
using PlaylistService.Requests;
using PlaylistService.Shared;
using SharedLibrary;
using System.Net;

namespace PlaylistService.Features
{
    public class GetUserPlaylists
    {
        private readonly PlaylistDbUtils playlistDbUtils;
        private readonly IProducer producer;

        public GetUserPlaylists(PlaylistDbUtils playlistDbUtils, IProducer producer)
        {
            this.playlistDbUtils = playlistDbUtils;
            this.producer = producer;
        }

        public async Task GetUserPlaylistsAsync(BaseRequest baseRequest)
        {
            BaseResponse response = ApplicationResponseUtils.GenerateResponse(baseRequest.OperationId,
                               baseRequest.ApiVersion, true, HttpStatusCode.OK, null, null, null, null, Topics.PlaylistService, null, null, baseRequest.HandlerMethod);
            try
            {
                response = await ExecuteGetUserPlaylists(baseRequest, response);
            }
            catch (Exception e)
            {
                response = ApplicationResponseUtils.AddErrorResultToResponse(response, e);
            }
            finally
            {
                await producer.ProduceAsync(Topics.ApiGatewayResponse, baseRequest.OperationId, response);
            }
        }
        private async Task<BaseResponse> ExecuteGetUserPlaylists(BaseRequest baseRequest, BaseResponse response)
        {
            Playlist playlistReq = ExtractPlaylistFromRequest(baseRequest)
                ?? throw new Exception("Playlist request is null");

            var result = await playlistDbUtils.GetUserPlaylistsAsync(playlistReq.UserId)
                ?? throw new Exception("No playlists found for user");

            return ApplicationResponseUtils.AddSuccessResultToResponse(response, result);
        }

        private Playlist ExtractPlaylistFromRequest(BaseRequest baseRequest)
        {
            var createPlaylistReq = JsonConvert.DeserializeObject<GetUserPlaylistReq>(baseRequest.Value.ToString()!);
            Playlist playlist = new Playlist
            {
                UserId = createPlaylistReq!.UserId
            };
            return playlist;
        }
    }
}