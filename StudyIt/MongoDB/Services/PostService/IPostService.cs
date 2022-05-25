using MongoDB.Driver;
using StudyIt.MongoDB.Models;

namespace StudyIt.MongoDB.Services;

public interface IPostService
{
    // Creating a post
    public Task CreatePost(Post post);
    // Getting all company posts
    public Task<AllCompanyPosts> GetAllCompanyPosts(string _id);
    // getting one post by Company id
    public Task<Post?> GetPostByCompanyId(string _id);
    // getting one post by post id
    public Task<Post?> GetPostById(string _id);
    public Task<ReplaceOneResult> UpdatePost(Post updatedPost);
    public Task<UpdateResult> ApplyToPost(string postId, Application applicationFromUser);
    public Task<AllCompanyPosts> GetThreeNearestDeadlineByType(string type);


}