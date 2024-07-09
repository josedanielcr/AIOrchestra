using CommonLibrary;

namespace AIOrchestra.APIGateway.Contracts.MusicRecommender.Requests
{
    public class GetTrackByIdReq : BaseRequest
    {
        public List<string> TrackIds { get; set; } = new List<string>();
    }
}
