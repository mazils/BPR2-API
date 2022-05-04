using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;

namespace StudyIt;

public class Firebase
{
    private static Firebase firebaseInstance;
    private FirebaseApp defaultApp;
    private  GoogleCredential Credential;
    private Firebase()
    {
        defaultApp = FirebaseApp.Create(new AppOptions()
        {
            //TODO: try to change to GoogleCredential.default
            Credential = GoogleCredential.FromFile("studyit-df727-f40ab4417afd.json"),
        });
    }

    public static Firebase GetInstance()
    {
        if (firebaseInstance == null)
        {
            firebaseInstance = new Firebase();
        }
        return firebaseInstance;
    }
    public async Task<bool> varify(string token)
    {
        try
        {
            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
            return true;
        }
        catch (FirebaseAuthException  e)
        {
            Console.WriteLine(e.ErrorCode);
            return false;
            
        } 
        catch(Exception e)
        {
             Console.WriteLine(e.ToString());
             return false;
        }
    }
}





 // bool notExpiredTime;
            // bool notExpiredIssuedTime;
            // //Expiration time	Must be in the future. The time is measured in seconds since the UNIX epoch.
            // {
            //     DateTimeOffset now = DateTimeOffset.UtcNow;
            //     long unixTimeSecondsNow = now.ToUnixTimeSeconds();
            //     notExpiredTime = decodedToken.ExpirationTimeSeconds - unixTimeSecondsNow > 0;
            // }
            // //Issued-at time	Must be in the past. The time is measured in seconds since the UNIX epoch
            // {
            //     DateTimeOffset now = DateTimeOffset.UtcNow;
            //     long unixTimeSecondsNow = now.ToUnixTimeSeconds();
            //     notExpiredIssuedTime = decodedToken.IssuedAtTimeSeconds - unixTimeSecondsNow < 0;
            // }
            // //Audience	Must be your Firebase project ID, the unique identifier for your Firebase project, which can be found in the URL of that project's console.
            // bool isValidProjectId = defaultApp.Options.ProjectId.Equals(decodedToken.Audience);
            // //Issuer	Must be "https://securetoken.google.com/<projectId>", where <projectId> is the same project ID used for aud above.
            // bool isValidIssuer = ("https://securetoken.google.com/"+defaultApp.Options.ProjectId).Equals(decodedToken.Issuer);
            // //Subject	Must be a non-empty string and must be the uid of the user or device.
            // //bool isValidSubject = decodedToken.Subject.Length != 0 &&
            
            // //Authentication time	Must be in the past. The time when the user authenticated.
            // {
            //     DateTimeOffset now = DateTimeOffset.UtcNow;
            //     long unixTimeSecondsNow = now.ToUnixTimeSeconds();
            //     notExpiredIssuedTime = decodedToken. - unixTimeSecondsNow < 0;
            // }
            