using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using AIOrchestra.UserManagementService.Common.Enums;

namespace AIOrchestra.UserManagementService.Common.Entities
{

    public class User
    {
        [BsonId]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; } = -1;
        public string Country { get; set; } = string.Empty;
        public Genre Genre { get; set; } = Genre.None;
        public Languages Language { get; set; } = Languages.None;
        public Ethnicity Ethnicity { get; set; } = Ethnicity.None;
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public string RequesterId { get; set; } = string.Empty;
    }
}