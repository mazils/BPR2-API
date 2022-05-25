using StudyIt.MongoDB.Models;

namespace StudyIt.MongoDB.Services;

public interface IProjectGroupService
{
    //Creating project group
    public Task<String> CreateGroup(ProjectGroup projectGroup);
    //Getting project group
    public Task<ProjectGroup> GetGroup(string email);
}