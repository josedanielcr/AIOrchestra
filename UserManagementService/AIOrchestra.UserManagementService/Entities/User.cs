using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AIOrchestra.UserManagementService.Entities
{

    public class User
    {
        [BsonId]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; } = -1;
    }
}