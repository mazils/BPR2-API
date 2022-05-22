using Microsoft.AspNetCore.Mvc;
using StudyIt.MongoDB.Models;
using StudyIt.MongoDB.Services;

namespace StudyIt.Controllers;

[ApiController]
[Route("[Controller]")]
public class ProjectGroupController : Controller
{
    private readonly ProjectGroupService _projectGroupService;
    private readonly UserService _userService;
    private Firebase firebase;


    public ProjectGroupController(UserService userService, ProjectGroupService projectGroupService)
    {
        _projectGroupService = projectGroupService;
        _userService = userService;
        firebase = Firebase.GetInstance();
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateGroup(ProjectGroup projectGroup)
    {
        if (Request.Headers.TryGetValue("token", out var value))
        {
            string token = value;
            if (firebase.Verify(token).Result)
            {
                projectGroup.competences = new List<string>();
                projectGroup.applicationIds = new List<string>();
                List<string> allCompetences = new List<string>();
                foreach (var email in projectGroup.userEmails)
                {
                    var user = await _userService.GetByEmail(email);
                    if (user == null)
                    {
                        return NotFound("user not found: " + email);
                    }

                    if (user.competences != null)
                    {
                        allCompetences.AddRange(user.competences);
                    }
                }

                projectGroup.competences = allCompetences.Distinct().ToList();
                string error = await _projectGroupService.CreateGroup(projectGroup);
                if (error == string.Empty)
                {
                    return Ok();
                }

                return Conflict(error);
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
            if (firebase.Verify(token).Result)
            {
                var group = await _projectGroupService.GetGroup(email);
                return group;
            }
        }

        return Unauthorized();
    }
}