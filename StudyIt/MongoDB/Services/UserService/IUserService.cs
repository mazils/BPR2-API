using MongoDB.Driver;
using StudyIt.MongoDB.Models;

namespace StudyIt.MongoDB.Services;

public interface IUserService
{
    // Creating a User
    public Task Register(User user);
    // Finding a User by email
    public Task<User?> GetByEmail(string email);
    // Finding a User by id
    public Task<User?> GetById(string _id);
    // Updating user
    public Task<ReplaceOneResult> UpdateUser(User updatedUser);
    // Updating Profile Picture
    public Task UpdatePicture(string _id, byte[] fileBytes);
    // Updating Personality Profile
    public Task UpdatePersonalityProfile(string _id, byte[] fileBytes);
    // Getting Profile Picture
    public Task<byte[]> GetProfilePicture(string _id);
    // Getting Personality Profile
    public Task<byte[]> GetPersonalityProfile(string _id);

}