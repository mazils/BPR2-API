using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using StudyIt.MongoDB.Models;

namespace StudyIt.MongoDB.Services;

public class CompanyService
{
    private readonly IMongoCollection<Company> _companyCollection;

    public CompanyService(IOptions<StudyItDatabaseSettings> studyItDatabaseSettings)
    {
        var mongoClient = new MongoClient(studyItDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(studyItDatabaseSettings.Value.DatabaseName);

        _companyCollection = mongoDatabase.GetCollection<Company>(studyItDatabaseSettings.Value.CompanyCollection);
    }

    // Creating a Company
    public async Task CreateCompany(Company company) =>
        await _companyCollection.InsertOneAsync(company);

    // Finding a Company
    public async Task<Company?> GetCompany(string email) =>
        await _companyCollection.AsQueryable<Company>()
            .Where(e => e.email == email).FirstOrDefaultAsync();
}