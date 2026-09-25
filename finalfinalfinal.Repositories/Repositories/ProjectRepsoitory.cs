using Microsoft.EntityFrameworkCore;
using finalfinalfinal.Data;
using finalfinalfinal.Models;
using finalfinalfinal.Repositories.Interfaces;

namespace finalfinalfinal.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _db;
    public ProjectRepository(AppDbContext db) => _db = db;

    public Task<List<Project>> GetAllWithCountsAsync() =>
        _db.Projects.Include(p => p.Tasks).ToListAsync();

    public Task<Project?> GetByIdAsync(int id) =>
        _db.Projects.FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Project> AddAsync(Project project)
    {
        await _db.Projects.AddAsync(project);
        await _db.SaveChangesAsync();
        return project;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var project = await _db.Projects.FindAsync(id);
        if (project == null) return false;

        _db.Projects.Remove(project);
        await _db.SaveChangesAsync();
        return true;
    }
}