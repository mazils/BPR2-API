using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudyIt.MongoDB.Models;

public class Company
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _id { get; set; }

    public string email { get; set; } = null!;
    public string name { get; set; } = null!;
    public int cvr { get; set; } 
    public string? location { get; set; }
    public string? phoneNumber { get; set; }
    public string? description { get; set; }
    public byte[]? logo { get; set; }
}