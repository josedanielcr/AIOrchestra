using CommonLibrary;

namespace AIOrchestra.APIGateway.Contracts.PlaylistService.Requests
{
    public class GetUserPlaylistsReq : BaseRequest
    {
        public string UserId { get; set; } = string.Empty;
    }
}
