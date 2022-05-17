using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using StudyIt.MongoDB.Models;

namespace StudyIt.MongoDB.Services;

public class CompanyService
{
    private readonly IMongoCollection<Company> _companyCollection;
    private readonly IMongoCollection<BsonDocument> _companyCollectionRegister;

    private readonly string _defaultCompanyLogo;

    public CompanyService(IOptions<StudyItDatabaseSettings> studyItDatabaseSettings)
    {
        var mongoClient = new MongoClient(studyItDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(studyItDatabaseSettings.Value.DatabaseName);

        _companyCollection = mongoDatabase.GetCollection<Company>(studyItDatabaseSettings.Value.CompanyCollection);
        _companyCollectionRegister =
            mongoDatabase.GetCollection<BsonDocument>(studyItDatabaseSettings.Value.CompanyCollection);

        _defaultCompanyLogo = studyItDatabaseSettings.Value.DefaultCompanyLogo;
    }

    // Creating a Company
    public async Task Register(Company company)
    {
        var newCompany = new BsonDocument
        {
            {
                "email", company.email
            },
            {
                "name", company.name
            },
            {
                "cvr", company.cvr
            },
            {
                "location", company.location
            },
            {
                "phoneNumber", company.phoneNumber
            },
            {
                "description", company.description
            },
            {
                "logo", new BsonBinaryData(FileConversion.Base64StringtoBin(_defaultCompanyLogo))
            }
        };
        await _companyCollectionRegister.InsertOneAsync(newCompany);
    }

    // Finding a Company by email
    public async Task<Company?> GetByEmail(string email) =>
        await _companyCollection.AsQueryable<Company>()
            .Where(e => e.email == email).FirstOrDefaultAsync();

    // Finding a Company by id
    public async Task<Company?> GetById(string _id) =>
        await _companyCollection.AsQueryable<Company>()
            .Where(e => e._id == _id).FirstOrDefaultAsync();

    // Updating company
    public async Task<ReplaceOneResult> UpdateCompany(Company updatedCompany) =>
        await _companyCollection.ReplaceOneAsync(c => c._id == updatedCompany._id, updatedCompany);

    // Updating Logo
    public async Task UpdateLogo(string _id, byte[] fileBytes) =>
        await _companyCollection.UpdateOneAsync(x => x._id == _id,
            Builders<Company>.Update.Set(x => x.logo, fileBytes));

    // Getting Logo
    public async Task<byte[]> GetLogo(string _id)
    {
        Company company =
            await _companyCollection.AsQueryable<Company>().Where(e => e._id == _id).FirstOrDefaultAsync();
        var logo = company.logo;
        return logo;
    }
}