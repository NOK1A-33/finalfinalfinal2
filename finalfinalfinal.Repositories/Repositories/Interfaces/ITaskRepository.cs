using finalfinalfinal.Models;
using finalfinalfinal.Services.Filters;

namespace finalfinalfinal.Repositories.Interfaces;

public interface ITaskRepository
{
    Task<List<TaskItem>> GetAllAsync(TaskFilter filter);
    Task<TaskItem?> GetByIdAsync(int id);
    Task<TaskItem> AddAsync(TaskItem task);
    Task<TaskItem?> UpdateAsync(TaskItem task);   // пересохраняет
    Task<bool> DeleteAsync(int id);               // удаляет + сохраняет
    Task<bool> ExistsAsync(int id);
}