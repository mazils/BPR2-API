using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudyIt.MongoDB.Models;

public class Application
{
    [BsonId]
    public ObjectId? _id { get; set; }

    public List<string> applicants { get; set; } = null!;
    public string? status { get; set; }
}