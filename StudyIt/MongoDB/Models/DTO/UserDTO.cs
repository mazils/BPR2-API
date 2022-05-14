namespace StudyIt.MongoDB.Models;

public class UserDTO
{
    public string? _id { get; set; }

    public string email { get; set; } = null!;
    public string name { get; set; } = null!;
    public string education { get; set; } = null!;
    public string? phoneNumber { get; set; }
    public string? profilePicture { get; set; }
    public string? personalityProfile { get; set; }
    public List<string>? competences { get; set; }
    public List<string>? interests { get; set; }
}