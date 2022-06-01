using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using StudyIt.MongoDB.Models;

namespace StudyIt.MongoDB.Services;

public class ProjectGroupService : IProjectGroupService
{
    private readonly IMongoCollection<ProjectGroup> _ProjectGroupCollection;

    private readonly IMongoCollection<BsonDocument> _companyCollectionRegister;

    public ProjectGroupService(IOptions<StudyItDatabaseSettings> studyItDatabaseSettings)
    {
        var mongoClient = new MongoClient(studyItDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(studyItDatabaseSettings.Value.DatabaseName);
        _ProjectGroupCollection =
            mongoDatabase.GetCollection<ProjectGroup>(studyItDatabaseSettings.Value.ProjectGroupCollection);
    }

    public async Task<bool> CreateGroup(ProjectGroup projectGroup)
    {
        try
        {
            await _ProjectGroupCollection.InsertOneAsync(projectGroup);
            return true;
        }
        catch (MongoWriteException e)
        {
            return false;
        }
    }

    public async Task<ProjectGroup> GetGroup(string email) =>
        await _ProjectGroupCollection.AsQueryable<ProjectGroup>()
            .Where(e => e.userEmails.Contains(email)).FirstOrDefaultAsync();
}