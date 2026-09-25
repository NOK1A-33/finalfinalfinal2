using finalfinalfinal.Models;
using finalfinalfinal.Repositories.Interfaces;

namespace finalfinalfinal.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projects;
    public ProjectService(IProjectRepository projects) => _projects = projects;

    public async Task<List<ProjectSummary>> GetAllAsync()
    {
        var projects = await _projects.GetAllWithCountsAsync();
        return projects
            .Select(p => new ProjectSummary(p.Id, p.Name, p.Color, p.Tasks.Count))
            .ToList();
    }

    public Task<Project> CreateAsync(string name, string? color) =>
        _projects.AddAsync(new Project
        {
            Name = name,
            Color = color ?? "#3b82f6"
        });

    public Task<bool> DeleteAsync(int id) =>
        _projects.DeleteAsync(id);
}