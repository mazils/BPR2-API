using System.Collections.Generic;

namespace StudyIt.MongoDB.Models;

public class PostDTO
{
    public string? _id { get; set; }
    
    public string title { get; set; } = null!;
    public string description { get; set; } = null!;
    public string location { get; set; } = null!;
    public List<string>? competences { get; set; }
    public string? type { get; set; }
    public string deadline { get; set; }
    public string companyId { get; set; }
    
    public List<Application>? application { get; set; }
}