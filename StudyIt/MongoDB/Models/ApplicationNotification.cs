using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudyIt.MongoDB.Models;

public class ApplicationNotification
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _id { get; set; }

    public string applicationId { get; set; } = null!;
    public string status { get; set; } = null!;
}