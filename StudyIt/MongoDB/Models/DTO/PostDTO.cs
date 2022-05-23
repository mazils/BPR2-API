namespace StudyIt.MongoDB.Models;

public class PostDTO
{
    public string? _id { get; set; }
    
    public string title { get; set; } = null!;
    public string description { get; set; } = null!;
    public string location { get; set; } = null!;
    public List<string>? competences { get; set; }
    public string? type { get; set; }
    public string deadline { get; set; } // Cannot be set such its not allowed to be null, has to be done in front-end
    public string companyId { get; set; }
    
    public Application[]? application { get; set; }
}