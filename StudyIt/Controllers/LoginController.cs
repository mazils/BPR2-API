using System.Net;
using Microsoft.AspNetCore.Mvc;
using StudyIt.MongoDB.Models;
using StudyIt.MongoDB.Services;

namespace StudyIt.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : Controller
{
   private readonly LoginService _loginService;

   public LoginController(LoginService loginService) =>
       _loginService = loginService;

   [HttpGet]
   public async Task<ActionResult<Login>> GetLogin(string username)
   {
       var login = await _loginService.GetLogin(username);
       
       Console.WriteLine(login);

       if (login == null)
       {
           return NotFound();
       }

       return login;
   }

   [HttpPost]
   public async Task<IActionResult> LoginCheck(string username, string password)
   {
       var login = await _loginService.CheckLogin(username, password);
   
       if (login == null)
       {
           return NotFound();
       }
   
       return Ok();
   }

   [HttpPost]
   [Route("signup")]
   public async Task<IActionResult> PostTest(Login newLogin)
   {
       await _loginService.CreateLogin(newLogin);
       
       return CreatedAtAction(nameof(GetLogin), new {username = newLogin.username}, newLogin);
   }
}