using CommonLibrary;

namespace AIOrchestra.APIGateway.Contracts.PlaylistService
{
    public class CreatePlaylistReq : BaseRequest
    {
        public string Name { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public List<String> SongIds { get; set; } = new List<String>();
    }
}
