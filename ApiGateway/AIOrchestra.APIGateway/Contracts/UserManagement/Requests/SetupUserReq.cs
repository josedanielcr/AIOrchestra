using CommonLibrary;

namespace AIOrchestra.APIGateway.Contracts.UserManagement.Requests
{
    public class SetupUserReq : BaseRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public int Age { get; set; } = -1;
        public int Danceability { get; set; }
        public int Energy { get; set; }
        public int Loudness { get; set; }
        public int Speechiness { get; set; }
        public int Instrumentalness { get; set; }
        public int Liveness { get; set; }
    }
}