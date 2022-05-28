using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudyIt.MongoDB.Models;

public class Post
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _id { get; set; }

    public string title { get; set; } = null!;
    public string description { get; set; } = null!;
    public string location { get; set; } = null!;
    public List<string>? competences { get; set; }
    public string? type { get; set; }
    public DateTime deadline { get; set; }
    public string companyId { get; set; }

    public List<Application>? application { get; set; }
}