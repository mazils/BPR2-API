using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using StudyIt;
using StudyIt.helperClasses;
using StudyIt.MongoDB.Models;
using StudyIt.MongoDB.Services;


namespace StudyIt.Controllers;

[ApiController]
[Route("[Controller]")]
public class UserController : Controller
{
    private readonly UserService _userService;
    private Firebase firebase;

    public UserController(UserService userService)
    {
        _userService = userService;
        firebase = Firebase.GetInstance();
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(User newUser)
    {   
    
        await _userService.Register(newUser);
        return Ok();
    }

    [HttpGet]
    [Route("getByEmail")]
    public async Task<ActionResult<User>> GetUserByEmail(string email)
    {
        if (Request.Headers.TryGetValue("token", out var value))
        {
            string token = value;
            if (firebase.varify(token).Result)
            {
                var user = await _userService.GetByEmail(email);
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
            string token = value;
            if (firebase.varify(token).Result)
            {
                var user = await _userService.GetById(_id);
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
    public async Task<IActionResult> Update(UserDTO updatedUser)
    {
        if (Request.Headers.TryGetValue("token", out var value))
        {
            string token = value;
            if (firebase.varify(token).Result)
            {
                // Converts the Profile Picture and Personality Profile into a byte[]
                var userDto = DataTransferObject.ConvertBase64ToBinaryUser(updatedUser);
                var result = await _userService.UpdateUser(userDto);
                Console.WriteLine("as MatchedCount: " + result.MatchedCount);
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
            string token = value;
            if (firebase.varify(token).Result)
            {
                var formCollection = await Request.ReadFormAsync();
                if (formCollection.Files.Count == 0)
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
                        await _userService.UpdatePicture(_id, fileBytes);
                    }
                }

                return Ok();
            }
        }

        return Unauthorized();
    }

    [HttpPut]
    [Route("updatePersonalityProfile")]
    public async Task<IActionResult> UpdatepersonalityProfile(string _id)
    {
        if (Request.Headers.TryGetValue("token", out var value))
        {
            string token = value;
            if (firebase.varify(token).Result)
            {
                var formCollection = await Request.ReadFormAsync();
                if (formCollection.Files.Count == 0)
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
                        await _userService.UpdatePersonalityProfile(_id, fileBytes);
                    }
                }

                return Ok();
            }
        }

        return Unauthorized();
    }

    [HttpGet]
    [Route("getPersonalityProfile")]
    public async Task<ActionResult<byte[]>> GetpersonallityProfile(string _id)
    {
        var personalityProfile = await _userService.GetPersonalityProfile(_id);
        if (personalityProfile == null)
        {
            return NotFound();
        }

        return personalityProfile;
    }

    [HttpGet]
    [Route("getProfilePicture")]
    public async Task<ActionResult<string>> GetProfilePicture(string _id)
    {
        var picture = await _userService.GetProfilePicture(_id);
        if (picture == null)
        {
            return NotFound();
        }

        var decodedIntoString = FileConversion.BinToBase64String(picture);

        var imageExtension = FileConversion.AddFileExtension(decodedIntoString);

        return imageExtension.ToJson();
    }

    //just a template
    [HttpGet]
    [Route("tokenTest")]
    public async Task<IActionResult> testToken()
    {
        if (Request.Headers.TryGetValue("token", out var value))
        {
            string token = value;
            if (firebase.varify(token).Result)
            {
                return Ok();
            }
        }

        return Unauthorized();
    }
}