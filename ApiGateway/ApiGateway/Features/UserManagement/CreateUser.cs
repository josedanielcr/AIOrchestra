using ApiGateway.Common;
using ApiGateway.Contracts.UserManagement.Requests;
using ApiGateway.Features.UserManagement;
using Carter;
using Mapster;
using MediatR;

namespace ApiGateway.Features.UserManagement
{
    public static class CreateUser
    {

        public class Command : IRequest<BaseResponse>
        {
            public required string Name { get; set; }
            public required string Email { get; set; }
        }

        internal sealed class Handler : IRequestHandler<Command, BaseResponse>
        {
            public async Task<BaseResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                // Add your logic here
                return new BaseResponse();
            }
        }
    }
}

// Path: ApiGateway/Features/UserManagement/UpdateUser.cs
public class CreateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {   
        app.MapPost("/api/user", async (CreateUserReq request, ISender sender) =>
        {
            var command = request.Adapt<CreateUser.Command>();
            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Message);
            }
            return Results.Ok(result);
        }).RequireAuthorization();
    }
}