using finalfinalfinal.DTOs;
using finalfinalfinal.Models;
using finalfinalfinal.Services.Filters;

namespace finalfinalfinal.Services;

public interface ITaskService
{
    Task<List<TaskItem>> GetAllAsync(TaskFilter filter);
    Task<TaskItem?> GetByIdAsync(int id);
    Task<TaskItem> CreateAsync(TaskCreateDto dto);
    Task<TaskItem?> UpdateAsync(int id, TaskUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}