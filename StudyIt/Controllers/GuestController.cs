using Microsoft.AspNetCore.Mvc;
using StudyIt.MongoDB.Models;
using StudyIt.MongoDB.Services;

namespace StudyIt.Controllers;

[ApiController]
[Route("[Controller]")]
public class GuestController : Controller
{
    private readonly GuestService _guestService;

    public GuestController(GuestService guestService) => _guestService = guestService;

    [HttpGet]
    [Route("search")]
    public async Task<ActionResult<List<Post>>> GuestSearch(string searchTerm)
    {
        var search = await _guestService.GetSearchedPost(searchTerm);

        if(search == null) {
            return NotFound();
        }
        Console.WriteLine(search);
        return search;
    }
}