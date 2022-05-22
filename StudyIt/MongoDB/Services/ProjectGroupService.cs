using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using StudyIt.MongoDB.Models;

namespace StudyIt.MongoDB.Services;

public class ProjectGroupService
{
    private readonly IMongoCollection<User> _userCollection;
    private readonly IMongoCollection<ProjectGroup> _ProjectGroupCollection;

    private readonly IMongoCollection<BsonDocument> _companyCollectionRegister;

    public ProjectGroupService(IOptions<StudyItDatabaseSettings> studyItDatabaseSettings)
    {
        var mongoClient = new MongoClient(studyItDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(studyItDatabaseSettings.Value.DatabaseName);
        _userCollection = mongoDatabase.GetCollection<User>(studyItDatabaseSettings.Value.UserCollection);
        _ProjectGroupCollection =
            mongoDatabase.GetCollection<ProjectGroup>(studyItDatabaseSettings.Value.ProjectGroupCollection);
    }

    // Creating project group
    public async Task<String> CreateGroup(ProjectGroup projectGroup)
    {
        try
        {
            await _ProjectGroupCollection.InsertOneAsync(projectGroup);
            return string.Empty;
        }
        catch (MongoWriteException e)
        {
            return Regex.Replace(e.Message.Split("dup key: ")[1], "[{\"}]", string.Empty).TrimEnd('.').Trim();
        }
    }

    // Getting project group
    public async Task<ProjectGroup> GetGroup(string email) =>
        await _ProjectGroupCollection.AsQueryable<ProjectGroup>()
            .Where(e => e.userEmails.Contains(email)).FirstOrDefaultAsync();
}