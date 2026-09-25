using finalfinalfinal.Models;

namespace finalfinalfinal.Services;

public record ProjectSummary(int Id, string Name, string Color, int Count);

public interface IProjectService
{
    Task<List<ProjectSummary>> GetAllAsync();
    Task<Project> CreateAsync(string name, string? color);
    Task<bool> DeleteAsync(int id);
}