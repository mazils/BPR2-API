using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using StudyIt.helperClasses;
using StudyIt.MongoDB.Models;
using StudyIt.MongoDB.Services;

namespace StudyIt.Controllers;

[ApiController]
[Route("[Controller]")]
public class CompanyController : Controller
{
    private readonly ICompanyService _companyService;
    private IFirebaseAuthentication _firebaseAuthentication;

    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
        _firebaseAuthentication = FirebaseAuthentication.GetInstance();
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(Company newCompany)
    {
        bool isRegistred = await _companyService.Register(newCompany);
        return isRegistred ? Ok() : Conflict();
    }

    [HttpGet]
    [Route("getByEmail")]
    public async Task<ActionResult<Company>> GetCompanyByEmail(string email)
    {
        if (Request.Headers.TryGetValue("token", out var value))
        {
            string token = value;
            if (_firebaseAuthentication.Verify(token).Result)
            {
                var company = await _companyService.GetByEmail(email);

                if (company == null)
                {
                    return NotFound();
                }

                return company;
            }
        }

        return Unauthorized();
    }

    [HttpGet]
    [Route("getById")]
    public async Task<ActionResult<Company>> GetCompanyById(string _id)
    {
        if (Request.Headers.TryGetValue("token", out var value))
        {
            string token = value;
            if (_firebaseAuthentication.Verify(token).Result)
            {
                var company = await _companyService.GetById(_id);

                if (company == null)
                {
                    return NotFound();
                }

                return company;
            }
        }

        return Unauthorized();
    }

    //updating company
    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> Update(CompanyDTO updatedUser)
    {
        if (Request.Headers.TryGetValue("token", out var value))
        {
            string token = value;
            if (_firebaseAuthentication.Verify(token).Result)
            {
                // Converts the Logo into a byte[]
                var userDto = DataTransferObject.ConvertBase64ToBinaryCompany(updatedUser);
                var result = await _companyService.UpdateCompany(userDto);
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
    [Route("updateLogo")]
    public async Task<IActionResult> UpdateLogo(string _id)
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
                        await _companyService.UpdateLogo(_id, fileBytes);
                    }
                }

                return Ok();
            }
        }

        return Unauthorized();
    }

    [HttpGet]
    [Route("getLogo")]
    public async Task<ActionResult<string>> GetProfilePicture(string _id)
    {
        var logo = await _companyService.GetLogo(_id);
        if (logo == null)
        {
            return NotFound();
        }

        var decodedIntoString = FileConversion.BinToBase64String(logo);

        var imageExtension = FileConversion.AddFileExtension(decodedIntoString);

        return imageExtension.ToJson();
    }
}