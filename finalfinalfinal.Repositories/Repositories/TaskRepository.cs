using Microsoft.EntityFrameworkCore;
using finalfinalfinal.Data;
using finalfinalfinal.Models;
using finalfinalfinal.Repositories.Interfaces;
using finalfinalfinal.Services.Filters;

namespace finalfinalfinal.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _db;
    public TaskRepository(AppDbContext db) => _db = db;

    public async Task<List<TaskItem>> GetAllAsync(TaskFilter f)
    {
        var q = _db.Tasks
            .Include(t => t.Tags)
            .Include(t => t.Project)
            .AsQueryable();

        q = ApplySearch(q, f.Search);
        q = ApplyTags(q, f.Tag);
        q = ApplyPriority(q, f.Priority);
        q = ApplyProject(q, f.ProjectId);
        q = ApplyRange(q, f.Range);

        return await q
            .OrderBy(t => t.DueDate)
            .ThenByDescending(t => t.Priority)
            .ToListAsync();
    }

    public Task<TaskItem?> GetByIdAsync(int id) =>
        _db.Tasks
           .Include(t => t.Tags)
           .Include(t => t.Project)
           .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<TaskItem> AddAsync(TaskItem task)
    {
        await _db.Tasks.AddAsync(task);
        await _db.SaveChangesAsync();
        return task;
    }

    public async Task<TaskItem?> UpdateAsync(TaskItem task)
    {
        _db.Tasks.Update(task);
        await _db.SaveChangesAsync();
        return task;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task == null) return false;

        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();
        return true;
    }

    public Task<bool> ExistsAsync(int id) =>
        _db.Tasks.AnyAsync(t => t.Id == id);

    // ---------- приватные фильтры ----------

    private static IQueryable<TaskItem> ApplySearch(IQueryable<TaskItem> q, string? search)
    {
        if (string.IsNullOrWhiteSpace(search)) return q;
        var s = search.Trim();
        return q.Where(t =>
            t.Title.Contains(s) ||
            (t.Description != null && t.Description.Contains(s)));
    }

    private static IQueryable<TaskItem> ApplyTags(IQueryable<TaskItem> q, string? tag)
    {
        if (string.IsNullOrWhiteSpace(tag)) return q;
        var tags = tag.Split(',', StringSplitOptions.RemoveEmptyEntries)
                      .Select(x => x.Trim())
                      .ToList();
        return q.Where(t => t.Tags.Any(tg => tags.Contains(tg.Name)));
    }

    private static IQueryable<TaskItem> ApplyPriority(IQueryable<TaskItem> q, Priority? p) =>
        p.HasValue ? q.Where(t => t.Priority == p.Value) : q;

    private static IQueryable<TaskItem> ApplyProject(IQueryable<TaskItem> q, int? projectId) =>
        projectId.HasValue ? q.Where(t => t.ProjectId == projectId.Value) : q;

    private static IQueryable<TaskItem> ApplyRange(IQueryable<TaskItem> q, string? range)
    {
        if (string.IsNullOrEmpty(range) || range == "all") return q;

        var now = DateTime.UtcNow.Date;
        DateTime end = range switch
        {
            "day" => now.AddDays(1),
            "week" => now.AddDays(7),
            "month" => now.AddMonths(1),
            _ => now.AddYears(100)
        };

        return q.Where(t => t.DueDate != null && t.DueDate >= now && t.DueDate < end);
    }
}