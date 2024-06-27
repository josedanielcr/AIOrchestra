using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AIOrchestra.UserManagementService.Common.Entities
{

    public class User
    {
        [BsonId]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public string Picture { get; set; } = string.Empty;
        public int Age { get; set; } = -1;
        public int? Danceability { get; set; }
        public int? Energy { get; set; }
        public int? Loudness { get; set; }
        public int? Speechiness { get; set; }
        public int? Instrumentalness { get; set; }
        public int? Liveness { get; set; }
        public string RequesterId { get; set; } = string.Empty;
        public bool IsProfileCompleted { get; set; } = false;
    }
}