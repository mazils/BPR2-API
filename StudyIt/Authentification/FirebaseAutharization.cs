using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;

namespace StudyIt;

public class FirebaseAutharization : IFirebaseAutharization
{
    private static FirebaseAutharization firebaseInstance;
    private GoogleCredential Credential;

    private FirebaseAutharization()
    {
        FirebaseApp.Create(new AppOptions()
        {
            // Credential = GoogleCredential.FromFile("studyit-df727-f40ab4417afd.json"),
            Credential = GoogleCredential.GetApplicationDefault(),
        });
    }

    public static FirebaseAutharization GetInstance()
    {
        if (firebaseInstance == null)
        {
            firebaseInstance = new FirebaseAutharization();
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