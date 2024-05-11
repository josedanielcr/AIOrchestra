namespace ApiGateway.Contracts.UserManagement.Requests
{
    public class CreateUserReq
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
}
