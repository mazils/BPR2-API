using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using StudyIt.helperClasses;
using StudyIt.MongoDB.Models;
using StudyIt.MongoDB.Services;

namespace StudyIt.Controllers;

[ApiController]
[Route("[Controller]")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private IFirebaseAuthentication _firebaseAuthentication;

    public UserController(IUserService userService)
    {
        _userService = userService;
        _firebaseAuthentication = FirebaseAuthentication.GetInstance();
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(User newUser)
    {
        bool isCreated = await _userService.Register(newUser);
        return isCreated?Ok():Conflict();
    }

    [HttpGet]
    [Route("getByEmail")]
    public async Task<ActionResult<User>> GetUserByEmail(string email)
    {
        if (Request.Headers.TryGetValue("token", out var value))
        {
            string token = value;
            if (_firebaseAuthentication.Verify(token).Result)
            {
                var user = await _userService.GetByEmail(email);
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
            if (_firebaseAuthentication.Verify(token).Result)
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
            if (_firebaseAuthentication.Verify(token).Result)
            {
                // Converts the Profile Picture and Personality Profile into a byte[]
                var userDto = DataTransferObject.ConvertBase64ToBinaryUser(updatedUser);
                var result = await _userService.UpdateUser(userDto);
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
            if (_firebaseAuthentication.Verify(token).Result)
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
    public async Task<IActionResult> UpdatePersonalityProfile(string _id)
    {
        if (Request.Headers.TryGetValue("token", out var value))
        {
            string token = value;
            if (_firebaseAuthentication.Verify(token).Result)
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
    public async Task<ActionResult<byte[]>> GetPersonalityProfile(string _id)
    {
        if (Request.Headers.TryGetValue("token", out var value))
        {
            string token = value;
            if (_firebaseAuthentication.Verify(token).Result)
            {
                var personalityProfile = await _userService.GetPersonalityProfile(_id);
                if (personalityProfile == null)
                {
                    return NotFound();
                }

                return personalityProfile;
            }
        }

        return Unauthorized();
    }

    [HttpGet]
    [Route("getProfilePicture")]
    public async Task<ActionResult<string>> GetProfilePicture(string _id)
    {
        if (Request.Headers.TryGetValue("token", out var value))
        {
            string token = value;
            if (_firebaseAuthentication.Verify(token).Result)
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
        }

        return Unauthorized();
    }

}