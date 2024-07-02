using AIOrchestra.APIGateway.Contracts.PlaylistService;
using AIOrchestra.APIGateway.Features.PlaylistService;
using AIOrchestra.APIGateway.Resources;
using AIOrchestra.APIGateway.Shared;
using Carter;
using CommonLibrary;
using FluentValidation;
using KafkaLibrary.Interfaces;
using Mapster;
using MediatR;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace AIOrchestra.APIGateway.Features.PlaylistService
{
    public static class GetUserPlaylists
    {
        public class Command : BaseRequest, IRequest<BaseResponse>
        {
            public string UserId { get; set; } = string.Empty;
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.UserId).NotEmpty();
            }
        }

        internal sealed class Handler : IRequestHandler<Command, BaseResponse>
        {
            private readonly IProducer producer;
            private readonly IValidator<Command> validator;

            public Handler(IProducer producer, IValidator<Command> validator)
            {
                this.producer = producer;
                this.validator = validator;
            }

            public async Task<BaseResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                BaseResponse response = await APIUtils.ExecuteBaseRequest(request, "GetUserPlaylists", producer, validator);
                return response;
            }
        }
    }
}

public class GetUserPlaylistEndpont : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        string endpoint = ServiceEndpoints.PlaylistService_User;
        string endpointPrefix = ServiceEndpoints.EndpointPrefix;
        app.MapPost(endpointPrefix + endpoint, async (GetUserPlaylistsReq request, ISender sender) =>
        {
            request.TargetTopic = Topics.PlaylistService;
            var command = request.Adapt<GetUserPlaylists.Command>();
            command.Value = new
            {
                request.UserId
            };
            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                return Results.BadRequest(result);
            }
            addSongIdsToResponse(result);
            return Results.Ok(result);
        }).RequireAuthorization();
    }

    private void addSongIdsToResponse(BaseResponse result)
    {
        var resultVal = (List<object>)result.Value;
        foreach (var playlist in resultVal)
        {
            var playlistDic = (Dictionary<string, object>)playlist;
            var songIds = (JToken)playlistDic["SongIds"];
            var songIdsResponse = new List<string>();
            foreach (var songId in songIds)
            {
                songIdsResponse.Add(songId.ToString());
            }
            playlistDic["SongIds"] = songIdsResponse;
        }
    }
}