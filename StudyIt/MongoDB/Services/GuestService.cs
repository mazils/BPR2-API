using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using StudyIt.MongoDB.Models;

namespace StudyIt.MongoDB.Services;

public class GuestService
{
    private readonly IMongoCollection<Post> _posts;
    private int pageSize = 5;
    private int page = 1;
        

    public GuestService(IOptions<StudyItDatabaseSettings> studyItDatabaseSettings)
    {
        var mongoClient = new MongoClient(studyItDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(studyItDatabaseSettings.Value.DatabaseName);

        _posts = mongoDatabase.GetCollection<Post>(studyItDatabaseSettings.Value.PostCollection);
    }

    // Finding a Searched term in posts
    public async Task<List<Post>> GetSearchedPost(string searchTerm)
    {
        var filter = Builders<Post>.Filter.Eq(x => x.title, searchTerm);
        var filter2 = Builders<Post>.Filter.Eq(x => x.description, searchTerm);
        var aggregateFluent = await _posts.Aggregate().Match(filter).ToListAsync();

        var countFace = AggregateFacet.Create("count",
        PipelineDefinition<Post, AggregateCountResult>.Create(
                new []
                {
                    PipelineStageDefinitionBuilder.Count<Post>()
                })
        );

        var dataFacet = AggregateFacet.Create("data",
            PipelineDefinition<Post, Post>.Create(new[]
            {
                PipelineStageDefinitionBuilder.Sort(Builders<Post>.Sort.Ascending(x=> x.title)),
                PipelineStageDefinitionBuilder.Skip<Post>((page - 1) * pageSize),
                PipelineStageDefinitionBuilder.Limit<Post>(pageSize),
            }));
        
        
        
        var aggregation = await _posts.Aggregate().Match(filter).Facet(countFace, dataFacet).ToListAsync();

        var data = aggregation.First().Facets.First(x => x.Name == "data").Output<Post>();
        
        return aggregateFluent;
    }
    
    //     await _posts.AsQueryable<Post?>()
    //         .Where(e => e.title.Contains(searchTerm) || e.description.Contains(searchTerm)).Take(30).ToListAsync();
    // 
    // public async  Task<List<Post>> GetSearchResults(string searchTerm) =>
    // {
    //     var searchTerm = 
    //     var filter = Builders<Post>.Filter.Text();
    // }
}