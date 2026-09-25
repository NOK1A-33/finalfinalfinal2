using finalfinalfinal.Models;

namespace finalfinalfinal.Repositories.Interfaces;

public interface IProjectRepository
{
    Task<List<Project>> GetAllWithCountsAsync();
    Task<Project?> GetByIdAsync(int id);
    Task<Project> AddAsync(Project project);
    Task<bool> DeleteAsync(int id);
}