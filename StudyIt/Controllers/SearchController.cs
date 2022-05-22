using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using StudyIt.MongoDB.Models;
using StudyIt.MongoDB.Services;

namespace StudyIt.Controllers;

[ApiController]
[Route("[Controller]")]
public class SearchController : Controller
{
    private readonly SearchService _guestService;

    public SearchController(SearchService guestService) => _guestService = guestService;

    [HttpGet]
    [Route("search")]
    public async Task<IActionResult> GuestSearch(string searchTerm, string? location, string? type, int page)
    {
        var search = await _guestService.GetSearchResult(searchTerm, location, type, page);

        if(search == null) {
            return NotFound();
        }
        return Ok(search);
    }
}