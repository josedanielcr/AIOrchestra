using AIOrchestra.UserManagementService.Common.Enums;
using MongoDB.Bson;

namespace AIOrchestra.UserManagementService.Requests
{
    public class SetupUserRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public int Age { get; set; } = -1;
        public string Country { get; set; } = string.Empty;
        public Genre Genre { get; set; } = Genre.None;
        public Languages Language { get; set; } = Languages.None;
        public Ethnicity Ethnicity { get; set; } = Ethnicity.None;
    }
}