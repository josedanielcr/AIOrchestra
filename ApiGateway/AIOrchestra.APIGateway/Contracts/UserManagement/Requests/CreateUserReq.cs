using CommonLibrary;

namespace AIOrchestra.APIGateway.Contracts.UserManagement.Requests
{
    public class CreateUserReq : BaseRequest
    {
        public required string Email { get; set; }
        public required string Nickname { get; set; }
        public required string Picture { get; set; }
    }
}
