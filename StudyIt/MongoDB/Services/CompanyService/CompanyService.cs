using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using StudyIt.MongoDB.Models;

namespace StudyIt.MongoDB.Services;

public class CompanyService :ICompanyService
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
    
    public async Task<bool> Register(Company company)
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
                "location", new BsonString("")
            },
            {
                "phoneNumber", new BsonString("")
            },
            {
                "description", new BsonString("")
            },
            {
                "logo", new BsonBinaryData(FileConversion.Base64StringtoBin(_defaultCompanyLogo))
            }
        };
        try
        {
            await _companyCollectionRegister.InsertOneAsync(newCompany);
            return true;
        }
        catch (MongoWriteException e)
        {
           return false;
        }
        
    }
    
    public async Task<Company?> GetByEmail(string email) =>
        await _companyCollection.AsQueryable<Company>()
            .Where(e => e.email == email).FirstOrDefaultAsync();

    public async Task<Company?> GetById(string _id) =>
        await _companyCollection.AsQueryable<Company>()
            .Where(e => e._id == _id).FirstOrDefaultAsync();
    
    public async Task<ReplaceOneResult> UpdateCompany(Company updatedCompany) =>
        await _companyCollection.ReplaceOneAsync(c => c._id == updatedCompany._id, updatedCompany);

    public async Task UpdateLogo(string _id, byte[] fileBytes) =>
        await _companyCollection.UpdateOneAsync(x => x._id == _id,
            Builders<Company>.Update.Set(x => x.logo, fileBytes));
    
    public async Task<byte[]> GetLogo(string _id)
    {
        Company company =
            await _companyCollection.AsQueryable<Company>().Where(e => e._id == _id).FirstOrDefaultAsync();
        var logo = company.logo;
        return logo;
    }
}