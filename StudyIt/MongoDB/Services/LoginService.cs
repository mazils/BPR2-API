using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using StudyIt.MongoDB.Models;

namespace StudyIt.MongoDB.Services;

public class LoginService
{
    private readonly IMongoCollection<Login> _loginCollection;

    public LoginService(IOptions<StudyItDatabaseSettings> studyItDatabaseSettings)
    {
        var mongoClient = new MongoClient(studyItDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(studyItDatabaseSettings.Value.DatabaseName);

        _loginCollection = mongoDatabase.GetCollection<Login>(studyItDatabaseSettings.Value.CollectionName);
    }

    public async Task<Login?> GetLogin(string email) =>
        await _loginCollection.AsQueryable<Login?>()
        .Where(e => e.username == email).FirstOrDefaultAsync();

    public async Task<Login?> CheckLogin(string username, string password) =>
        await _loginCollection.AsQueryable<Login?>()
            .Where(
                e => e.username == username
                && e.password == password
                ).FirstAsync();

    public async Task CreateLogin(Login login) =>
        await _loginCollection.InsertOneAsync(login);

}