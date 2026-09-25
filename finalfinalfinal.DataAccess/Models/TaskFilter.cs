using finalfinalfinal.Models;

namespace finalfinalfinal.Services.Filters;

public class TaskFilter
{
    public string? Search { get; set; }
    public string? Tag { get; set; }
    public Priority? Priority { get; set; }
    public int? ProjectId { get; set; }
    public string? Range { get; set; }   // "day" | "week" | "month" | null
}