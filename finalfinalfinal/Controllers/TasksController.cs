using finalfinalfinal.Data;
using finalfinalfinal.DTOs;
using finalfinalfinal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 

namespace finalfinalfinal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _db;
    public TasksController(AppDbContext db) => _db = db;
     
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] string? tag,
        [FromQuery] Priority? priority,
        [FromQuery] int? projectId,
        [FromQuery] string? range)
    {
        var q = _db.Tasks.Include(t => t.Tags).Include(t => t.Project).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            q = q.Where(t => t.Title.Contains(s) ||
                             (t.Description != null && t.Description.Contains(s)));
        }

        if (!string.IsNullOrWhiteSpace(tag))
        {
            var tags = tag.Split(',', StringSplitOptions.RemoveEmptyEntries)
                          .Select(x => x.Trim()).ToList();
            q = q.Where(t => t.Tags.Any(tg => tags.Contains(tg.Name)));
        }

        if (priority.HasValue) q = q.Where(t => t.Priority == priority.Value);
        if (projectId.HasValue) q = q.Where(t => t.ProjectId == projectId.Value);

        if (!string.IsNullOrEmpty(range) && range != "all")
        {
            var now = DateTime.UtcNow.Date;
            DateTime end = range switch
            {
                "day" => now.AddDays(1),
                "week" => now.AddDays(7),
                "month" => now.AddMonths(1),
                _ => now.AddYears(100)
            };
            q = q.Where(t => t.DueDate != null && t.DueDate >= now && t.DueDate < end);
        }

        var list = await q.OrderBy(t => t.DueDate).ThenByDescending(t => t.Priority).ToListAsync();
        return Ok(list);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var t = await _db.Tasks.Include(x => x.Tags).Include(x => x.Project)
                               .FirstOrDefaultAsync(x => x.Id == id);
        return t == null ? NotFound() : Ok(t);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TaskCreateDto dto)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            Priority = dto.Priority,
            ProjectId = dto.ProjectId,
            Tags = await ResolveTags(dto.Tags)
        };
        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = task.Id }, task);
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Update(int id, TaskUpdateDto dto)
    {
        var t = await _db.Tasks.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        if (t == null) return NotFound();

        if (dto.Title != null) t.Title = dto.Title;
        if (dto.Description != null) t.Description = dto.Description;
        if (dto.DueDate.HasValue) t.DueDate = dto.DueDate;
        if (dto.Priority.HasValue) t.Priority = dto.Priority.Value;
        if (dto.Completed.HasValue) t.Completed = dto.Completed.Value;
        if (dto.ProjectId.HasValue) t.ProjectId = dto.ProjectId.Value;

        if (dto.Tags != null)
        {
            t.Tags.Clear();
            foreach (var tag in await ResolveTags(dto.Tags)) t.Tags.Add(tag);
        }

        await _db.SaveChangesAsync();
        return Ok(t);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var t = await _db.Tasks.FindAsync(id);
        if (t == null) return NotFound();
        _db.Tasks.Remove(t);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private async Task<List<Tag>> ResolveTags(List<string>? names)
    {
        if (names == null || names.Count == 0) return new();
        var clean = names.Select(n => n.Trim()).Where(n => n.Length > 0).Distinct().ToList();
        var existing = await _db.Tags.Where(t => clean.Contains(t.Name)).ToListAsync();
        var result = new List<Tag>(existing);
        foreach (var name in clean.Except(existing.Select(e => e.Name)))
            result.Add(new Tag { Name = name });
        return result;
    }
}