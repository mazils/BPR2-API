using StudyIt.MongoDB.Models;

namespace StudyIt.helperClasses;

public class DataTransferObject
{
    public static User ConvertBase64ToBinary(UserDTO receivedUser)
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
}