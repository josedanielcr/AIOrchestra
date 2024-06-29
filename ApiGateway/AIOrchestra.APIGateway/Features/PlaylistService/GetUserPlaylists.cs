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
                IValidator<BaseRequest> validator = (IValidator<BaseRequest>)this.validator;
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
            return Results.Ok(result);
        }).RequireAuthorization();
    }
}