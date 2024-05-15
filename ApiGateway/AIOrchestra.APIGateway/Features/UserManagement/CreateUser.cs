using AIOrchestra.APIGateway.Common.Entities;
using AIOrchestra.APIGateway.Common.Enums;
using AIOrchestra.APIGateway.Contracts.UserManagement.Requests;
using AIOrchestra.APIGateway.Features.UserManagement;
using AIOrchestra.APIGateway.Kafka.Producers;
using AIOrchestra.APIGateway.Resources;
using Carter;
using Mapster;
using MediatR;

namespace AIOrchestra.APIGateway.Features.UserManagement
{
    public static class CreateUser
    {
        public class Command : BaseRequest, IRequest<BaseResponse>
        {
            public required string Name { get; set; }
            public required string Email { get; set; }
        }

        internal sealed class Handler : IRequestHandler<Command, BaseResponse>
        {
            private readonly IProducerService producer;

            public Handler(IProducerService producer)
            {
                this.producer = producer;
            }

            public async Task<BaseResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                BaseResponse response = await producer.ProduceAsync(request.TargetTopic, request.OperationId, request);
                return response;
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