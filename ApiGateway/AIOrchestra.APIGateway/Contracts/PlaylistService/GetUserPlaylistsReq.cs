using CommonLibrary;

namespace AIOrchestra.APIGateway.Contracts.PlaylistService
{
    public class GetUserPlaylistsReq : BaseRequest
    {
        public string UserId { get; set; } = String.Empty;
    }
}
