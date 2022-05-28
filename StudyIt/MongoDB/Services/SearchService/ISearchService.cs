using StudyIt.MongoDB.Models;

namespace StudyIt.MongoDB.Services;

public interface ISearchService
{
    public Task<SearchResult> GetSearchResult(string searchTerm, string location, string type, int page);
}