using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;

namespace StudyIt;

public class FirebaseAuthentication : IFirebaseAuthentication
{
    private static FirebaseAuthentication firebaseInstance;
    private GoogleCredential Credential;

    private FirebaseAuthentication()
    {
        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile("studyit-df727-f40ab4417afd.json"),
        });
    }

    public static FirebaseAuthentication GetInstance()
    {
        if (firebaseInstance == null)
        {
            firebaseInstance = new FirebaseAuthentication();
        }

        return firebaseInstance;
    }

    public async Task<bool> Verify(string token)
    {
        try
        {
            await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
            return true;
        }
        catch (FirebaseAuthException e)
        {
            Console.WriteLine(e.ErrorCode);
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            return false;
        }
    }
}