using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using StudyIt.MongoDB.Models;

namespace StudyIt.MongoDB.Services;

public class PostService : IPostService
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
    
    public async Task<Post?> GetPostByCompanyId(string _id) =>
           await _postCollection.AsQueryable<Post>()
               .Where(e => e.companyId == _id).FirstOrDefaultAsync();
    
    public async Task<Post?> GetPostById(string _id) =>
        await _postCollection.AsQueryable<Post>()
            .Where(e => e._id == _id).FirstOrDefaultAsync();

    public async Task<ReplaceOneResult> UpdatePost(Post updatedPost) =>
        await _postCollection.ReplaceOneAsync(r => r._id == updatedPost._id, updatedPost);

    public async Task<UpdateResult> ApplyToPost(string postId,Application applicationFromUser)
    {
        Application application = new Application();
        application.applicants = applicationFromUser.applicants;
        application.status = "In progress";
        application._id = ObjectId.GenerateNewId();
        var filterBuilder = Builders<Post>.Filter;
        var filter = filterBuilder.Eq(x => x._id, postId);
        var updateBuilder = Builders<Post>.Update;
        var update = updateBuilder.Push(doc => doc.application,application );
        return await _postCollection.UpdateOneAsync(filter, update);
    }
    
    public async Task<AllCompanyPosts> GetThreeNearestDeadlineByType(string type)
    {
        var dataFacet = AggregateFacet.Create("dataFacet",
            PipelineDefinition<Post, Post>.Create(new[]
            {
                PipelineStageDefinitionBuilder.Sort(Builders<Post>.Sort.Ascending(x => x.deadline)),
                PipelineStageDefinitionBuilder.Limit<Post>(3)
            }));

        var filterDate = Builders<Post>.Filter.Gte(x => x.deadline, DateTime.UtcNow);
        var filterType = Builders<Post>.Filter.Eq(x => x.type, type);

        var filterCombination = Builders<Post>.Filter.And(filterDate, filterType);

        var aggregation = await _postCollection.Aggregate()
            .Match(filterCombination)
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