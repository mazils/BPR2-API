using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudyIt.MongoDB.Models;
using StudyIt.MongoDB.Services;

namespace StudyIt.Controllers;

[ApiController]
[Route("[Controller]")]
public class PostController : Controller
{
    private readonly PostService _postService;
    private  Firebase firebase;
    

    public PostController(PostService postService) {
        _postService = postService;
         firebase = Firebase.GetInstance();
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreatePost(Post post)
    {
         if (Request.Headers.TryGetValue("token", out var value))
        {
            string token = value;
            if (firebase.varify(token).Result)
            {
                await _postService.CreatePost(post);
                return Created("Post/create",post.title);
            }
        }
        return Unauthorized();
    }

    [HttpGet]
    [Route("GetById")]
    public async Task<IActionResult> GetById(string _id)
    {
         if (Request.Headers.TryGetValue("token", out var value))
        {
            string token = value;
            if (firebase.varify(token).Result)
            {
                var allPosts = await _postService.GetAllCompanyPosts(_id);

                if (allPosts == null)
                {
                    return NotFound();
                }

                return Ok(allPosts);
            }
        }
        return Unauthorized();
    }

}