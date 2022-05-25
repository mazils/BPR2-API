namespace StudyIt;

public interface IFirebaseAutharization
{
    public Task<bool> Verify(string token);
}