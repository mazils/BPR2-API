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
                await _postService.createPost(post);
                return Created("Post/create",post);
            }
        }
        return Unauthorized();
      
       
    }

}