using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using StudyIt.MongoDB.Models;

namespace StudyIt.MongoDB.Services;

public class UserService
{
    private readonly IMongoCollection<User> _userCollection;

    public UserService(IOptions<StudyItDatabaseSettings> studyItDatabaseSettings)
    {
        var mongoClient = new MongoClient(studyItDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(studyItDatabaseSettings.Value.DatabaseName);

        _userCollection = mongoDatabase.GetCollection<User>(studyItDatabaseSettings.Value.UserCollection);
    }

    // Creating a User
    public async Task Register(User user) =>
        await _userCollection.InsertOneAsync(user);

    // Finding a User by email
    public async Task<User?> GetUserByEmail(string email) =>
        await _userCollection.AsQueryable<User>()
            .Where(e => e.email == email).FirstOrDefaultAsync();

    // Finding a User by id  
    public async Task<User?> GetUserbyId(string _id) =>
        await _userCollection.AsQueryable<User>()
            .Where(e => e._id == _id).FirstOrDefaultAsync();
    //updating user
     public async Task<ReplaceOneResult> updateUser(User updatedUser) =>
            await _userCollection.ReplaceOneAsync(r => r._id == updatedUser._id, updatedUser);
            
    //updating picture
    public async Task UpdatePicture(string _id,byte[] fileBytes) =>
       
        await _userCollection.UpdateOneAsync(x => x._id == _id, Builders<User>.Update.Set(x => x.profilePicture, fileBytes));
        
    //updating picture
    public async Task UpdatePersonalityProfile(string _id,byte[] fileBytes) =>
       
        await _userCollection.UpdateOneAsync(x => x._id == _id, Builders<User>.Update.Set(x => x.personalityProfile, fileBytes));
    
    //getting GetProfilePicture
     public async Task<byte[]> GetProfilePicture(string _id) {
        User user = await _userCollection.AsQueryable<User>().Where(e => e._id == _id).FirstOrDefaultAsync();
        var profilePicture = user.profilePicture;
        return profilePicture;
     }
    //getting personality profile
     public async Task<byte[]> GetPersonalityProfile(string _id) {
        User user = await _userCollection.AsQueryable<User>().Where(e => e._id == _id).FirstOrDefaultAsync();
        var personalityProfile = user.profilePicture;
        return personalityProfile;
     }
}

