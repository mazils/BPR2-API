using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using StudyIt.MongoDB.Models;

namespace StudyIt.MongoDB.Services;

public class PostService
{
    private readonly IMongoCollection<Post> _postCollection;
    private readonly IMongoCollection<BsonDocument> _postCollectionCreate;

    public PostService(IOptions<StudyItDatabaseSettings> studyItDatabaseSettings)
    {
        var mongoClient = new MongoClient(studyItDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(studyItDatabaseSettings.Value.DatabaseName);

        _postCollection = mongoDatabase.GetCollection<Post>(studyItDatabaseSettings.Value.PostCollection);
        _postCollectionCreate = mongoDatabase.GetCollection<BsonDocument>(studyItDatabaseSettings.Value.PostCollection);
    }

    // Creating a post
    public async Task CreatePost(Post post)
    {
        var newPost = new BsonDocument
        {
            {
                "title", post.title
            },
            {
                "description", post.description
            },
            {
                "location", post.location
            },
            {
                "competences", new BsonArray(post.competences)
            },
            {
                "type", post.type
            },
            {
                "deadline", new BsonDateTime(post.deadline)
            },
            {
                "companyId", post.companyId
            }
        };
        await _postCollectionCreate.InsertOneAsync(newPost);
        
        
    }
}