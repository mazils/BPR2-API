using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudyIt.MongoDB.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _id { get; set; }

    public string email { get; set; } = null!;
    public string name { get; set; } = null!;
    public string education { get; set; } = null!;
    public string? phoneNumber { get; set; }
    public byte[]? profilePicture { get; set; }
    public byte[]? personalityProfile { get; set; }
    public List<string>? competences { get; set; }
    public List<string>? interests { get; set; }
}