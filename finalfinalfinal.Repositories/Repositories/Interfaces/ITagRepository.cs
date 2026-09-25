using finalfinalfinal.Models;

namespace finalfinalfinal.Repositories.Interfaces;

public interface ITagRepository
{
    Task<List<Tag>> GetByNamesAsync(IEnumerable<string> names);
    Task<List<Tag>> AddRangeAsync(IEnumerable<Tag> tags);
}