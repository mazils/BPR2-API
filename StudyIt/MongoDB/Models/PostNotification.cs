using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudyIt.MongoDB.Models;

public class PostNotification
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _id { get; set; }

    public string postId { get; set; } = null!;
}