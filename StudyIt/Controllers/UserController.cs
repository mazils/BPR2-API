using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudyIt;
using StudyIt.MongoDB.Models;
using StudyIt.MongoDB.Services;

namespace StudyIt.Controllers;


[ApiController]
[Route("[Controller]")]
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
    public async Task<IActionResult> Register(User newUser)
    {
        await _userService.Register(newUser);
        
        Console.WriteLine(newUser);

        return CreatedAtAction(nameof(GetUserByEmail), new { email = newUser.email }, newUser);
    }

    [HttpGet]
    [Route("getByEmail")]
    public async Task<ActionResult<User>> GetUserByEmail(string email)
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
    [Route("getById")]
    public async Task<ActionResult<User>> GetUserById(string _id)
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
    [Route("update")]
    public async Task<ActionResult<User>> Update(User updatedUser)
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
    [HttpPut]
    [Route("updatePicture")]
    public async Task<IActionResult> UpdatePicture(string _id)
    {
        if (Request.Headers.TryGetValue("token", out var value))
        {
            string  token = value;
            if (firebase.varify(token).Result)
            {
                var formCollection = await Request.ReadFormAsync();
                if(formCollection.Files.Count == 0)
                {
                     return NoContent();
                }
                var file = formCollection.Files.FirstOrDefault();
                 if (file.Length > 0)
                 {
                    using (var ms = new MemoryStream())
                    {
                      file.CopyTo(ms);
                      byte[] fileBytes = ms.ToArray();
                      await _userService.UpdatePicture(_id,fileBytes);
                    }
                }
                return Ok($"Received file {Path.GetFileName(file.FileName)} ");
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