using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
    private readonly CompanyService _companyService;
    private Firebase firebase;
    
    public CompanyController(CompanyService companyService)
    {
        _companyService = companyService;
        firebase = Firebase.GetInstance();
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(Company newCompany)
    {
        await _companyService.Register(newCompany);

        Console.WriteLine(newCompany);

        return CreatedAtAction(nameof(GetCompanyByEmail), new { email = newCompany.email }, newCompany);
    }

    [HttpGet]
    [Route("getByEmail")]
    public async Task<ActionResult<Company>> GetCompanyByEmail(string email)
    {
        if (Request.Headers.TryGetValue("token", out var value))
        {
            string token = value;
            if (firebase.varify(token).Result)
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
            if (firebase.varify(token).Result)
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
            if (firebase.varify(token).Result)
            {
                // Converts the Logo into a byte[]
                var userDto = DataTransferObject.ConvertBase64ToBinaryCompany(updatedUser);
                 await _companyService.UpdateCompany(userDto);
                // Console.WriteLine("as MatchedCount: " + result.MatchedCount);
                // if (result.MatchedCount == 0)
                // {
                //     return NotFound();
                // }

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