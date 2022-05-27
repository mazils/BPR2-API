using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;


using StudyIt.MongoDB.Models;

public interface ICompanyService
{
    // Creating a Company
    public  Task<bool> Register(Company company);
    // Finding a Company by email
    public  Task<Company?> GetByEmail(string email);
    // Finding a Company by id
    public Task<Company?> GetById(string _id);
    // Updating company
    public Task<ReplaceOneResult> UpdateCompany(Company updatedCompany);
    // Updating Logo
    public Task UpdateLogo(string _id, byte[] fileBytes);
    // Getting Logo
    public Task<byte[]> GetLogo(string _id);
}  