using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using StudyIt.MongoDB.Models;

namespace StudyIt.MongoDB.Services;

public class SearchService: ISearchService
{
    private readonly IMongoCollection<Post> _posts;

    public SearchService(IOptions<StudyItDatabaseSettings> studyItDatabaseSettings)
    {
        var mongoClient = new MongoClient(studyItDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(studyItDatabaseSettings.Value.DatabaseName);

        _posts = mongoDatabase.GetCollection<Post>(studyItDatabaseSettings.Value.PostCollection);
    }

    public async Task<SearchResult> GetSearchResult(string searchTerm, string location, string type, int page)
    {
        const int pageSize = 5;

        // count facet, aggregation stage of count
        var countFacet = AggregateFacet.Create("countFacet",
            PipelineDefinition<Post, AggregateCountResult>.Create(new[]
            {
                PipelineStageDefinitionBuilder.Count<Post>()
            }));

        // data facet, we’ll use this to sort the data and do the skip and limiting of the results for the paging.
        var dataFacet = AggregateFacet.Create("dataFacet",
            PipelineDefinition<Post, Post>.Create(new[]
            {
                PipelineStageDefinitionBuilder.Sort(Builders<Post>.Sort.Ascending(x => x.deadline)),
                PipelineStageDefinitionBuilder.Skip<Post>((page - 1) * pageSize),
                PipelineStageDefinitionBuilder.Limit<Post>(pageSize)
            }));

        var filterDate = Builders<Post>.Filter.Gte(x => x.deadline, DateTime.UtcNow);
        var filterSearch = Builders<Post>.Filter.Regex(x => x.title, new BsonRegularExpression(searchTerm, "i"));

        var filterLocation = Builders<Post>.Filter.Eq(x => x.location, location);
        var filterType = Builders<Post>.Filter.Eq(x => x.type, type);

        var filterComb = Builders<Post>.Filter.And(filterDate, filterSearch);

        if (location != null)
        {
            filterComb = Builders<Post>.Filter.And(filterComb, filterLocation);
        }

        if (type != null)
        {
            filterComb = Builders<Post>.Filter.And(filterComb, filterType);
        }

        if (location != null && type != null)
        {
            filterComb = Builders<Post>.Filter.And(filterComb, filterLocation, filterType);
        }

        var aggregation = await _posts.Aggregate()
            .Match(filterComb)
            .Facet(countFacet, dataFacet)
            .ToListAsync();

        var count = aggregation.First()
            .Facets.First(x => x.Name == "countFacet")
            .Output<AggregateCountResult>()
            ?.FirstOrDefault()
            ?.Count ?? 0;

        var data = aggregation.First()
            .Facets.First(x => x.Name == "dataFacet")
            .Output<Post>();

        var searchResult = new SearchResult()
        {
            Docs = (int)count,
            TotalPages = (int)Math.Round((double)count / pageSize),
            CurrentPage = page,
            PageSize = pageSize,
            Data = data
        };

        return searchResult;
    }
}