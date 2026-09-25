using Microsoft.EntityFrameworkCore;
using finalfinalfinal.Data;
using finalfinalfinal.Models;
using finalfinalfinal.Repositories.Interfaces;

namespace finalfinalfinal.Repositories;

public class TagRepository : ITagRepository
{
    private readonly AppDbContext _db;
    public TagRepository(AppDbContext db) => _db = db;

    public Task<List<Tag>> GetByNamesAsync(IEnumerable<string> names) =>
        _db.Tags.Where(t => names.Contains(t.Name)).ToListAsync();

    public async Task<List<Tag>> AddRangeAsync(IEnumerable<Tag> tags)
    {
        var list = tags.ToList();
        await _db.Tags.AddRangeAsync(list);
        await _db.SaveChangesAsync();
        return list;
    }
}