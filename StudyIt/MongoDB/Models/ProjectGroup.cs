using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudyIt.MongoDB.Models;

public class ProjectGroup
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _id { get; set; }

    public List<string>? userEmails { get; set; } = null!;
    public List<string>? applicationIds { get; set; } = null!;
    public List<string>? competences { get; set; } = null!;
}