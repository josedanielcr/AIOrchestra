using AIOrchestra.APIGateway.Contracts.UserManagement.Requests;
using AIOrchestra.APIGateway.Features.UserManagement;
using AIOrchestra.APIGateway.Resources;
using AIOrchestra.APIGateway.Shared;
using Carter;
using CommonLibrary;
using FluentValidation;
using KafkaLibrary.Interfaces;
using Mapster;
using MediatR;
using System.Net;

namespace AIOrchestra.APIGateway.Features.UserManagement
{
    public static class SetupUser
    {
        private static readonly string HandlerMethod = "SetupUserAsync";
        public class Command : BaseRequest, IRequest<BaseResponse>
        {
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Nickname { get; set; } = string.Empty;
            public int Age { get; set; } = -1;
            public int Danceability { get; set; }
            public int Energy { get; set; }
            public int Loudness { get; set; }
            public int Speechiness { get; set; }
            public int Instrumentalness { get; set; }
            public int Liveness { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Name is required.");

                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Invalid email format.");

                RuleFor(x => x.Nickname)
                    .NotEmpty().WithMessage("Nickname is required.");

                RuleFor(x => x.Age)
                    .NotNull().WithMessage("Age is required.");

                RuleFor(x => x.Danceability)
                    .NotNull().WithMessage("Danceability is required.");

                RuleFor(x => x.Energy)
                    .NotNull().WithMessage("Energy is required.");

                RuleFor(x => x.Loudness)
                    .NotNull().WithMessage("Loudness is required.");

                RuleFor(x => x.Speechiness)
                    .NotNull().WithMessage("Speechiness is required.");

                RuleFor(x => x.Instrumentalness)
                    .NotNull().WithMessage("Instrumentalness is required.");

                RuleFor(x => x.Liveness)
                    .NotNull().WithMessage("Liveness is required.");
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

//Path: AIOrchestra.APIGateway/Features/UserManagement/UpdateUser.cs
public class SetupUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        string endpoint = ServiceEndpoints.UserManagement_Setup;
        string endpointPrefix = ServiceEndpoints.EndpointPrefix;
        app.MapPost(endpointPrefix + endpoint, async (SetupUserReq request, ISender sender) =>
        {
            request.TargetTopic = Topics.UserManagement;
            var command = request.Adapt<SetupUser.Command>();
            command.Value = new
            {
                request.Name,
                request.Email,
                request.Nickname,
                request.Age,
                request.Danceability,
                request.Energy,
                request.Loudness,
                request.Speechiness,
                request.Instrumentalness,
                request.Liveness
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