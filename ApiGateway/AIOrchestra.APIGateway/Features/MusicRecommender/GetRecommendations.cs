using AIOrchestra.APIGateway.Contracts.MusicRecommender.Requests;
using AIOrchestra.APIGateway.Contracts.UserManagement.Requests;
using AIOrchestra.APIGateway.Features.MusicRecommender;
using AIOrchestra.APIGateway.Resources;
using AIOrchestra.APIGateway.Shared;
using Carter;
using CommonLibrary;
using FluentValidation;
using KafkaLibrary.Interfaces;
using Mapster;
using MediatR;
using System.Net;

namespace AIOrchestra.APIGateway.Features.MusicRecommender
{
    public static class GetRecommendations
    {
        private static readonly string HandlerMethod = "recommendation.recommend";
        public class Command : BaseRequest, IRequest<BaseResponse>
        {
            public int Danceability { get; set; }
            public int Energy { get; set; }
            public int Loudness { get; set; }
            public int Speechiness { get; set; }
            public int Instrumentalness { get; set; }
            public int Liveness { get; set; }
            public IEnumerable<string> Songs { get; set; } = new List<string>();
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Danceability).NotEmpty();
                RuleFor(x => x.Energy).NotEmpty();
                RuleFor(x => x.Loudness).NotEmpty();
                RuleFor(x => x.Speechiness).NotEmpty();
                RuleFor(x => x.Instrumentalness).NotEmpty();
                RuleFor(x => x.Liveness).NotEmpty();
            }
        }

        internal sealed class Handler : IRequestHandler<Command, BaseResponse>
        {
            private readonly IProducer producer;
            private readonly IValidator<Command> validator;

            public Handler(IProducer producer,
                               IValidator<Command> validator)
            {
                this.producer = producer;
                this.validator = validator;
            }

            public async Task<BaseResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                IValidator<BaseRequest> validator = (IValidator<BaseRequest>)this.validator;
                BaseResponse response = await APIUtils.ExecuteBaseRequest(request, HandlerMethod, producer, validator);
                return response;
            }
        }
    }
}

//path: AIOrchestra.APIGateway/Features/MusicRecommender/GetRecommendationEndpoint.cs
public class GetRecommendationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        string endpoint = ServiceEndpoints.MusicRecommender;
        string endpointPrefix = ServiceEndpoints.EndpointPrefix;
        app.MapPost(endpointPrefix + endpoint, async (GetRecommendationReq request, ISender sender) =>
        {
            request.TargetTopic = Topics.MusicRecommender;
            var command = request.Adapt<GetRecommendations.Command>();
            command.Value = new
            {
                request.Danceability,
                request.Energy,
                request.Loudness,
                request.Speechiness,
                request.Instrumentalness,
                request.Liveness,
                request.Songs
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