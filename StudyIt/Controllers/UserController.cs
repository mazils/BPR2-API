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
    private  Firebase firebase;
    public UserController(UserService userService) {
        _userService = userService;
        firebase = Firebase.GetInstance();
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> PostUser(User newUser)
    {
        await _userService.CreateUser(newUser);
        
        Console.WriteLine(newUser);

        return CreatedAtAction(nameof(getUserByEmail), new { email = newUser.email }, newUser);
    }

    [HttpGet]
    [Route("getUserByEmail")]
    public async Task<ActionResult<User>> getUserByEmail(string email)
    {
         if (Request.Headers.TryGetValue("token", out var value))
        {
            string  token = value;
            if (firebase.varify(token).Result)
            {
                var user = await _userService.GetUserByEmail(email);
                Console.WriteLine(user.name);
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
    [Route("getUserById")]
    public async Task<ActionResult<User>> getUserById(string _id)
    {
         if (Request.Headers.TryGetValue("token", out var value))
        {
            string  token = value;
            if (firebase.varify(token).Result)
            {
                var user = await _userService.GetUserbyId(_id);
                Console.WriteLine(user.name);
                if (user == null)
                {
                    return NotFound();
                }
                return user;
            }
        }
        return Unauthorized();
    }

[HttpPut]
    [Route("updateUser")]
    public async Task<ActionResult<User>> updateUser(User updatedUser)
    {
         if (Request.Headers.TryGetValue("token", out var value))
        {
            string  token = value;
            if (firebase.varify(token).Result)
            {
                var result = await _userService.updateUser(updatedUser);
                Console.WriteLine("as MatchedCount: "+ result.MatchedCount);
                 if (result.MatchedCount == 0)
                 {
                     return NotFound();
                 }
                 return Ok();
            }
        }
            return Unauthorized();
        
    }

    //just a template
    [HttpGet]
    [Route("tokenTest")]
    public async Task<IActionResult> testToken()
    {
        if (Request.Headers.TryGetValue("token", out var value))
        {
            string  token = value;
            if (firebase.varify(token).Result)
            {
                return Ok();
            }
        }
        return Unauthorized();
    }
}