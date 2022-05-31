using Microsoft.AspNetCore.Mvc;
using StudyIt.MongoDB.Models;
using StudyIt.MongoDB.Services;

namespace StudyIt.Controllers;

[ApiController]
[Route("[Controller]")]
public class ProjectGroupController : Controller
{
    private readonly IProjectGroupService _projectGroupService;
    private readonly IUserService _userService;
    private IFirebaseAuthentication _firebaseAuthentication;

    public ProjectGroupController(IUserService userService, IProjectGroupService projectGroupService)
    {
        _projectGroupService = projectGroupService;
        _userService = userService;
        _firebaseAuthentication = FirebaseAuthentication.GetInstance();
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateGroup(ProjectGroup projectGroup)
    {
        if (Request.Headers.TryGetValue("token", out var value))
        {
            string token = value;
            if (_firebaseAuthentication.Verify(token).Result)
            {
                projectGroup.competences = new List<string>();
                projectGroup.applicationIds = new List<string>();
                List<string> allCompetences = new List<string>();
                foreach (var email in projectGroup.userEmails)
                {
                    var group = await _projectGroupService.GetGroup(email);
                    var user = await _userService.GetByEmail(email);
                    if (user == null)
                    {
                        return NotFound();
                    }

                    if (group != null)
                    {
                        return Conflict();
                    }

                    if (user.competences != null)
                    {
                        allCompetences.AddRange(user.competences);
                    }
                }

                projectGroup.competences = allCompetences.Distinct().ToList();
                bool isCreated = await _projectGroupService.CreateGroup(projectGroup);
                if (isCreated)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        return Unauthorized();
    }

    [HttpGet]
    [Route("getProjectGroup")]
    public async Task<ActionResult<ProjectGroup>> GetGroup(string email)
    {
        if (Request.Headers.TryGetValue("token", out var value))
        {
            string token = value;
            if (_firebaseAuthentication.Verify(token).Result)
            {
                var group = await _projectGroupService.GetGroup(email);
                if (group == null)
                {
                    return NotFound();
                }

                return group;
            }
        }

        return Unauthorized();
    }
}