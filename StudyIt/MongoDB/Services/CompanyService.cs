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
    public async Task Register(Company company) =>
        await _companyCollection.InsertOneAsync(company);

    // Finding a Company by email
    public async Task<Company?> GetCompanyByEmail(string email) =>
        await _companyCollection.AsQueryable<Company>()
            .Where(e => e.email == email).FirstOrDefaultAsync();

    // Finding a Company by id
    public async Task<Company?> GetCompanyById(string _id) =>
        await _companyCollection.AsQueryable<Company>()
            .Where(e => e._id == _id).FirstOrDefaultAsync();

    //updating company
     public async Task<ReplaceOneResult> Update(Company company) {
           var result =  await _companyCollection.ReplaceOneAsync(r => r._id == company._id, company);
           return result;
     }
     
}