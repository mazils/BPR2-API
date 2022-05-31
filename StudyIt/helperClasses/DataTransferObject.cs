using System.Runtime.InteropServices;
using StudyIt.MongoDB.Models;
namespace StudyIt.helperClasses;

public class DataTransferObject
{
    public static User ConvertBase64ToBinaryUser(UserDTO receivedUser)
    {
        User user = new User();

        user._id = receivedUser._id;
        user.email = receivedUser.email;
        user.name = receivedUser.name;
        user.education = receivedUser.education;
        user.phoneNumber = receivedUser.phoneNumber;
        user.profilePicture = FileConversion.Base64StringtoBin(receivedUser.profilePicture);
        user.personalityProfile = FileConversion.Base64StringtoBin(receivedUser.personalityProfile);
        user.competences = receivedUser.competences;
        user.interests = receivedUser.interests;

        return user;
    }
    
    public static Company ConvertBase64ToBinaryCompany(CompanyDTO receivedCompany)
    {
        Company company = new Company();

        company._id = receivedCompany._id;
        company.email = receivedCompany.email;
        company.name = receivedCompany.name;
        company.cvr = receivedCompany.cvr;
        company.location = receivedCompany.location;
        company.phoneNumber = receivedCompany.phoneNumber;
        company.description = receivedCompany.description;
        company.logo = FileConversion.Base64StringtoBin(receivedCompany.logo);

        return company;
    }

    public static Post ConvertStringToDateTimePost(PostDTO receivedPost)
    {
        Post post = new Post();
        post._id = receivedPost._id;
        post.title = receivedPost.title;
        post.description = receivedPost.description;
        post.location = receivedPost.location;
        post.competences = receivedPost.competences;
        post.type = receivedPost.type;
        post.deadline = DateTime.Parse(receivedPost.deadline);
        post.companyId = receivedPost.companyId;
        post.application = new List<Application>();
        if(receivedPost.application != null) post.application = receivedPost.application;
        return post;
    }
}