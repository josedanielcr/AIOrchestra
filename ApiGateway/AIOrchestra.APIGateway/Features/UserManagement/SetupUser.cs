using AIOrchestra.APIGateway.Contracts.UserManagement.Requests;
using AIOrchestra.APIGateway.Features.UserManagement;
using AIOrchestra.APIGateway.Resources;
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
            public string Country { get; set; } = string.Empty;
            public string Genre { get; set; } = string.Empty;
            public string Language { get; set; } = string.Empty;
            public string Ethnicity { get; set; } = string.Empty;
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

                RuleFor(x => x.Country)
                    .NotEmpty().WithMessage("Country is required.");

                RuleFor(x => x.Language)
                    .NotEmpty().WithMessage("Language is required.");
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
                var (hasError, baseResponse) = ValidateRequest(request);

                if (hasError)
                {
                    return baseResponse!;
                }

                request.HandlerMethod = HandlerMethod;
                var response = await producer.ProduceAsync(request.TargetTopic, request.OperationId, request);
                return response;
            }

            private (bool hasErorr, BaseResponse? baseResponse) ValidateRequest(Command request)
            {
                var validationResult = SharedLibrary.ValidationHelper.ValidateRequest(validator, request);
                if (!string.IsNullOrEmpty(validationResult))
                {
                    return (true, SharedLibrary.GenerateApplicationResponse.GenerateResponse(
                        request.OperationId,
                        request.ApiVersion,
                        false,
                        HttpStatusCode.BadRequest,
                        ApplicationErrors.InvalidRequest_NullFields,
                        validationResult,
                        null,
                        null,
                        request.TargetTopic,
                        null,
                        request.Value,
                        request.HandlerMethod));
                }
                return (false, null);
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
                request.Country,
                request.Genre,
                request.Language,
                request.Ethnicity
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