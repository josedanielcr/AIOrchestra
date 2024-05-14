using AIOrchestra.APIGateway.Common.Entities;
using AIOrchestra.APIGateway.Common.Enums;

namespace AIOrchestra.APIGateway.Contracts.UserManagement.Requests
{
    public class CreateUserReq : BaseRequest
    {
        public required string Email { get; set; }
        public required string Name { get; set; }
    }
}
