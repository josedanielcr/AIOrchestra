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
    public static class CreateUser
    {
        public class Command : BaseRequest, IRequest<BaseResponse>
        {
            public required string Name { get; set; }
            public required string Email { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
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
                BaseResponse response = await producer.ProduceAsync(request.TargetTopic, request.OperationId, request);
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
                        null));
                }
                return (false, null);
            }
        }

    }
}

// Path: AIOrchestra.APIGateway/Features/UserManagement/CreateUserEndpoint.cs
public class CreateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        string endpoint = ServiceEndpoints.UserManagement;
        string endpointPrefix = ServiceEndpoints.EndpointPrefix;
        app.MapPost(endpointPrefix + endpoint, async (CreateUserReq request, ISender sender) =>
        {
            request.TargetTopic = Topics.UserManagement;
            var command = request.Adapt<CreateUser.Command>();
            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                return Results.BadRequest(result);
            }
            return Results.Ok(result);
        }).RequireAuthorization();
    }
}