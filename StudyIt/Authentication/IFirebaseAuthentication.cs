namespace StudyIt;

public interface IFirebaseAuthentication
{
    public Task<bool> Verify(string token);
}