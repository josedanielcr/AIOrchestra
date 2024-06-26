using CommonLibrary;

namespace AIOrchestra.APIGateway.Contracts.MusicRecommender.Requests
{
    public class GetRecommendationReq : BaseRequest
    {
        public int Danceability { get; set; }
        public int Energy { get; set; }
        public int Loudness { get; set; }
        public int Speechiness { get; set; }
        public int Instrumentalness { get; set; }
        public int Liveness { get; set; }
        public IEnumerable<string> Songs { get; set; } = new List<string>();
    }
}