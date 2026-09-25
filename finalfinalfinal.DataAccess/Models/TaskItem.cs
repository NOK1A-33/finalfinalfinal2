using finalfinalfinal.Models;

namespace finalfinalfinal.Models;

public enum Priority { Low, Medium, High, Urgent }

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public Priority Priority { get; set; } = Priority.Medium;
    public bool Completed { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int? ProjectId { get; set; }
    public Project? Project { get; set; }

    public List<Tag> Tags { get; set; } = new();
} 