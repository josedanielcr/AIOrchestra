using Amazon.Runtime.Internal;
using CommonLibrary;
using KafkaLibrary.Implementations;
using Newtonsoft.Json;
using PlaylistService.Common.Entities;
using PlaylistService.Requests;
using PlaylistService.Shared;
using SharedLibrary;
using System.Net;

namespace PlaylistService.Features
{
    public class CreatePlaylist
    {
        private readonly PlaylistDbUtils playlistDbUtils;
        private readonly Producer producer;

        public CreatePlaylist(PlaylistDbUtils playlistDbUtils, Producer producer)
        {
            this.playlistDbUtils = playlistDbUtils;
            this.producer = producer;
        }

        public async Task CreatePlaylistAsync(BaseRequest baseRequest)
        {
            BaseResponse response = ApplicationResponseUtils.GenerateResponse(baseRequest.OperationId,
                baseRequest.ApiVersion, true, HttpStatusCode.OK, null, null, null, null, Topics.PlaylistService, null, null, baseRequest.HandlerMethod);
            try
            {
                response = await ExecutePlaylistCreation(baseRequest, response);
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

        private async Task<BaseResponse> ExecutePlaylistCreation(BaseRequest baseRequest, BaseResponse response)
        {
            Playlist playlistReq = ExtractPlaylistFromRequest(baseRequest)
                ?? throw new Exception("Playlist request is null");

            Playlist newPlaylist;
            (newPlaylist, bool wasFound) = await playlistDbUtils.GetPlaylistAsync(playlistReq.Id);

            if (wasFound) throw new Exception("Playlist already exists");

            await playlistDbUtils.AddPlaylistAsync(playlistReq);
            newPlaylist = playlistReq;
            return ApplicationResponseUtils.AddSuccessResultToResponse(response, newPlaylist);
        }

        private Playlist ExtractPlaylistFromRequest(BaseRequest baseRequest)
        {
            var createPlaylistReq = JsonConvert.DeserializeObject<CreatePlaylistRequest>(baseRequest.Value.ToString()!);
            Playlist playlist = new Playlist
            {
                UserId = createPlaylistReq!.UserId,
                Name = createPlaylistReq!.Name,
                SongIds = createPlaylistReq!.SongIds
            };
            return playlist;
        }
    }
}