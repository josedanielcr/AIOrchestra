using CommonLibrary;

namespace AIOrchestra.APIGateway.Contracts.PlaylistService.Requests
{
    public class DeletePlaylistReq : BaseRequest
    {
        public string PlaylistId { get; set; } = string.Empty;
    }
}
