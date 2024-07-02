using AIOrchestra.APIGateway.Contracts.PlaylistService;
using AIOrchestra.APIGateway.Contracts.UserManagement.Requests;
using AIOrchestra.APIGateway.Features.PlaylistService;
using AIOrchestra.APIGateway.Resources;
using AIOrchestra.APIGateway.Shared;
using Carter;
using CommonLibrary;
using FluentValidation;
using KafkaLibrary.Implementations;
using KafkaLibrary.Interfaces;
using Mapster;
using MediatR;

namespace AIOrchestra.APIGateway.Features.PlaylistService
{
    public static class CreatePlaylist
    {
        public class Command : BaseRequest, IRequest<BaseResponse>
        {
            public string Name { get; set; } = string.Empty;
            public string UserId { get; set; } = string.Empty;
            public List<String> SongIds { get; set; } = new List<String>();
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.UserId).NotEmpty();
                RuleFor(x => x.SongIds).NotEmpty();
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
                BaseResponse response = await APIUtils.ExecuteBaseRequest(request, "CreatePlaylist", producer, validator);
                return response;
            }
        }
    }
}

public class CreatePlaylistEndpont : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        string endpoint = ServiceEndpoints.PlaylistService;
        string endpointPrefix = ServiceEndpoints.EndpointPrefix;
        app.MapPost(endpointPrefix + endpoint, async (CreatePlaylistReq request, ISender sender) =>
        {
            request.TargetTopic = Topics.PlaylistService;
            var command = request.Adapt<CreatePlaylist.Command>();
            command.Value = new
            {
                request.Name,
                request.UserId,
                request.SongIds
            };
            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                return Results.BadRequest(result);
            }
            return Results.Ok(result);
        }).RequireAuthorization();
    }
}