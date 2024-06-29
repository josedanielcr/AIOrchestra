using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PlaylistService.Common.Entities
{
    public class Playlist
    {
        [BsonId]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string Name { get; set; } = String.Empty;
        public string UserId { get; set; } = String.Empty;
        public List<string> SongIds { get; set; } = new List<string>();
    }
}
