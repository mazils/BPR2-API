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
    //get post by post id
    [HttpGet]
    [Route("GetById")]
    public async Task<ActionResult<Post>> GetById(string _id)
    {
         if (Request.Headers.TryGetValue("token", out var value))
        {
            string token = value;
            if (firebase.varify(token).Result)
            {
                return await _postService.GetPostById(_id);
            }
        }
        return Unauthorized();
    }

    //get all posts by company id
    [HttpGet]
    [Route("GetAllById")]
    public async Task<IActionResult> GetAllById(string _id)
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
    //apply with post id and project group
    [HttpPut]
    [Route("Apply")]
    public async Task<IActionResult> ApplyToPost(string postId,Application applicationFromUser)
    {
        if (Request.Headers.TryGetValue("token", out var value))
        {
            string token = value;
            if (firebase.varify(token).Result)
            {
                var post = await _postService.GetPostById(postId);
                if (post.application != null)
                {
                    foreach (var application in post.application)
                    {
                        if (application.applicants.Contains(applicationFromUser.applicants.ElementAt(0)))
                        {
                            return Conflict("Already applied");
                        }
                    }
                }
                var result = await _postService.ApplyToPost(postId,applicationFromUser);
                if (result.MatchedCount == 0)
                {
                    return NotFound();
                }
                return Ok("Successfully applied");
            }
        }
        return Unauthorized();
    }

}