using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

 // Creating all posts
 public async Task<AllCompanyPosts> GetAllCompanyPosts(string _id)
 {
     var dataFacet = AggregateFacet.Create("dataFacet",
         PipelineDefinition<Post, Post>.Create(new[]
         {
             PipelineStageDefinitionBuilder.Sort(Builders<Post>.Sort.Ascending(x => x.deadline))
         }));

     var filter = Builders<Post>.Filter.Eq(x => x.companyId, _id);
     
     var aggregation = await _postCollection.Aggregate()
         .Match(filter)
         .Facet(dataFacet)
         .ToListAsync();
     
     var data = aggregation.First()
         .Facets.First(x => x.Name == "dataFacet")
         .Output<Post>();

     var allCompanyPosts = new AllCompanyPosts()
     {
         data = data
     };

     return allCompanyPosts;
 }
 
 
}