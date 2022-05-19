using MongoDB.Bson;

namespace StudyIt.MongoDB.Models;

public class SearchResult
{
    public int Docs { get; set; }
    
    public int TotalPages { get; set; }

    public int CurrentPage { get; set; }

    public int PageSize { get; set; }

    public IEnumerable<Post> Data { get; set; }
}