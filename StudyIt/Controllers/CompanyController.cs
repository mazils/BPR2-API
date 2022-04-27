using Microsoft.AspNetCore.Mvc;
using StudyIt.MongoDB.Models;
using StudyIt.MongoDB.Services;

namespace StudyIt.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class CompanyController : Controller
{
    private readonly CompanyService _companyService;

    public CompanyController(CompanyService companyService) =>
        _companyService = companyService;

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> PostCompany(Company newCompany)
    {
        await _companyService.CreateCompany(newCompany);
        
        Console.WriteLine(newCompany);

        return CreatedAtAction(nameof(GetCompany), new { email = newCompany.email }, newCompany);
    }

    [HttpGet]
    public async Task<ActionResult<Company>> GetCompany(string email)
    {
        var company = await _companyService.GetCompany(email);
        
        Console.WriteLine(company);

        if (company == null)
        {
            return NotFound();
        }

        return company;
    }
}