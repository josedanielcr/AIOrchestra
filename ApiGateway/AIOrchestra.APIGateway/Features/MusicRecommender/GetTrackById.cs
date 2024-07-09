using AIOrchestra.APIGateway.Contracts.MusicRecommender.Requests;
using AIOrchestra.APIGateway.Features.MusicRecommender;
using AIOrchestra.APIGateway.Resources;
using AIOrchestra.APIGateway.Shared;
using Carter;
using CommonLibrary;
using FluentValidation;
using KafkaLibrary.Interfaces;
using Mapster;
using MediatR;

namespace AIOrchestra.APIGateway.Features.MusicRecommender
{
    public class GetTrackById
    {
        private static readonly string HandlerMethod = "track.get_tracks_by_ids";
        public class Command : BaseRequest, IRequest<BaseResponse>
        {
            public List<string> TrackIds { get; set; } = new List<string>();
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.TrackIds).NotEmpty();
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
                BaseResponse response = await APIUtils.ExecuteBaseRequest(request, HandlerMethod, producer, validator);
                return response;
            }
        }
    }
}

public class GetTracksByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        string endpoint = ServiceEndpoints.MusicRecommender_getTrack;
        string endpointPrefix = ServiceEndpoints.EndpointPrefix;
        app.MapPost(endpointPrefix + endpoint, async (GetTrackByIdReq request, ISender sender) =>
        {
            request.TargetTopic = Topics.MusicRecommender;
            var command = request.Adapt<GetTrackById.Command>();
            command.Value = new
            {
                request.TrackIds
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