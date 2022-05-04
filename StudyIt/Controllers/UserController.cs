using Microsoft.AspNetCore.Mvc;
using StudyIt;
using StudyIt.MongoDB.Models;
using StudyIt.MongoDB.Services;

namespace StudyIt.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class UserController : Controller
{
    private readonly UserService _userService;

    public UserController(UserService userService) =>
        _userService = userService;

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> PostUser(User newUser)
    {
        await _userService.CreateUser(newUser);
        
        Console.WriteLine(newUser);

        return CreatedAtAction(nameof(GetUser), new { email = newUser.email }, newUser);
    }

    [HttpGet]
    [Route("getUser")]
    public async Task<ActionResult<User>> GetUser(string email)
    {
         if (Request.Headers.TryGetValue("token", out var value))
        {
            var firebase = Firebase.GetInstance();
            string  token = value;
            if (firebase.varify(token).Result)
            {
                var user = await _userService.GetUser(email);
                Console.WriteLine(user);
                if (user == null)
                {
                    return NotFound();
                }
                return user;
            }
        }
        return Unauthorized();
    }
    
    [HttpGet]
    [Route("tokenTest")]
    public async Task<IActionResult> testToken()
    {
        if (Request.Headers.TryGetValue("token", out var value))
        {
            var firebase = Firebase.GetInstance();
            string  token = value;
            if (firebase.varify(token).Result)
            {
                return Ok();
            }
        }
        return Unauthorized();
    }
}