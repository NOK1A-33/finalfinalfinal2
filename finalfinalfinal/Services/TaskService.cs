using finalfinalfinal.DTOs;
using finalfinalfinal.Models;
using finalfinalfinal.Repositories.Interfaces;
using finalfinalfinal.Services.Filters;

namespace finalfinalfinal.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _tasks;
    private readonly ITagRepository _tags;

    public TaskService(ITaskRepository tasks, ITagRepository tags)
    {
        _tasks = tasks;
        _tags = tags;
    }

    public Task<List<TaskItem>> GetAllAsync(TaskFilter filter) =>
        _tasks.GetAllAsync(filter);

    public Task<TaskItem?> GetByIdAsync(int id) =>
        _tasks.GetByIdAsync(id);

    public async Task<TaskItem> CreateAsync(TaskCreateDto dto)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            Priority = dto.Priority,
            ProjectId = dto.ProjectId,
            Tags = await ResolveTagsAsync(dto.Tags)
        };

        return await _tasks.AddAsync(task);
    }

    public async Task<TaskItem?> UpdateAsync(int id, TaskUpdateDto dto)
    {
        var task = await _tasks.GetByIdAsync(id);
        if (task == null) return null;

        if (dto.Title != null) task.Title = dto.Title;
        if (dto.Description != null) task.Description = dto.Description;
        if (dto.DueDate.HasValue) task.DueDate = dto.DueDate;
        if (dto.Priority.HasValue) task.Priority = dto.Priority.Value;
        if (dto.Completed.HasValue) task.Completed = dto.Completed.Value;
        if (dto.ProjectId.HasValue) task.ProjectId = dto.ProjectId.Value;

        if (dto.Tags != null)
        {
            task.Tags.Clear();
            foreach (var tag in await ResolveTagsAsync(dto.Tags))
                task.Tags.Add(tag);
        }

        return await _tasks.UpdateAsync(task);
    }

    public Task<bool> DeleteAsync(int id) =>
        _tasks.DeleteAsync(id);

    private async Task<List<Tag>> ResolveTagsAsync(List<string>? names)
    {
        if (names == null || names.Count == 0) return new();

        var clean = names.Select(n => n.Trim())
                         .Where(n => n.Length > 0)
                         .Distinct()
                         .ToList();

        var existing = await _tags.GetByNamesAsync(clean);

        var toCreate = clean
            .Except(existing.Select(e => e.Name))
            .Select(n => new Tag { Name = n })
            .ToList();

        if (toCreate.Count > 0)
            await _tags.AddRangeAsync(toCreate);

        return existing.Concat(toCreate).ToList();
    }
}