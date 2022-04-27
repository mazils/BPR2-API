using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudyIt.MongoDB.Models;

public class Notification
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _id { get; set; }

    public bool visibility { get; set; }
    
    public FormatApplicationNotification? applicationNotification { get; set; }
    public FormatPostNotification? postNotification { get; set; }
}