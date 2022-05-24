namespace StudyIt.MongoDB.Models;

public class CompanyDTO
{
    public string? _id { get; set; }

    public string email { get; set; } = null!;
    public string name { get; set; } = null!;
    public int cvr { get; set; } 
    public string? location { get; set; }
    public string? phoneNumber { get; set; }
    public string? description { get; set; }
    public string? logo { get; set; }
}