using finalfinalfinal.Models;

namespace finalfinalfinal.Models;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Color { get; set; } = "#3b82f6";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<TaskItem> Tasks { get; set; } = new();
}