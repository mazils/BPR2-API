using Microsoft.AspNetCore.Mvc;
using StudyIt.helperClasses;
using StudyIt.MongoDB.Models;
using StudyIt.MongoDB.Services;

namespace StudyIt.Controllers;

[ApiController]
[Route("[Controller]")]
public class PostController : Controller
{
    private readonly IPostService _postService;
    private IFirebaseAuthentication _firebaseAuthentication;


    public PostController(IPostService postService)
    {
        _postService = postService;
        _firebaseAuthentication = FirebaseAuthentication.GetInstance();
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreatePost(Post post)
    {
        if (Request.Headers.TryGetValue("token", out var value))
        {
            string token = value;
            if (_firebaseAuthentication.Verify(token).Result)
            {
                bool isCreated = await _postService.CreatePost(post);
                return isCreated?Created("Post/create", post.title):Conflict();
            }
        }

        return Unauthorized();
    }

    [HttpGet]
    [Route("GetById")]
    public async Task<ActionResult<Post>> GetById(string _id)
    {
        if (Request.Headers.TryGetValue("token", out var value))
        {
            string token = value;
            if (_firebaseAuthentication.Verify(token).Result)
            {
                return await _postService.GetPostById(_id);
            }
        }

        return Unauthorized();
    }

    //get post by post id
    [HttpGet]
    [Route("GetByCompanyId")]
    public async Task<ActionResult<Post>> GetByCompanyId(string _id)
    {
        if (Request.Headers.TryGetValue("token", out var value))
        {
            string token = value;
            if (_firebaseAuthentication.Verify(token).Result)
            {
                return await _postService.GetPostByCompanyId(_id);
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
            if (_firebaseAuthentication.Verify(token).Result)
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
    public async Task<IActionResult> ApplyToPost(string postId, Application applicationFromUser)
    {
        if (Request.Headers.TryGetValue("token", out var value))
        {
            string token = value;
            if (_firebaseAuthentication.Verify(token).Result)
            {
                var post = await _postService.GetPostById(postId);
                if (post == null)
                {
                    return NotFound();
                }
                
                if (post.application != null)
                {
                    foreach (var application in post.application)
                    {
                        if (application.applicants.Contains(applicationFromUser.applicants.ElementAt(0)))
                        {
                            return Conflict();
                        }
                    }
                }
               
                var result = await _postService.ApplyToPost(postId, applicationFromUser);
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
    [Route("update")]
    public async Task<IActionResult> Update(PostDTO updatedPost)
    {
        if (Request.Headers.TryGetValue("token", out var value))
        {
            string token = value;
            if (_firebaseAuthentication.Verify(token).Result)
            {
                var postDto = DataTransferObject.ConvertStringToDateTimePost(updatedPost);
                var result = await _postService.UpdatePost(postDto);
                if (result.MatchedCount == 0)
                {
                    return NotFound();
                }

                return Ok();
            }
        }

        return Unauthorized();
    }

    [HttpGet]
    [Route("getThreeNearestDeadlineByType")]
    public async Task<IActionResult> GetThreeNearestDeadlineByType(string type)
    {
        var getPosts = await _postService.GetThreeNearestDeadlineByType(type);

        if (getPosts == null)
        {
            return NotFound();
        }

        return Ok(getPosts);
    }
}