using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudyIt.MongoDB.Models;
using StudyIt.MongoDB.Services;

namespace StudyIt.Controllers;

[ApiController]
[Route("[Controller]")]
public class CompanyController : Controller
{
    private readonly CompanyService _companyService;
    private  Firebase firebase;
    

    public CompanyController(CompanyService companyService) {
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
            string  token = value;
            if (firebase.varify(token).Result)
            {
                var company = await _companyService.GetCompanyByEmail(email);

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
            string  token = value;
            if (firebase.varify(token).Result)
            {
                var company = await _companyService.GetCompanyById(_id);
                
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
    public async Task<ActionResult<Company>> Update(Company company)
    {
         if (Request.Headers.TryGetValue("token", out var value))
        {
            string  token = value;
            if (firebase.varify(token).Result)
            {
                var result = await _companyService.Update(company);
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
}