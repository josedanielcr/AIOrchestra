using AIOrchestra.APIGateway.Contracts.PlaylistService;
using AIOrchestra.APIGateway.Contracts.PlaylistService.Requests;
using AIOrchestra.APIGateway.Features.PlaylistService;
using AIOrchestra.APIGateway.Resources;
using AIOrchestra.APIGateway.Shared;
using Carter;
using CommonLibrary;
using FluentValidation;
using KafkaLibrary.Interfaces;
using Mapster;
using MediatR;

namespace AIOrchestra.APIGateway.Features.PlaylistService
{
    public static class DeletePlaylist
    {
        public class Command : BaseRequest, IRequest<BaseResponse>
        {
            public string PlaylistId { get; set; } = string.Empty;
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.PlaylistId).NotEmpty();
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
                BaseResponse response = await APIUtils.ExecuteBaseRequest(request, "DeletePlaylist", producer, validator);
                return response;
            }
        }
    }
}

public class DeletePlaylistEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        string endpoint = ServiceEndpoints.PlaylistService_Delete;
        string endpointPrefix = ServiceEndpoints.EndpointPrefix;
        app.MapPost(endpointPrefix + endpoint, async (DeletePlaylistReq request, ISender sender) =>
        {
            request.TargetTopic = Topics.PlaylistService;
            var command = request.Adapt<DeletePlaylist.Command>();
            command.Value = new
            {
                request.PlaylistId
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