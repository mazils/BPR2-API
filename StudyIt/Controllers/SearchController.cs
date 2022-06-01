using Microsoft.AspNetCore.Mvc;
using StudyIt.MongoDB.Services;

namespace StudyIt.Controllers;

[ApiController]
[Route("[Controller]")]
public class SearchController : Controller
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService) => _searchService = searchService;

    [HttpGet]
    [Route("search")]
    public async Task<IActionResult> Search(string searchTerm, string? location, string? type, int page)
    {
        var search = await _searchService.GetSearchResult(searchTerm, location, type, page);

        if (search == null)
        {
            return NotFound();
        }

        return Ok(search);
    }
}