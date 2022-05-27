using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using StudyIt.MongoDB.Models;

namespace StudyIt.MongoDB.Services;

public class UserService: IUserService
{
    private readonly IMongoCollection<User> _userCollection;
    private readonly IMongoCollection<BsonDocument> _userCollectionRegister;

    private readonly string _defaultUserImage;
    private readonly string _defaultPersonalityProfile;

    public UserService(IOptions<StudyItDatabaseSettings> studyItDatabaseSettings)
    {
        var mongoClient = new MongoClient(studyItDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(studyItDatabaseSettings.Value.DatabaseName);

        _userCollection = mongoDatabase.GetCollection<User>(studyItDatabaseSettings.Value.UserCollection);
        _userCollectionRegister =
            mongoDatabase.GetCollection<BsonDocument>(studyItDatabaseSettings.Value.UserCollection);

        _defaultUserImage = studyItDatabaseSettings.Value.DefaultUserImage;
        _defaultPersonalityProfile = studyItDatabaseSettings.Value.DefaultPersonalityProfile;
    }

    public async Task<bool> Register(User user)
    {
        var newLogin = new BsonDocument
        {
            {
                "email", user.email
            },
            {
                "name", user.name
            },
            {
                "education", user.education
            },
            {
                "phoneNumber", new BsonString("")
            },
            {
                "profilePicture", new BsonBinaryData(FileConversion.Base64StringtoBin(_defaultUserImage))
            },
            {
                "personalityProfile", new BsonBinaryData(FileConversion.Base64StringtoBin(_defaultPersonalityProfile))
            },
            {
                "competences", new BsonArray
                {
                    ""
                }
            },
            {
                "interests", new BsonArray
                {
                    ""
                }
            }
        };
        try
        {
            await _userCollectionRegister.InsertOneAsync(newLogin);
            return true;
        }
        catch (MongoWriteException e)
        {
            return false;
        }
        
    }
    
    public async Task<User?> GetByEmail(string email) =>
        await _userCollection.AsQueryable<User>()
            .Where(e => e.email == email).FirstOrDefaultAsync();
    
    public async Task<User?> GetById(string _id) =>
        await _userCollection.AsQueryable<User>()
            .Where(e => e._id == _id).FirstOrDefaultAsync();

    
    public async Task<ReplaceOneResult> UpdateUser(User updatedUser) =>
        await _userCollection.ReplaceOneAsync(r => r._id == updatedUser._id, updatedUser);
    
    public async Task UpdatePicture(string _id, byte[] fileBytes) =>
        await _userCollection.UpdateOneAsync(x => x._id == _id,
            Builders<User>.Update.Set(x => x.profilePicture, fileBytes));
    
    public async Task UpdatePersonalityProfile(string _id, byte[] fileBytes) =>
        await _userCollection.UpdateOneAsync(x => x._id == _id,
            Builders<User>.Update.Set(x => x.personalityProfile, fileBytes));
    
    public async Task<byte[]> GetProfilePicture(string _id)
    {
        User user = await _userCollection.AsQueryable<User>().Where(e => e._id == _id).FirstOrDefaultAsync();
        var profilePicture = user.profilePicture;
        return profilePicture;
    }
    
    public async Task<byte[]> GetPersonalityProfile(string _id)
    {
        User user = await _userCollection.AsQueryable<User>().Where(e => e._id == _id).FirstOrDefaultAsync();
        var personalityProfile = user.personalityProfile;
        return personalityProfile;
    }
}