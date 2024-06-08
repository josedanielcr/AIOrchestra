using CommonLibrary;

namespace AIOrchestra.UserManagementService.Requests
{
    public class CreateUserRequest
    {
        public string Email { get; set; }
        public string Nickname { get; set; }
        public string Picture { get; set; }
    }
}
