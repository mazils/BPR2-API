using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using StudyIt.MongoDB.Models;

namespace StudyIt.MongoDB.Services;

public class GuestService
{
    private readonly IMongoCollection<Post> _posts;

    public GuestService(IOptions<StudyItDatabaseSettings> studyItDatabaseSettings)
    {
        var mongoClient = new MongoClient(studyItDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(studyItDatabaseSettings.Value.DatabaseName);

        _posts = mongoDatabase.GetCollection<Post>(studyItDatabaseSettings.Value.PostCollection);
    }

    // Finding a Searched term in posts
    public async Task<Post?> GetSearchedPost(string searchTerm) =>
        await _posts.AsQueryable<Post>()
            .Where(e => e.title == searchTerm).FirstOrDefaultAsync();
}