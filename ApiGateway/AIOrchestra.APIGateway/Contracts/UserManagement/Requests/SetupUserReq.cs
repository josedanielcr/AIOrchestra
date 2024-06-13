using CommonLibrary;

namespace AIOrchestra.APIGateway.Contracts.UserManagement.Requests
{
    public class SetupUserReq : BaseRequest
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
}
